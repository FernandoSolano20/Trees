using bl.Structures.AVL;
using bl.Structures.AVLRB;

namespace bl;
public class Business
{
    private AvlTree _avlTree;
    private Tree Tree_tree;

    public Business()
    {
        _avlTree = new AvlTree();
        Tree_tree = new Tree();
    }

    public void AddInAvl(int value)
    {
        _avlTree.Add(value);
    }

    public string DrawAvl()
    {
        return _avlTree.Draw();
    }

    public void AddInTreeRB(int value)
    {
        Tree_tree.Insertar(value);
    }
    public void AddDeleteTreeRB(RedBlackNode delete)
    {
        Tree_tree.Eliminar(delete);
    }
    public RedBlackNode SearchInTreeRB(int value)
    {
        return Tree_tree.buscar(value);
    }
    public void RecorridoInARB(RedBlackNode reco)
    {
        Tree_tree.Recorrido(reco);
    }
}
