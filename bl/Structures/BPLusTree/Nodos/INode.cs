using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bl.Structures.BPLusTree.Nodos
{
    public interface INode<TKey, TValue>
    {
        List<TKey> Keys { get; }
        INode<TKey, TValue> Split();
        int Count { get; }
        void AddRange(INode<TKey, TValue> node, IComparer<TKey> comparer);
        void AddFromLeft(INode<TKey, TValue> node, IComparer<TKey> comparer);
        void AddFromRight(INode<TKey, TValue> node, IComparer<TKey> comparer);
    }
}
