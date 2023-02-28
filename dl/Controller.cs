using bl;
using bl.Structures.AVLRB;

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
    public void AddInARBl(int value)
    {
        _bl.AddInTreeRB(value);
    }
    public void DeleteARBl(RedBlackNode value)
    {
        _bl.AddDeleteTreeRB(value);
    }
    public RedBlackNode SearchInARBl(int value)
    {
       return _bl.SearchInTreeRB(value);
    }
    public void RecorridoInAvl(RedBlackNode reco)
    {
        _bl.RecorridoInARB(reco);
    }

}
