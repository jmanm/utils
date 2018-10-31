using System;
using System.Collections.Generic;
using System.Text;

namespace SP.Containers
{
    public class StringTable : Dictionary<string, string>
    {
        public new string this[string key]
        {
            get => ContainsKey(key) ? base[key] : null;
            set => base[key] = value;
        }
    }
}
