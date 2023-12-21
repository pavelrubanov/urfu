using System;

public class Node<T> where T : IComparable<T>
{
    public T Value;
    public Node<T> Left;
    public Node<T> Right;

    public Node(T value)
    {
        Value = value;
    }
}

public class BinaryTree<T> where T : IComparable<T>
{
    public Node<T> Root;

    public void Add(T value)
    {
        Root = InsertNode(Root, value);
    }

    private Node<T> InsertNode(Node<T> node, T value)
    {
        if (node == null)
        {
            return new Node<T>(value);
        }

        if (value.CompareTo(node.Value) >= 0)
        {
            node.Right = InsertNode(node.Right, value);
        }
        else
        {
            node.Left = InsertNode(node.Left, value);
        }

        return node;
    }

    public IEnumerable<T> InOrderTraversal()
    {
        if (Root == null)
        {
            yield break;
        }

        Stack<Node<T>> stack = new Stack<Node<T>>();
        Node<T> current = Root;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            current = stack.Pop();
            yield return current.Value;

            current = current.Right;
        }
    }

    public static BinaryTree<T> Merge(BinaryTree<T> tree1, BinaryTree<T> tree2)
    {
        if (tree1.Root == null)
        {
            return tree2;
        }

        if (tree2.Root == null)
        {
            return tree1;
        }

        Node<T> mergedRoot = MergeNodes(tree1.Root, tree2.Root);
        return new BinaryTree<T> { Root = mergedRoot };
    }

    private static Node<T> MergeNodes(Node<T> node1, Node<T> node2)
    {
        if (node1 == null)
        {
            return node2;
        }

        if (node2 == null)
        {
            return node1;
        }

        if (node1.Value.CompareTo(node2.Value) <= 0)
        {
            node1.Right = MergeNodes(node1.Right, node2);
            return node1;
        }
        else
        {
            node2.Right = MergeNodes(node1, node2.Right);
            return node2;
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        BinaryTree<int> tree1 = new BinaryTree<int>();
        tree1.Add(1);
        tree1.Add(3);
        tree1.Add(5);

        BinaryTree<int> tree2 = new BinaryTree<int>();
        tree2.Add(2);
        tree2.Add(4);
        tree2.Add(6);

        BinaryTree<int> mergedTree = BinaryTree<int>.Merge(tree1, tree2);

        // Вывод элементов объединенного дерева
        foreach (int value in mergedTree.InOrderTraversal())
        {
            Console.WriteLine(value);
        }
    }
}
