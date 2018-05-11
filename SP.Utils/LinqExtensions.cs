using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Compact<T>(this IEnumerable<T> target) => target.Where(val => val != null); //borrowed from Ruby :)
        public static IEnumerable<T> Except<T>(this IEnumerable<T> target, Func<T, bool> predicate) => target.Where(val => !predicate(val));
        public static bool None<T>(this IEnumerable<T> target) => !target.Any();
        public static bool None<T>(this IEnumerable<T> target, Func<T, bool> predicate) => !target.Any(predicate);
        public static bool IntersectsWith<T>(this IEnumerable<T> target, IEnumerable<T> second) => target.Intersect(second).Any();
    }
}
