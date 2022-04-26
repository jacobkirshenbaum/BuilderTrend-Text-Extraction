using System.Diagnostics;
using System.IO;
using GroupDocs.Parser;
using NUnit.Framework;
using TikaOnDotNet.TextExtraction;

namespace Capstone.TextExtractionTests;

[TestFixture]
public class TextExtractionTest
{
    private FileProcessor _fileProcessor;
    private TextExtractor _textExtractor;

    [SetUp]
    public void SetUp()
    {
        _fileProcessor = new();
        _textExtractor = new();
    }

    [Test]
    public void isExtracting()
    {
        Assert.IsNotEmpty(_fileProcessor.ReadFiles(@"..\..\..\..\TestFiles"));
    }

    [Test]
    public void ExtractionTime()
    {
        Stopwatch stopwatch = new();
        stopwatch.Restart();
        extractTextTika(100);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
        stopwatch.Restart();
        extractTextGroup(100);
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
    }

    private void extractTextTika(int times)
    {
        while (times-- > 0)
        {
            var contents = _textExtractor.Extract(@"..\..\..\..\TestFiles\bill-of-sale.docx");
        }
    }

    private void extractTextGroup(int times)
    {
        while (times-- > 0)
        {
            Parser parser = new Parser(@"..\..\..\..\TestFiles\bill-of-sale.docx");
            TextReader reader = parser.GetText();
            reader.ReadToEnd();
        }
    }

}