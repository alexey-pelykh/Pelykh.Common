using System.Collections.Generic;

namespace Pelykh.Common.Collections
{
    public class AutoDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>
        , IDictionary<TKey, TValue>
        where TValue : new()
    {
        public new TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (base.TryGetValue(key, out value))
                    return value;
                base[key] = value = new TValue();
                return value;
            }
            set
            {
                base[key] = value;
            }
        }
    }
}
