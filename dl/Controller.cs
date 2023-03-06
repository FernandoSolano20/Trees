using bl;
using bl.Structures.RedBlack;

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
}
