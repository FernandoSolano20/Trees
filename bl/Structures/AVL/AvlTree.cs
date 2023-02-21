using System.Text;

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
}
