using bl.Structures.AVL;
using bl.Structures.RedBlack;

namespace bl;
public class Business
{
    private AvlTree _avlTree;
    private RedBlackTree _redBlackTree;

    public Business()
    {
        _avlTree = new AvlTree();
        _redBlackTree = new RedBlackTree();
    }

    public void AddInAvl(int value)
    {
        _avlTree.Add(value);
    }

    public string DrawAvl()
    {
        return _avlTree.Draw();
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
}
