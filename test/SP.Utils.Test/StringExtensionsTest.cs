using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SP.Test
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [TestCase("single", ExpectedResult = "Single")]
        [TestCase("UPPER", ExpectedResult = "Upper")]
        [TestCase("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG", ExpectedResult = "The quick brown fox jumped over the lazy dog")]
        [TestCase("2DAY", ExpectedResult = "2Day")]
        [TestCase("60second", ExpectedResult = "60Second")]
        [TestCase("", ExpectedResult = "")]
        [TestCase("    ", ExpectedResult = "    ")]
        [TestCase("a", ExpectedResult = "A")]
        public string TestCapitalize(string value) => value.Capitalize();

        [TestCase("First", ExpectedResult = "first")]
        [TestCase("tree", ExpectedResult = "tree")]
        [TestCase("A", ExpectedResult = "a")]
        [TestCase("", ExpectedResult = "")]
        public string TestDeCapitalize(string value) => value.Decapitalize();

        [TestCase("part", ExpectedResult = "parts")]
        [TestCase("turkey", ExpectedResult = "turkies")]
        [TestCase("lady", ExpectedResult = "ladies")]
        [TestCase("", ExpectedResult = "")]
        [TestCase("a", ExpectedResult = "a's")]
        [TestCase("tooth", ExpectedResult = "tooths")]
        [TestCase("funny", ExpectedResult = "funnies")]
        [TestCase("pass", ExpectedResult = "passes")]
        [TestCase("passes", ExpectedResult = "passes")]
        [TestCase("funnies", ExpectedResult = "funnies")]
        public string TestToPlural(string value) => value.ToPlural();

        [TestCase("First word", ' ', ExpectedResult = "First")]
        [TestCase("QC_TEST_PKG.TEST_METHOD", '.', ExpectedResult = "QC_TEST_PKG")]
        public string TestGetToken(string phrase, char delimiter)
        {
            var (token, rest) = phrase.GetToken(delimiter);
            Assert.That(token, Is.Not.Null.And.Not.Empty);
            Assert.That(rest, Is.Not.Null.And.Not.Empty);
            Assert.That(phrase, Does.StartWith(token));
            Assert.That(phrase, Does.EndWith(rest));
            return token;
        }

        [TestCase("Color", "Colour", TextDivisions.Letter, 4, 1, ExpectedResult = "ur")]
        [TestCase("Divide", "Division", TextDivisions.Letter, 4, 2, ExpectedResult = "sion")]
        [TestCase("testphrases", "testphr@$3s", TextDivisions.Letter, 7, 4, ExpectedResult = "@$3s")]
        [TestCase("shorter", "shoutier", TextDivisions.Letter, 3, 4, ExpectedResult = "utier")]
        [TestCase("longer", "ong", TextDivisions.Letter, 0, 6, ExpectedResult = "ong")]
        [TestCase("", "", TextDivisions.Letter, -1, 0, ExpectedResult = null)]
        [TestCase("a", "", TextDivisions.Letter, 0, 1, ExpectedResult = "")]
        [TestCase("", "a", TextDivisions.Letter, 0, 0, ExpectedResult = "a")]
        [TestCase("a", "a", TextDivisions.Letter, -1, 0, ExpectedResult = null)]
        [TestCase("test", "test", TextDivisions.Letter, -1, 0, ExpectedResult = null)]
        [TestCase("hi there", "you there", TextDivisions.Word, 0, 2, ExpectedResult = "you")]
        [TestCase("what's new?", "what's old?", TextDivisions.Word, 7, 4, ExpectedResult = "old?")]
        [TestCase("the quick brown fox jumped", "the slow green turtle jumped", TextDivisions.Word, 4, 15, ExpectedResult = "slow green turtle")]
        [TestCase("", "", TextDivisions.Word, -1, 0, ExpectedResult = null)]
        [TestCase("test", "", TextDivisions.Word, 0, 4, ExpectedResult = "")]
        [TestCase("", "test", TextDivisions.Word, 0, 0, ExpectedResult = "test")]
        [TestCase("test", "test", TextDivisions.Word, -1, 0, ExpectedResult = null)]
        [TestCase("same phrase", "same phrase", TextDivisions.Word, -1, 0, ExpectedResult = null)]
        [TestCase("multi-line text\r\nanother line", "multi-line data\r\nanother line", TextDivisions.Line, 0, 15, ExpectedResult = "multi-line data")]
        [TestCase("first line\r\nsecond line\r\nthird line", "first line\r\nlast line", TextDivisions.Line, 12, 23, ExpectedResult = "last line")]
        [TestCase("first line\r\nsecond line\r\nthird line", "first line\r\n2nd line\r\n3rd line", TextDivisions.Line, 12, 23, ExpectedResult = "2nd line\r\n3rd line")]
        public string TestDiffWith(string text1, string text2, TextDivisions type, int expectedStart, int expectedLength)
        {
            var (start, length, difference) = text1.DiffWith(text2, type);
            Assert.That(start, Is.EqualTo(expectedStart));
            Assert.That(length, Is.EqualTo(expectedLength));
            return difference;
        }

        [TestCase("multi_word_name", ExpectedResult = "MultiWordName")]
        [TestCase("singlewordname", ExpectedResult = "Singlewordname")]
        [TestCase("in_transit", ExpectedResult = "InTransit")]
        [TestCase("", ExpectedResult = "")]
        public string TestToCamelCase(string value) => value.ToCamelCase();

        [TestCase("THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG", ExpectedResult = "The Quick Brown Fox Jumped Over The Lazy Dog")]
        [TestCase("willy  beamish", ExpectedResult = "Willy Beamish")]
        [TestCase("1ST 2ND 3RD 4TH 5TH 6TH 7TH 8TH 9TH 10TH 32ND", ExpectedResult = "1st 2nd 3rd 4th 5th 6th 7th 8th 9th 10th 32nd")]
        [TestCase("1234 SESAME ST", ExpectedResult = "1234 Sesame St")]
        [TestCase("", ExpectedResult = "")]
        [TestCase("    ", ExpectedResult = "    ")]
        [TestCase("a", ExpectedResult = "A")]
        public string TestToProperCase(string value) => value.ToProperCase();

        [TestCase("test", '\'', ExpectedResult = "'test'")]
        [TestCase("Jamin's", '\'', ExpectedResult = "'Jamin''s'")]
        [TestCase("'what whierd?'", '\'', ExpectedResult = "'''what whierd?'''")]
        [TestCase("for Pete's sake", '"', ExpectedResult = "\"for Pete's sake\"")]
        [TestCase("", '"', ExpectedResult = "\"\"")]
        public string TestToQuotedString(string value, char quoteChar) => value.ToQuotedString(quoteChar);

        [TestCase("tests", ExpectedResult = "test")]
        [TestCase("funnies", ExpectedResult = "funny")]
        [TestCase("addresses", ExpectedResult = "address")]
        [TestCase("silly", ExpectedResult = "silly")]
        [TestCase("guess", ExpectedResult = "guess")]
        [TestCase("zones", ExpectedResult = "zone")]
        [TestCase("", ExpectedResult = "")]
        [TestCase("x", ExpectedResult = "x")]
        public string TestToSingular(string value) => value.ToSingular();

        [TestCase("TestCase", ExpectedResult = "test_case")]
        [TestCase("    anotherOne    ", ExpectedResult = "    another_one    ")]
        [TestCase("", ExpectedResult = "")]
        [TestCase("   ", ExpectedResult = "   ")]
        [TestCase("12345", ExpectedResult = "12345")]
        [TestCase("testID", ExpectedResult = "test_id")]
        [TestCase("softwareDev", ExpectedResult = "software_dev")]
        [TestCase("ApesOfWrath", ExpectedResult = "apes_of_wrath")]
        [TestCase("oS", ExpectedResult = "os")]
        [TestCase("TPSReport", ExpectedResult = "tps_report")]
        [TestCase("AReallyGoodIdea", ExpectedResult = "a_really_good_idea",
            IgnoreReason = "In order to make this work, the algorithm would need to tell the difference between single-letter words and acronyms.")]
        public string TestToUnderscores(string value) => value.ToUnderscores();

        [TestCase("'test'", '\'', ExpectedResult = "test")]
        [TestCase("'test", '\'', ExpectedResult = "'test")]
        [TestCase("\"Super fly\"", '\'', ExpectedResult = "\"Super fly\"")]
        [TestCase("\"What's the word?\"", '"', ExpectedResult = "What's the word?")]
        [TestCase("a's", '\'', ExpectedResult = "a's")]
        [TestCase("", '\'', ExpectedResult = "")]
        public string TestToUnQuotedString(string value, char quoteChar) => value.ToUnQuotedString(quoteChar);
    }
}
