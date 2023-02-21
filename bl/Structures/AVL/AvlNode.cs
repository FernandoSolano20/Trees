namespace bl.Structures.AVL;
public class AvlNode
{
    public int Value { get; set; }

    public int BalanceFactor { get; set; } = 0;
    public int Height { get; set; } = 1;

    public AvlNode Left { get; set; }
    public AvlNode Right { get; set; }

    public void UpdateHeightAndBalanceFactor()
    {
        int leftHeight = Left != null ? Left.Height : 0;
        int rightHeight = Right != null ? Right.Height : 0;
        Height = Math.Max(leftHeight, rightHeight) + 1;
        BalanceFactor = rightHeight - leftHeight;
    }
}
