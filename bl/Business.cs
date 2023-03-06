using bl.Structures.AVL;
using bl.Structures.B;
using bl.Structures.BPlus;
using bl.Structures.RedBlack;

namespace bl;
public class Business
{
    private AvlTree _avlTree;
    private BTree _bTree;
    private RedBlackTree _redBlackTree;
    private BPlusTree _bPlusTree;

    public Business()
    {
        _avlTree = new AvlTree();
        _bTree = new BTree(3);
        _redBlackTree = new RedBlackTree();
        _bPlusTree = new BPlusTree(2);
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
    
    public void AddInRedBlack(int value)
    {
        _redBlackTree.Add(value);
    }
    public void DeleteInRedBlack(int value)
    {
        _redBlackTree.Delete(value);
    }

    public string DrawRedBlack()
    {
        return _redBlackTree.Draw();
    }

    public void AddInBPlusTree(int value)
    {
        _bPlusTree.Add(value);
    }

    public string DrawBPlusTree()
    {
        return _bPlusTree.Draw();
    }

    public string DeleteInBPlusTree(int value)
    {
        return _bPlusTree.Delete(value);
    }
}
