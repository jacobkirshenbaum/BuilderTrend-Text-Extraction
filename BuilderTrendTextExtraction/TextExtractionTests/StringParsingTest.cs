using NUnit.Framework;
using System.Text.RegularExpressions;

namespace Capstone.TextExtractionTests;

[TestFixture]
public class StringParsingTest
{
    [Test]
    public void getZipcode()
    {
        string result = RegexUtilities.Zipcode;
        Assert.AreEqual(result, @"\b(\d{5}(?:-\d{4})?)\b");
    }

    [Test]
    public void zipcodeMatches()
    {
        Regex rx = new Regex(RegexUtilities.Zipcode);
        string input = "Some filler text here 68104 asdfasdf 456 sadkflasfd 1234 asdf 12345 ;lkjaasdf 78945-4567";
        MatchCollection matches = rx.Matches(input);
        Assert.AreEqual(3, matches.Count);

        string[] expected = {"68104", "12345", "78945-4567"};
        int i = 0;
        foreach (Match match in matches)
        {
            Assert.AreEqual(expected[i], match.Value);
            i++;
        }
    }
    
    [Test]
    public void fullStateMatches()
    {
        Regex rx = new Regex(RegexUtilities.FullState);
        string input = "What's your favorite state? Mine's Nebraska. Not nebraska, not Florida, not Coloroado (or Colorado), not even California. Just our humble NE";
        MatchCollection matches = rx.Matches(input);
        Assert.AreEqual(4, matches.Count);

        // Demonstrates that it only matches the full states that are spelled correctly. Highlights that nebraska, Coloroado, and NE don't match
        string[] expected = {"Nebraska", "Florida", "Colorado", "California"};
        int i = 0;
        foreach (Match match in matches)
        {
            Assert.AreEqual(expected[i], match.Value);
            i++;
        }
    }

    [Test]
    public void shortState()
    {
        Regex rx = new Regex(RegexUtilities.ShortState);
        string input = "Nebraska and NE are the same state, but ME MI and MO are all different! And AB MU and MY are not states, so they won't match";
        MatchCollection matches = rx.Matches(input);
        Assert.AreEqual(4, matches.Count);

        string[] expected = {"NE", "ME", "MI", "MO"};
        int i = 0;
        foreach (Match match in matches)
        {
            Console.WriteLine($"Expected {expected[i]} and got {match.Value}");
            Assert.AreEqual(expected[i], match.Value);
            i++;
        }
    }
    
    [Test]
    public void addressMatches()
    {
        Regex rx = new Regex(RegexUtilities.Address);
        string input = "blah blah blah 3456 Pine St. Omaha, NE 68124 text text asdf 187 Main Street \n Fake city, Maine 78415 asdlfwea ;lxfkn";
        MatchCollection matches = rx.Matches(input);
        Assert.AreEqual(2, matches.Count);

        string[] expected = {"3456 Pine St. Omaha, NE 68124", "187 Main Street \n Fake city, Maine 78415"};
        int i = 0;
        foreach (Match match in matches)
        {
            Assert.AreEqual(expected[i], match.Value);
            i++;
        }
    }

}