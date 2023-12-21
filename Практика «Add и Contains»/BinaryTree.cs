using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public class NodeOfTrees<T> where T : IComparable
    {
        public T Value;
        public NodeOfTrees<T> Right, Left;
    }

    public class BinaryTree<T> where T : IComparable, IEnumerable<T>
    {
        public NodeOfTrees<T> Root;

        public void Add(T key)
        {
            if (Equals(Root, null)) Root = new NodeOfTrees<T> { Value = key };
            else
            {
                var node = Root;
                var source = Root;
                while (!Equals(node, null))
                {
                    source = node;
                    node = node.Value.CompareTo(key) < 0 ? node.Right : node.Left;
                }
                if (source.Value.CompareTo(key) < 0)
                    source.Right = new NodeOfTrees<T> { Value = key };
                else
                    source.Left = new NodeOfTrees<T> { Value = key };
            }
        }

        public bool Contains(T key)
        {
            var node = Root;
            while (!Equals(node, null))
            {
                if (node.Value.CompareTo(key) == 0)
                    return true;
                node = node.Value.CompareTo(key) < 0 ? node.Right : node.Left;
            }
            return false;
        }
    }
}
