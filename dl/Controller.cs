using bl;
using bl.Structures.AVL;
using bl.Structures.RedBlack;
using bl.Structures.B;
using bl.Structures.BPLusTree.BPTree;

namespace dl;
public class Controller
{
    private Business _bl;
    public Controller()
    {
        _bl = new Business();
    }

    public void AddInAvl(int value)
    {
        _bl.AddInAvl(value);
    }

    public string DrawAvl()
    {
        return _bl.DrawAvl();
    }

    public void DeleteInAvl(int value)
    {
        _bl.DeleteInAvl(value);
    }


    public void AddInBTree(int value)
    {
        _bl.AddInBTree(value);
    }

    public string DeleteInBTree(int value)
    {
        return _bl.DeleteInBTree(value);
    }

    public bool SearchInBTree(int value)
    {
        return _bl.SearchInBTree(value);
    }

    public string ShowBTree()
    {
        return _bl.ShowBTree();
    }
    
    public void AddInRedBlack(int value)
    {
        _bl.AddInRedBlack(value);
    }
    public void DeleteInRedBlack(int value)
    {
        _bl.DeleteInRedBlack(value);
    }

    public string DrawRedBlack()
    {
        return _bl.DrawRedBlack();
    }

    public void DrawBPTree(BTreeDictionary<int, int> tree)
    {
        _bl.DrawBPTree(tree);
    }

    public BTreeDictionary<int, int> AddInBPTree(BTreeDictionary<int, int> tree, int item)
    {
        tree = _bl.AddInBPTree(tree, item);
        return tree;
    }
}
