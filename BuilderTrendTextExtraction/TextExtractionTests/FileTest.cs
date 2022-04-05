using NUnit.Framework;

namespace Capstone.TextExtractionTests;

[TestFixture]
public class FileTest
{
    [Test]
    public void canInitialize()
    {
        File file = new File()
        {
            ID = 1,
            FileName = "TestFile.txt",
            Text = "File Contents"
        };
        Assert.AreEqual("TestFile.txt", file.FileName);
        Assert.AreEqual("File Contents", file.Text);
        Assert.AreEqual(1, file.ID);
    }
}