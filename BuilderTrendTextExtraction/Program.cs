using System.Windows;
using Capstone.UI;
using Nest;
using Directory = System.IO.Directory;

/**
 * This class acts as the entry point of the program. It contains the Main function of the
 * application, which is the first function that is executed when the program is run.
 * Instead of defining any additional behaviors or functionalities, it simply calls the high
 * level functions and objects that are defined the other project files. It launches the
 * GUI of our application.
 */
class Program
{
    /**
     * The ElasticClient to connect with the ElasticSearch server.
     */
    private static IElasticClient _client;
    
    /**
     * The Main function of our program, which is the entry point of the program.
     * It is responsible for launching the main GUI.
     * @param args A list of arguments to the program (is not currently used in the function)
     */
    [STAThread]
    static void Main(string[] args)
    {
        FileProcessor processor = new();
        //GoogleStorage storage = new();
        //ElasticAccess elastic = new();
        //List<File> files = processor.ReadFiles("..\\..\\..\\..\\TestFiles");
        //storage.AddFiles(files);
        String[] files = Directory.GetFiles("..\\..\\..\\..\\TestFiles");
        processor.ReadFilesAlternative(files);
        //elastic.IndexDocuments(files);
        //elastic.DeleteAll();
        MainWindow main = new();
        Application app = new Application();
        app.Run(main);
    }
}
