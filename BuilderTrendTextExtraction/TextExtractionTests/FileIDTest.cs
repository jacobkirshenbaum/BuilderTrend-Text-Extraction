using NUnit.Framework;

namespace Capstone.TextExtractionTests;

[TestFixture]
public class FileIDTest
{
    [Test]
    public void testIncrement()
    {
        Assert.AreEqual(1, FileID.newId());
        Assert.AreEqual(2, FileID.newId());
        Assert.AreEqual(3, FileID.newId());
    }
}