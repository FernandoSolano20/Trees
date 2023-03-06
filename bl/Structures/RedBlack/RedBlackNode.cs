using System.Xml.Linq;

namespace bl.Structures.RedBlack;

public class RedBlackNode 
{
    public int Value { get; set; }
    public Color Color { get; set; }
    public RedBlackNode Left { get; set; }
    public RedBlackNode Right { get; set; }
    public RedBlackNode Parent { get; set; }

    public RedBlackNode(int value)
    {
        Value = value;
        Color = Color.Red;
        Left = null;
        Right = null;
        Parent = null;
    }

    public bool IsOnLeft()
    {
        return this == Parent.Left;
    }

    public RedBlackNode Sibling()
    {
        // sibling null if no parent
        if (Parent == null)
            return null;

        if (IsOnLeft())
            return Parent.Right;

        return Parent.Left;
    }

    public bool HasRedChild()
    {
        return (Left != null && Left.Color == Color.Red) ||
            (Right != null && Right.Color == Color.Red);
    }
}
