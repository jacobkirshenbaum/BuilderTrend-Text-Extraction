// Class for testing out tools
using TikaOnDotNet.TextExtraction;
using Nest;


namespace TestProgram
{
    class ToolTest
    {

        public static ElasticClient Client()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))  
                .EnableDebugMode()
                .DisableDirectStreaming()
                .DefaultIndex("files");                                               
            return new ElasticClient(settings);                                 
        }

        public static async Task Index(Doc doc)
        {
            await Client().IndexDocumentAsync(doc);
        }

        public static async void indexDocs()
        {
            var textExtractor = new TextExtractor();
            var directory = new DirectoryInfo("C:\\Users\\jakek\\Desktop\\CapTest\\TestFiles");
            var es = Client();
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
                    await Index(document);
                }
            }
        }
        
        public static void Main(string[] args)
        {
            indexDocs();
            
            var searchResponse = Client().Search<Doc>(search => search
                .AllIndices()
                .From(0)
                .Size(10)
                .Query(query => query
                    .Match(match => match
                        .Field(field => field.FileName)
                        .Query("Project Plan.docx")
                    )
                )  
            );
            if (!searchResponse.IsValid)
            {
                Console.WriteLine(searchResponse.DebugInformation);
                //Console.WriteLine(searchResponse.ServerError.Error);
            }
            var docs = searchResponse.Documents;
            //Console.WriteLine(docs.Count);
            foreach (var doc in docs)
            {
                Console.WriteLine("TESTING " + doc.FileName);
            }  
        }
    }

    public class Doc
    {
        public string? FileName { get; set; }

        public string? Path { get; set; }

        public string? Text { get; set;  }
        
    }
    
}