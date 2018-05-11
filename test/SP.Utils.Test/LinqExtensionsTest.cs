using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Test
{
    [TestFixture]
    public class LinqExtensionsTest
    {
        [Test]
        public void TestNone()
        {
            var array = new[] { 2, 4, 6, 8 };
            Assert.That(array.None(i => i % 2 == 1));
            Assert.That(new string[0].None(), Is.True);
        }

        [Test]
        public void TestCompact()
        {
            var array = new[] { "aaa", "bbb", null };
            var compacted = array.Compact().ToArray();
            Assert.That(compacted, Has.Length.EqualTo(array.Length - 1));
            Assert.That(compacted.None(s => s == null), Is.True);
        }
    }
}
