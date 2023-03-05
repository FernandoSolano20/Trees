namespace bl.Structures.B;
public class BNode
{
    public int[] keys;
    public BNode[] children;
    public int numKeys;
    public bool isLeaf;

    public BNode(int order, bool isLeaf)
    {
        this.keys = new int[2*order-1];
        this.children = new BNode[2*order];
        this.numKeys = 0;
        this.isLeaf = isLeaf;
    }
}
