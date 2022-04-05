using NUnit.Framework;

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