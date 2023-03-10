using System.Text;
using bl.Structures.AVL;
using bl.Structures.B;
using bl.Structures.BPLusTree.BPTree;
using bl.Structures.RedBlack;

namespace bl;
public class Business
{
    private AvlTree _avlTree;
    private BTree _bTree;
    private RedBlackTree _redBlackTree;
    private BTreeDictionary<int, int> _bPlusTree;


    public Business()
    {
        _avlTree = new AvlTree();
        _bTree = new BTree(3);
        _redBlackTree = new RedBlackTree();
        _bPlusTree = new BTreeDictionary<int, int>();
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

    public string DrawBTree()
    {
        return _bTree.Draw();
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

    public void AddInBPTree(int item)
    {
        _bPlusTree.Add(item, item);
    }

    public string DrawBPTree()
    {
        var stringBuilder = new StringBuilder();
        foreach (var kvp in _bPlusTree)
        {
            stringBuilder.Append($"Key = {kvp.Key}, Value = {kvp.Value}\n");
        }

        return stringBuilder.ToString();
    }

    public void RemoveInBPTree(int item)
    {
        _bPlusTree.Remove(item);
    }
}
