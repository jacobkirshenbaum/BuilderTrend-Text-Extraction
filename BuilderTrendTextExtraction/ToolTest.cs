// Class for testing out tools

using System.Security.Cryptography.X509Certificates;
using Elasticsearch.Net;
using TikaOnDotNet.TextExtraction;
using Nest;


namespace Capstone
{
    class ToolTest
    {
        private static IElasticClient _client;
        public static ElasticClient Client()
        {
            var settings = new ConnectionSettings(new Uri("https://localhost:9200"))
                .DefaultIndex("docs")
                .BasicAuthentication("elastic", "T3qU2DkD8SSn_4nEC3Uc")
                .ServerCertificateValidationCallback(CertificateValidations.AuthorityIsRoot(new X509Certificate("C:\\Users\\jakek\\http_ca.crt")))
                .DefaultMappingFor<Doc>(doc => doc.IndexName("docs"));
            return new ElasticClient(settings);                                 
        }

        public static async Task Index(Doc doc)
        {
            await _client.IndexDocumentAsync(doc);
        }

        public static async void indexDocs()
        {
            var textExtractor = new TextExtractor();
            var directory = new DirectoryInfo("C:\\Users\\jakek\\RiderProjects\\Capstone\\TestFiles");
            if (directory.Exists)
            {
                foreach (var file in directory.GetFiles())
                {
                    var contents = textExtractor.Extract(file.FullName);
                    var document = new Doc()
                    {
                        FileName = file.Name,
                        Path = file.FullName,
                        Text = contents.Text
                    };
                    _client.IndexDocument(document);
                }
            }
        }
        
        static void Main(string[] args)
        {
            _client = Client();
            indexDocs();
            var searchResponse = _client.Search<Doc>(search => search
                .Query(query => query
                    .MatchAll()));
            
            Console.WriteLine(searchResponse.Documents.Count);
            /*if (!searchResponse.IsValid)
            {
                Console.WriteLine(searchResponse.DebugInformation);
                //Console.WriteLine(searchResponse.ServerError.Error);
            }
            var docs = searchResponse.Documents;
            //Console.WriteLine(docs.Count);
            foreach (var doc in docs)
            {
                Console.WriteLine("TESTING " + doc.FileName);
            }  */
        }
    }

    public class Doc
    {
        public string? FileName { get; set; }

        public string? Path { get; set; }

        public string? Text { get; set;  }
        
    }
    
}