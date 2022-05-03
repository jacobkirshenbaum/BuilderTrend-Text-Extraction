using System.Windows;
using Capstone.UI;
using Nest;
using Directory = System.IO.Directory;

class Program
{
    
    private static IElasticClient _client;
    
    [STAThread]
    static void Main(string[] args)
    {
        //FileProcessor processor = new();
        //GoogleStorage storage = new();
        //ElasticAccess elastic = new();
        //List<File> files = processor.ReadFiles("..\\..\\..\\..\\TestFiles");
        //storage.AddFiles(files);
        //String[] files = Directory.GetFiles("..\\..\\..\\..\\TestFiles");
        //processor.ReadFilesAlternative(files);
        //elastic.IndexDocuments(files);
        //elastic.DeleteAll();
        MainWindow main = new();
        Application app = new Application();
        app.Run(main);
    }
}
