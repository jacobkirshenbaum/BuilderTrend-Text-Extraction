// Class for processing files using Apache Tika

using System.IO;
using System.Text.RegularExpressions;
using GroupDocs.Parser;
using TikaOnDotNet.TextExtraction;
using Tesseract;

public class FileProcessor
{
    private TextExtractor _textExtractor;
    private DirectoryInfo _directoryInfo;

    public FileProcessor()
    {
        _textExtractor = new TextExtractor();
    }

    /**
     * Returns list of files that can be put into Elasticsearch
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
     * Returns file that can be put into Elasticsearch
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
