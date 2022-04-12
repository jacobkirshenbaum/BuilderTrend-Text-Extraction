﻿// Class for processing files using Apache Tika

using System.IO;
using org.jdom2;
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
                                Text = page.GetText()
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
                        Text = contents.Text
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
                            Text = page.GetText()
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
                    Text = contents.Text

                };
                files.Add(document);
            }
        }
        return files;
    }

}
