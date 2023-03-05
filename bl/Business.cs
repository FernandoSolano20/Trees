using bl.Structures.AVL;
using bl.Structures.B;

namespace bl;
public class Business
{
    private AvlTree _avlTree;
    private BTree _bTree;

    public Business()
    {
        _avlTree = new AvlTree();
        _bTree = new BTree(3);
    }

    public void AddInAvl(int value)
    {
        _avlTree.Add(value);
    }

    public void DeleteInAvl(int value)
    {
        _avlTree.Delete(value);
    }

    public string DrawAvl()
    {
        return _avlTree.Draw();
    }


    public void AddInBTree(int value)
    {
        _bTree.Add(value);
    }

    public string DeleteInBTree(int value)
    {
        return _bTree.Delete(value);
    }

    public bool SearchInBTree(int value)
    {
        return _bTree.Search(value);
    }

    public string ShowBTree()
    {
        return _bTree.Traverse();
    }
}
