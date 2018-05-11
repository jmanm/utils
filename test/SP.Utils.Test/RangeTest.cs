using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Test
{
    [TestFixture]
    public class RangeTest
    {
        [Test]
        public void TestRangeEquality()
        {
            var r1 = new Range<DateTime>(DateTime.Parse("2015-12-14"), DateTime.Parse("2015-12-19"));
            var r2 = new Range<DateTime>(DateTime.Parse("2015-12-14"), DateTime.Parse("2015-12-19"));
            Assert.That(r1, Is.EqualTo(r2));
        }
    }
}
