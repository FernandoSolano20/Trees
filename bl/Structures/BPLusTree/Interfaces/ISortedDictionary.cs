using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bl.Structures.BPLusTree.Interfaces
{
    public interface ISortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        ISortedDictionaryCursor<TKey, TValue> Cursor();
    }

    public interface ISortedDictionaryCursor<TKey, TValue>
    {
        bool MoveTo(TKey key);
        bool Next();
        bool Previous();
        void First();
        void Last();
    }
}
