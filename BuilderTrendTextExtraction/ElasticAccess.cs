// Access to Elasticsearch client and indexing files

using Nest;

using ConfigurationManager = System.Configuration.ConfigurationManager;

/**
 * Manages the connection with ElasticSearch. Performs all operations that
 * are related to ElasticSearch including indexing files and performing queries
 * to find files in Elasticsearch that satisfy a condition.
 */
public class ElasticAccess
{
    /**
     * The Elasticsearch client that handles the connection with ElasticSearch.
     */
    private IElasticClient _client;
    
    /**
     * Creates an ElasticAccess object.
     */
    public ElasticAccess()
    {
        _client = Client();
    }
    
    /**
     * Creates Elasticsearch client.
     * @return Returns the new Elasticsearch client.
     */
    public ElasticClient Client()
    {
        var settings = new ConnectionSettings(new Uri(ConfigurationManager.AppSettings.Get("elastic.url")))
            .DefaultIndex("files")
            .BasicAuthentication(ConfigurationManager.AppSettings.Get("elastic.user"), ConfigurationManager.AppSettings.Get("elastic.password"));
        return new ElasticClient(settings);                                 
    }

    /**
     * Indexes a single file into Elasticsearch.
     * @param file The file to be indexed.
     */
    public void IndexDocument(File file)
    {
        _client.IndexDocument(file);
    }

    /**
     * Indexes a list of files into Elasticsearch.
     * @param files A list of files to be indexed.
     */
    public void IndexDocuments(List<File> files)
    {
        foreach (var file in files)
        {
            _client.IndexDocument(file);
        }
    }

    /**
     * Returns the first 100 files currently in Elasticsearch.
     * @return Returns a list of the first 100 files in Elasticsearch.
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
     * Finds the files in Elasticsearch that have the given file name.
     * @param fileName The name of a file to search for.
     * @return Returns list of first 100 files currently in Elasticsearch whose file
     *         names contain the given file name.
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
     * Finds the files in Elasticsearch that contain the given substring in their text.
     * @param text A substring to search for.
     * @return Returns the list of first 100 files currently in Elasticsearch that contain the given text.
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
     * Finds the files in Elasticsearch that contain the given US postal address.
     * @param text An address to search for.
     * @return Returns the list of first 100 files currently in Elasticsearch that contain the given address.
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
     * Finds the files in Elasticsearch that contain the given phone number.
     * @param text A phone number to search for.
     * @return Returns the list of first 100 files currently in Elasticsearch that contain the given address.
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
     * Finds the files in Elasticsearch that contain the given email address.
     * @param text An email address to search for.
     * @return Returns the list of first 100 files currently in Elasticsearch that contain the given address
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
     * Deletes all files currently in Elasticsearch.
     */
    public void DeleteAll()
    {
        _client.DeleteByQueryAsync<File>(delete => delete
            .Query(query => query.MatchAll())
        );
    }

}
