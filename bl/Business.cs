using bl.Structures.AVL;

namespace bl;
public class Business
{
    private AvlTree _avlTree;

    public Business()
    {
        _avlTree = new AvlTree();
    }

    public void AddInAvl(int value)
    {
        _avlTree.Add(value);
    }

    public string DrawAvl()
    {
        return _avlTree.Draw();
    }
}
