using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace SP
{
    public class Range
    {
        public IComparable LowValue { get; set; }
        public IComparable HighValue { get; set; }
        public static readonly Range Empty = new Range(null, null);

        public Range(IComparable lowValue, IComparable highValue)
        {
            if (lowValue != null && highValue != null && lowValue.CompareTo(highValue) == 1)
                throw new ArgumentOutOfRangeException("highValue");
            this.LowValue = lowValue;
            this.HighValue = highValue;
        }

        private static bool AreEqual(Range r, object obj)
        {
            if (ReferenceEquals(r, null) && ReferenceEquals(r, null))
                return true;
            if (ReferenceEquals(r, null) || ReferenceEquals(r, null))
                return false;
            if (obj is Range r2)
            {
                return r.LowValue != null && r.HighValue != null && r2.LowValue != null && r2.HighValue != null &&
                    r.LowValue.CompareTo(r2.LowValue) == 0 && r.HighValue.CompareTo(r2.HighValue) == 0;
            }
            return false;
        }

        public static bool operator ==(Range r1, Range r2) => AreEqual(r1, r2);

        public static bool operator !=(Range r1, Range r2) => !AreEqual(r1, r2);

        public bool Contains(IComparable value)
        {
            return (this.LowValue == null || value.CompareTo(this.LowValue) >= 0) && (this.HighValue == null || value.CompareTo(this.HighValue) <= 0);
        }

        public override bool Equals(object obj) => AreEqual(this, obj);

        public override int GetHashCode()
        {
            var result = 41 * 31 + this.LowValue?.GetHashCode() ?? 0;
            return result * 31 + this.HighValue?.GetHashCode() ?? 0;
        }

        public override string ToString() => $"{this.LowValue}..{this.HighValue}";
    }

    public class Range<T> : Range where T : IComparable
    {
        public static readonly new Range<T> Empty = new Range<T>(default(T), default(T));
        public new T LowValue { get { return (T)base.LowValue; } set { base.LowValue = value; } }
        public new T HighValue { get { return (T)base.HighValue; } set { base.HighValue = value; } }
        public Range(T lowVal, T highVal) : base(lowVal, highVal) { }
    }

    public static class RangeHelpers
    {
        public static bool IsWithin(this IComparable target, Range range) => range.Contains(target);

        public static bool Overlaps(this Range<DateTime> target, Range<DateTime> other)
        {
            return target.Union(other) > TimeSpan.Zero;
        }

        public static TimeSpan Union(this Range<DateTime> target, Range<DateTime> other)
        {
            DateTime low = target.LowValue >= other.LowValue ? target.LowValue : other.LowValue;
            DateTime high = target.HighValue <= other.HighValue ? target.HighValue : other.HighValue;
            if (high >= low)
                return high - low + TimeSpan.FromDays(1);
            return TimeSpan.Zero;
        }
    }

    public static class KeyValuePairExtenstions
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> target, out TKey item1, out TValue item2)
        {
            item1 = target.Key;
            item2 = target.Value;
        }
    }
}