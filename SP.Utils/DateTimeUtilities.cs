using System;
using System.Collections.Generic;
using System.Text;

namespace SP
{
    public static class DateTimeExtensions
    {
        public static DateTime BeginnningOfTheMonth(this DateTime target)
        {
            return new DateTime(target.Year, target.Month, 1);
        }

        public static DateTime EndOfTheMonth(this DateTime target)
        {
            return new DateTime(target.Year, target.Month, DateTime.DaysInMonth(target.Year, target.Month));
        }

        public static bool IsWeekend(this DateTime target)
        {
            return target.DayOfWeek == DayOfWeek.Sunday || target.DayOfWeek == DayOfWeek.Saturday;
        }
    }

    public static class DateTimeUtilities
    {
        public static int GetWeekDayOrdinalPosition(DateTime aDate)
        {
            return ((aDate.Day - 1) / 7) + 1;
        }

        public static DateTime GetNthWeekdayInMonth(DayOfWeek weekday, int n, DateTime value)
        {
            DateTime firstDay = value.BeginnningOfTheMonth();
            if (weekday >= firstDay.DayOfWeek) n -= 1;
            if (n < 0) return firstDay;
            DateTime result = firstDay + TimeSpan.FromDays(weekday - firstDay.DayOfWeek + (n * 7));
            DateTime lastDay = value.EndOfTheMonth();
            if (result > lastDay) return lastDay;
            return result;
        }

        public static bool IsLastWeekDayInMonth(DateTime aDate, DayOfWeek weekDay)
        {
            return aDate.DayOfWeek == weekDay && (aDate + TimeSpan.FromDays(7)).Month != aDate.Month;
        }
    }
}
