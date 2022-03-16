using Nest;

class Program
{
    
    private static IElasticClient _client;
    static void Main(string[] args)
    {
        FileProcessor processor = new();
        ElasticAccess elastic = new();
        List<File> files = processor.ReadFiles("..\\..\\..\\..\\TestFiles");
        //elastic.IndexDocuemnts(files);
        //elastic.DeleteAll();
    }
}
