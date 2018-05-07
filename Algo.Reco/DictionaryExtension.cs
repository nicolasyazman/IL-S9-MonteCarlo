using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    public static class DictionaryExtension
    {
        public static TValue GetValueWithDefault<TKey, TValue>( this Dictionary<TKey, TValue> @this, TKey key, TValue def )
        {
            TValue v;
            return @this.TryGetValue( key, out v ) ? v : def;
        }

    }
}
