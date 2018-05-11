using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Test
{
    [TestFixture]
    public class DateUtilsTest
    {
        [Test]
        public void TestDatePositions()
        {
            Assert.That(DateTime.Parse("4/30/1991").BeginnningOfTheMonth(), Is.EqualTo(DateTime.Parse("4/1/1991")));
            Assert.That(DateTime.Parse("11/4/2011").BeginnningOfTheMonth(), Is.EqualTo(DateTime.Parse("11/1/2011")));

            Assert.That(DateTime.Parse("2/3/2009").EndOfTheMonth(), Is.EqualTo(DateTime.Parse("2/28/2009")));
            Assert.That(DateTime.Parse("11/18/1944").EndOfTheMonth(), Is.EqualTo(DateTime.Parse("11/30/1944")));

            Assert.That(DateTimeUtilities.GetWeekDayOrdinalPosition(DateTime.Parse("1/16/2009")), Is.EqualTo(3));
            Assert.That(DateTimeUtilities.GetWeekDayOrdinalPosition(DateTime.Parse("6/24/2009")), Is.EqualTo(4));

            Assert.That(DateTimeUtilities.GetNthWeekdayInMonth(DayOfWeek.Monday, 1, DateTime.Parse("9/4/2011")),
                Is.EqualTo(DateTime.Parse("9/5/2011")));
            Assert.That(DateTimeUtilities.GetNthWeekdayInMonth(DayOfWeek.Tuesday, 2, DateTime.Parse("7/1/2011")),
                Is.EqualTo(DateTime.Parse("7/12/2011")));
            Assert.That(DateTimeUtilities.GetNthWeekdayInMonth(DayOfWeek.Thursday, 4, DateTime.Parse("11/28/2011")),
                Is.EqualTo(DateTime.Parse("11/24/2011")));

            Assert.That(DateTimeUtilities.IsLastWeekDayInMonth(DateTime.Parse("6/29/2009"), DayOfWeek.Monday));
            Assert.That(DateTimeUtilities.IsLastWeekDayInMonth(DateTime.Parse("6/22/2009"), DayOfWeek.Monday), Is.False);
            Assert.That(DateTimeUtilities.IsLastWeekDayInMonth(DateTime.Parse("10/21/2009"), DayOfWeek.Wednesday), Is.False);
            Assert.That(DateTimeUtilities.IsLastWeekDayInMonth(DateTime.Parse("10/25/2009"), DayOfWeek.Sunday));
            Assert.That(DateTimeUtilities.IsLastWeekDayInMonth(DateTime.Parse("10/28/2009"), DayOfWeek.Wednesday));
            Assert.That(DateTimeUtilities.IsLastWeekDayInMonth(DateTime.Parse("11/26/2009"), DayOfWeek.Thursday));
            Assert.That(DateTimeUtilities.IsLastWeekDayInMonth(DateTime.Parse("11/26/2009"), DayOfWeek.Friday), Is.False);
        }
    }
}
