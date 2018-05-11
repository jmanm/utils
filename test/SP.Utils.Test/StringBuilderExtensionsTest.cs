using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Test
{
    [TestFixture]
    public class StringBuilderExtensionsTest
    {
        private StringBuilder sb;

        [SetUp]
        public void Setup()
        {
            sb = new StringBuilder();
        }

        [TestCase("test", "case", ExpectedResult = "test case")]
        [TestCase("", "Start", ExpectedResult = "Start")]
        public string TestAppendToken(string startValue, string appendValue)
        {
            sb.Append(startValue);
            sb.AppendToken(appendValue);
            return sb.ToString();
        }

        [Test]
        public void TestAppendTokenToExisting()
        {
            Assert.That(sb.AppendToken("shut").ToString(), Is.EqualTo("shut"));
            Assert.That(sb.AppendToken("the ").ToString(), Is.EqualTo("shut the "));
            Assert.That(sb.AppendToken("front door").ToString(), Is.EqualTo("shut the front door"));
        }

        [Test]
        public void TestGetToken()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => sb.GetToken(0));
            sb.Append("   work hard play hard   ");
            Assert.That(sb.GetToken(0), Is.EqualTo("work").And.EqualTo(sb.GetToken(3)));
            Assert.That(sb.GetToken(4), Is.EqualTo("ork"));
            Assert.That(sb.GetToken(7), Is.EqualTo("hard").And.EqualTo(sb.GetToken(8).ToString()));
            Assert.That(sb.GetToken(17, true), Is.EqualTo("play").And.EqualTo(sb.GetToken(16, true)));
            Assert.That(sb.GetToken(sb.Length - 1, true), Is.EqualTo("hard").And.EqualTo(sb.GetToken(sb.Length - 4, true)));
            Assert.Throws<ArgumentOutOfRangeException>(() => sb.GetToken(sb.Length));
        }

        [Test]
        public void TestInsertToken()
        {
            sb.Append("alphabet");
            Assert.That(sb.InsertToken(sb.Length, "soup").ToString(), Is.EqualTo("alphabet soup"));
            Assert.That(sb.InsertToken(5, "-").ToString(), Is.EqualTo("alpha - bet soup"));
            Assert.That(sb.InsertToken(0, "alf").ToString(), Is.EqualTo("alf alpha - bet soup"));
        }
    }
}
