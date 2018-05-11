using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> target, IEnumerable<KeyValuePair<TKey, TValue>> value)
        {
            if (target == null)
                throw new NullReferenceException();
            if (value == null)
                throw new ArgumentNullException("value)");
            foreach (var kvp in value)
                target.Add(kvp.Key, kvp.Value);
        }
    }
}
