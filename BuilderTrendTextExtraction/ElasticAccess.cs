// Access to Elasticsearch client and indexing files

using Nest;

using ConfigurationManager = System.Configuration.ConfigurationManager;

public class ElasticAccess
{
    private IElasticClient _client;
    public ElasticAccess()
    {
        _client = Client();
    }
    
    /**
     * Creates Elasticsearch client
     */
    public ElasticClient Client()
    {
        var settings = new ConnectionSettings(new Uri(ConfigurationManager.AppSettings.Get("elastic.url")))
            .DefaultIndex("files")
            .BasicAuthentication(ConfigurationManager.AppSettings.Get("elastic.user"), ConfigurationManager.AppSettings.Get("elastic.password"));
        return new ElasticClient(settings);                                 
    }

    /**
     * Indexes a single file into Elasticsearch
     */
    public void IndexDocument(File file)
    {
        _client.IndexDocument(file);
    }

    /**
     * Indexes a list of files into Elasticsearch
     */
    public void IndexDocuments(List<File> files)
    {
        foreach (var file in files)
        {
            _client.IndexDocument(file);
        }
    }

    /**
     * Returns list of first 100 files currently in Elasticsearch
     */
    public List<File> SearchAll()
    {
        var searchResponse = _client.Search<File>(search => search
            .Size(100)
            .Query(query => query
                .MatchAll()));
        return searchResponse.Documents.ToList();
    }

    /**
     * Returns list of first 100 files currently in Elasticsearch who's file names contain the given file name 
     */
    public List<File> SearchByName(string fileName)
    {
        var searchResponse = _client.Search<File>(search => search
            .Size(100)
            .Query(query => query
                .Match(match => match
                    .Field(field => field.FileName)
                    .Query(fileName)
                )
            )
        );
        return searchResponse.Documents.ToList();
    }

    /**
     * Returns the list of first 100 files currently in Elasticsearch that contain the given text
     */
    public List<File> SearchByText(string text)
    {
        var searchResponse = _client.Search<File>(search => search
            .Size(100)
            .Query(query => query
                .Match(match => match
                    .Field(field => field.Text)
                    .Query(text)
                )
            )
        );
        return searchResponse.Documents.ToList();
    }
    
    /**
     * Returns the list of first 100 files currently in Elasticsearch that contain the given address
     */
    public List<File> SearchByAddress(string text)
    {
        var addressList = text.Split(' ');
        var searchResponse = _client.Search<File>(search => search
            .Size(100)
            .Query(query => +query
                .Terms(t => t
                    .Field(f => f
                        .Addresses
                    )
                    .Terms(addressList)
                )
            )
        );
        return searchResponse.Documents.ToList();
    }
    
    /**
     * Returns the list of first 100 files currently in Elasticsearch that contain the given address
     */
    public List<File> SearchByPhone(string text)
    {
        var searchResponse = _client.Search<File>(search => search
            .Size(100)
            .Query(query => query
                .Match(match => match
                    .Field(field => field.PhoneNumbers)
                    .Query(text)
                )
            )
        );
        return searchResponse.Documents.ToList();
    }
    
    /**
     * Returns the list of first 100 files currently in Elasticsearch that contain the given address
     */
    public List<File> SearchByEmail(string text)
    {
        var searchResponse = _client.Search<File>(search => search
            .Size(100)
            .Query(query => query
                .Match(match => match
                    .Field(field => field.Emails)
                    .Query(text)
                )
            )
        );
        return searchResponse.Documents.ToList();
    }

    /**
     * Deletes all files currently in Elasticsearch
     */
    public void DeleteAll()
    {
        _client.DeleteByQueryAsync<File>(delete => delete
            .Query(query => query.MatchAll())
        );
    }

}
