// Class for processing files using Apache Tika

using System.IO;
using TikaOnDotNet.TextExtraction;

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
            foreach (var file in _directoryInfo.GetFiles())
            {
                var contents = _textExtractor.Extract(file.FullName);
                var document = new File()
                {
                    ID = FileID.newId(),
                    FileName = file.Name,
                    Path = file.FullName,
                    Text = contents.Text
                };
                files.Add(document);
            }
        }
        return files;
    }

}
