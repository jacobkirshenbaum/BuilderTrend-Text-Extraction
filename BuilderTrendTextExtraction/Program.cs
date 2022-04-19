using System.Windows;
using Capstone.UI;
using Nest;

class Program
{
    
    private static IElasticClient _client;
    
    [STAThread]
    static void Main(string[] args)
    {
        //FileProcessor processor = new();
        //ElasticAccess elastic = new();
        //List<File> files = processor.ReadFiles("..\\..\\..\\..\\TestFiles");
        //elastic.IndexDocuments(files);
        //elastic.DeleteAll();
        MainWindow main = new();
        Application app = new Application();
        app.Run(main);
    }
}
