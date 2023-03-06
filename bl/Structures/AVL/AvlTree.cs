using System.Text;
using System.Xml;

namespace bl.Structures.AVL;
public class AvlTree
{
    private AvlNode _root { get; set; }

    public AvlTree()
    {
        _root = null;
    }

    public void Add(int value)
    {
        _root = Add(_root, value);
    }

    public string Draw()
    {
        return $"{Draw(_root, stringBuilder: new StringBuilder())}";
    }

    private AvlNode Add(AvlNode aux, int value)
    {
        if (aux == null)
        {
            return new AvlNode()
            {
                Value = value,
            };
        }

        if (aux.Value > value)
        {
            aux.Left = Add(aux.Left, value);
        }
        else if (aux.Value < value)
        {
            aux.Right = Add(aux.Right, value);
        }
        else
        {
            return aux;
        }

        aux.UpdateHeightAndBalanceFactor();

        if (aux.BalanceFactor > 1 && aux.Right.BalanceFactor > 0)
        {
            aux = LeftRotate(aux);
        }
        if (aux.BalanceFactor < -1 && aux.Left.BalanceFactor < 0)
        {
            aux = RightRotate(aux);
        }

        if (aux.BalanceFactor > 1 && aux.Right.BalanceFactor < 0)
        {
            aux.Right = RightRotate(aux.Right);
            aux = LeftRotate(aux);
        }
        if (aux.BalanceFactor < -1 && aux.Left.BalanceFactor > 0)
        {
            aux.Left = LeftRotate(aux.Left);
            aux = RightRotate(aux);
        }
        return aux;
    }

    private AvlNode LeftRotate(AvlNode p)
    {
        var q = p.Right;
        p.Right = q.Left;
        q.Left = p;

        p.UpdateHeightAndBalanceFactor();
        q.UpdateHeightAndBalanceFactor();
        return q;
    }

    private AvlNode RightRotate(AvlNode p)
    {
        var q = p.Left;
        p.Left = q.Right;
        q.Right = p;

        p.UpdateHeightAndBalanceFactor();
        q.UpdateHeightAndBalanceFactor();
        return q;
    }

    private StringBuilder Draw(AvlNode node, int level = 0, StringBuilder stringBuilder = null)
    {
        if (node == null)
        {
            return stringBuilder;
        }

        Draw(node.Right, level + 1, stringBuilder);

        for (int i = 0; i < level; i++)
        {
            stringBuilder.Append("    ");
        }

        stringBuilder.Append($"{node.Value}\n");

        Draw(node.Left, level + 1, stringBuilder);

        return stringBuilder;
    }

    public void Delete(int value)
    {
        _root = Delete(_root, value);
    }

    private AvlNode Delete(AvlNode node, int value)
    {
        if (node == null)
        {
            // Value not found in the tree.
            return null;
        }

        if (value < node.Value)
        {
            node.Left = Delete(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = Delete(node.Right, value);
        }
        else
        {
            // Node to be deleted found.

            if (node.Left == null && node.Right == null)
            {
                // Node has no children.
                return null;
            }
            else if (node.Left == null)
            {
                // Node has only a right child.
                return node.Right;
            }
            else if (node.Right == null)
            {
                // Node has only a left child.
                return node.Left;
            }
            else
            {
                // Node has two children.
                var successor = FindSuccessor(node.Right);
                node.Value = successor.Value;
                node.Right = Delete(node.Right, successor.Value);
            }
        }

        // Update the height and balance factor of the current node.
        node.UpdateHeightAndBalanceFactor();

        // Check if the node is unbalanced and perform rotations if necessary.
        if (node.BalanceFactor > 1)
        {
            if (node.Right != null && node.Right.BalanceFactor < 0)
            {
                node.Right = RightRotate(node.Right);
            }
            node = LeftRotate(node);
        }
        else if (node.BalanceFactor < -1)
        {
            if (node.Left != null && node.Left.BalanceFactor > 0)
            {
                node.Left = LeftRotate(node.Left);
            }
            node = RightRotate(node);
        }

        return node;
    }

    private AvlNode FindSuccessor(AvlNode node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }
}
