// Class for processing files using Apache Tika

using System.IO;
using System.Text.RegularExpressions;
using GroupDocs.Parser;
using TikaOnDotNet.TextExtraction;
using Tesseract;

/**
 * Class for processing files. It extracts text out of the files
 * using ApacheTika, Tesseract, or GroupDocs and stores the relevant
 * data in File objects.
 */
public class FileProcessor
{
    /**
     * The object responsible for extracting text out of documents.
     */
    private TextExtractor _textExtractor;
    
    /**
     * Stores information to identify the directory we are processing.
     */
    private DirectoryInfo _directoryInfo;

    /**
     * Creates a FileProcessor object.
     */
    public FileProcessor()
    {
        _textExtractor = new TextExtractor();
    }

    /**
     * Returns list of files that can be put into Elasticsearch.
     * For each file in the given directory, it extracts text from the file
     * with the appropriate tool (Tesseract for files with .jpg extension and ApacheTika for
     * everything else), and stores the data in a File object, and adds that file object to
     * the output list.
     *
     * @param filePath The filepath of the directory that is being processed.
     * @return Returns a list of File objects, each storing the relevant data for a single
     *         file in the given directory.
     */
    public List<File> ReadFiles(String filePath)
    {
        List<File> files = new();
        _directoryInfo = new DirectoryInfo(filePath);
        if (_directoryInfo.Exists)
        {
            var engine = new TesseractEngine(@"..\..\..\..\TrainingData\", "eng");
            engine.SetVariable("user_defined_dpi", "70");
            foreach (var file in _directoryInfo.GetFiles())
            {
                if (file.Extension == ".jpg")
                {
                    using (var img = Pix.LoadFromFile(file.FullName))
                    {
                        using (var page = engine.Process(img))
                        {
                            var document = new File()
                            {
                                ID = FileID.newId(),
                                FileName = file.Name,
                                Text = page.GetText(),
                                Addresses = getFilteredItems(page.GetText(), "address"),
                                PhoneNumbers = getFilteredItems(page.GetText(), "phone"),
                                Emails = getFilteredItems(page.GetText(), "email")
                            };
                            files.Add(document);
                        }
                    }
                }
                else
                {
                    var contents = _textExtractor.Extract(file.FullName);
                    var document = new File()
                    {
                        ID = FileID.newId(),
                        FileName = file.Name,
                        Text = contents.Text,
                        Addresses = getFilteredItems(contents.Text, "address"),
                        PhoneNumbers = getFilteredItems(contents.Text, "phone"),
                        Emails = getFilteredItems(contents.Text, "email")
                    };
                    files.Add(document);
                }
            }
        }
        return files;
    }
    
    /**
     * Returns file that can be put into Elasticsearch.
     * It extracts text from the given file with the appropriate tool (Tesseract for
     * files with .jpg extension and ApacheTika for everything else), and stores the
     * data in a File object.
     *
     * @param filePath The file path of the file that is being processed.
     * @return Returns a single File object storing the relevant data for a single file.
     */
    public List<File> ReadFile(String[] filePaths)
    {
        List<File> files = new();
        var engine = new TesseractEngine(@"..\..\..\..\TrainingData\", "eng");
        engine.SetVariable("user_defined_dpi", "70");
        foreach (var filePath in filePaths)
        {
            if (Path.GetExtension(filePath) == ".jpg")
            {
                using (var img = Pix.LoadFromFile(filePath))
                {
                    using (var page = engine.Process(img))
                    {
                        var document = new File()
                        {
                            ID = FileID.newId(),
                            FileName = Path.GetFileName(filePath),
                            Text = page.GetText(),
                            Addresses = getFilteredItems(page.GetText(), "address"),
                            PhoneNumbers = getFilteredItems(page.GetText(), "phone"),
                            Emails = getFilteredItems(page.GetText(), "email")
                        };
                        files.Add(document);
                    }
                }
            }
            else
            {
                var contents = _textExtractor.Extract(filePath);
                var document = new File()
                {
                    ID = FileID.newId(),
                    FileName = Path.GetFileName(filePath),
                    Text = contents.Text,
                    Addresses = getFilteredItems(contents.Text, "address"),
                    PhoneNumbers = getFilteredItems(contents.Text, "phone"),
                    Emails = getFilteredItems(contents.Text, "email")
                };
                files.Add(document);
            }
        }
        return files;
    }

    /**
     * Finds all the substrings of the given text for a given type of information.
     * The searching is done using regular expressions, but the technical details like
     * the regular expression pattern are handled internally (so the caller only needs
     * to think in terms of general category).
     *
     * @param text The string of text that is being processed and searched.
     * @param filter The type of information to search for. The only meaningful values
     *               are "address", "email", and "phone".
     * @return Returns a list of all substrings of the filtered type that occur in text.
     */
    private List<string> getFilteredItems(string text, string filter)
    {
        List<string> items = new();
        Regex regex = null;
        switch (filter)
        {
            case "address":
                regex = new Regex(RegexUtilities.Address, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                break;
            case "email":
                regex = new Regex(RegexUtilities.Email, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                break;
            case "phone":
                regex = new Regex(RegexUtilities.PhoneNumber, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                break;
        }
        MatchCollection matches = regex.Matches(text);
        foreach (Match match in matches)
        {
            items.Add(match.Value);
        }
        return items;
    }

    /**
     * Reads the files using GroupDocs and returns a list of the resulting File objects.
     * For each given file path, it extracts the text using the GroupDocs tool, creates a
     * file object to store the data, and adds it to the output list.
     *
     * @param filePaths An array of file paths to be processed.
     * @return Returns a List of file objects, where each file object stores the data
     *         of a single file.
     */
    public List<File> ReadFilesAlternative(String[] filePaths)
    {
        List<File> files = new();
        foreach (var filePath in filePaths)
        {
            Parser parser = new Parser(filePath);
            using (TextReader reader = parser.GetText())
            {
                var contents = reader.ReadToEnd();
                var document = new File()
                {
                    ID = FileID.newId(),
                    FileName = Path.GetFileName(filePath),
                    Text = contents,
                    Addresses = getFilteredItems(contents, "address"),
                    PhoneNumbers = getFilteredItems(contents, "phone"),
                    Emails = getFilteredItems(contents, "email")
                };
                files.Add(document);
            }
        }
        return files;
    }

}
