namespace bl.Structures.BPlus;
public class BPlusNode
{
    public bool IsLeaf;
    public BPlusNode[] Children;
    public int[] Key;
    public int Size;

    public BPlusNode(int bucketSize)
    {
        Key = new int[bucketSize];
        Children = new BPlusNode[bucketSize + 1];
    }
}
