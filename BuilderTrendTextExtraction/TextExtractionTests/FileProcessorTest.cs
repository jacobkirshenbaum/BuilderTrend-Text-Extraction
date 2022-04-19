using System.IO;
using NUnit.Framework;
using TikaOnDotNet.TextExtraction;

namespace Capstone.TextExtractionTests;

[TestFixture]
public class TextExtractionTest
{
    private FileProcessor _fileProcessor;

    [SetUp]
    public void SetUp()
    {
        _fileProcessor = new();
    }

    [Test]
    public void isExtracting()
    {
        Assert.IsNotEmpty(_fileProcessor.ReadFiles(@"..\..\..\..\TestFiles"));
    }
    
}