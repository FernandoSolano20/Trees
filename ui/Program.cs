using dl;

var controller = new Controller();

int option;

do
{
    Console.WriteLine("Escoga una opcion:");
    Console.WriteLine("1- Agregar elemento en arbol AVL");
    Console.WriteLine("2- Borrar elemento en arbol AVL");
    Console.WriteLine("3- Agregar elemento en arbol B");
    Console.WriteLine("4- Borrar elemento en arbol B");
    Console.WriteLine("5- Agregar elemento en arbol B+");
    Console.WriteLine("6- Borrar elemento en arbol B+");
    Console.WriteLine("7- Agregar elemento en arbol Rojo-Negro");
    Console.WriteLine("8- Eliminar elemento en arbol Rojo-Negro");
    Console.WriteLine("9- Salir");
    option = Convert.ToInt32(Console.ReadLine());

    switch (option)
    {
        case 1:
            controller.AddInAvl(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Dibujando el arbol");
            Console.WriteLine("\n\n\n");
            Console.WriteLine(controller.DrawAvl());
            Console.WriteLine("\n\n\n");
            break;

        case 2:
            controller.DeleteInAvl(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Dibujando el arbol");
            Console.WriteLine("\n\n\n");
            Console.WriteLine(controller.DrawAvl());
            Console.WriteLine("\n\n\n");
            break;

        case 3:
            controller.AddInBTree(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Mostrando el arbol");
            Console.WriteLine(controller.ShowBTree());
            break;

        case 4:
            controller.DeleteInBTree(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Mostrando el arbol");
            Console.WriteLine(controller.ShowBTree());
            break;

        case 5:
            controller.AddInBPlusTree(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Mostrando el arbol");
            Console.WriteLine(controller.DrawBPlusTree());
            break;

        case 6:
            controller.DeleteInBPlusTree(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Mostrando el arbol");
            Console.WriteLine(controller.ShowBTree());
            break;

        case 7:
            Console.WriteLine("Ingrese el nodo para agregar");
            controller.AddInRedBlack(ReadNumber());
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Dibujando el arbol");
            Console.WriteLine("\n\n\n");
            Console.WriteLine(controller.DrawRedBlack());
            Console.WriteLine("\n\n\n");
            break;

        case 8:
            Console.WriteLine("Ingrese el nodo para eliminar");
            controller.DeleteInRedBlack(ReadNumber());
            Console.WriteLine("Dibujando el arbol");
            Console.WriteLine("\n\n\n");
            Console.WriteLine(controller.DrawRedBlack());
            Console.WriteLine("\n\n\n");
            break;

        default:
            Console.WriteLine("Opcion no valida");
            break;
    }
} while (option != 9);

static int ReadNumber()
{
    Console.WriteLine("Digite el numero");
    var number = Convert.ToInt32(Console.ReadLine());
    return number;
}

public class BPlusTree<TKey, TValue>
{
    private readonly int _degree;
    private readonly int _minDegree;
    private readonly int _maxDegree;
    private readonly IComparer<TKey> _comparer;

    private BPlusTreeNode<TKey, TValue> _root;

    public BPlusTree(int degree, IComparer<TKey> comparer = null)
    {
        if (degree < 2)
        {
            throw new ArgumentException("Degree must be at least 2.", nameof(degree));
        }

        _degree = degree;
        _minDegree = _degree / 2;
        _maxDegree = _degree - 1;
        _comparer = comparer ?? Comparer<TKey>.Default;

        _root = new BPlusTreeNode<TKey, TValue>(_degree);
    }

    public void Insert(TKey key, TValue value)
    {
        BPlusTreeNode<TKey, TValue> node = _root;

        if (node.IsFull)
        {
            BPlusTreeNode<TKey, TValue> newRoot = new BPlusTreeNode<TKey, TValue>(_degree);
            newRoot.Children.Add(node);

            _root = newRoot;

            node = SplitNode(node);
        }

        while (!node.IsLeaf)
        {
            int index = node.FindIndex(key, _comparer);

            if (index == node.Keys.Count || _comparer.Compare(key, node.Keys[index]) != 0)
            {
                if (node.Children[index].IsFull)
                {
                    node.Children.Insert(index + 1, SplitNode(node.Children[index]));
                    node.Keys.Insert(index, node.Children[index + 1].Keys[0]);
                }

                node = node.Children[index];
            }
            else
            {
                throw new ArgumentException("Duplicate keys not allowed.");
            }
        }

        int leafIndex = node.FindIndex(key, _comparer);

        if (leafIndex == node.Keys.Count)
        {
            node.Keys.Add(key);
            node.Values.Add(value);
        }
        else if (_comparer.Compare(key, node.Keys[leafIndex]) != 0)
        {
            node.Keys.Insert(leafIndex, key);
            node.Values.Insert(leafIndex, value);
        }
        else
        {
            throw new ArgumentException("Duplicate keys not allowed.");
        }
    }

    public bool Remove(TKey key)
    {
        bool success = false;

        BPlusTreeNode<TKey, TValue> node = _root;

        while (!node.IsLeaf)
        {
            int index = node.FindIndex(key, _comparer);

            if (index == node.Keys.Count || _comparer.Compare(key, node.Keys[index]) != 0)
            {
                node = node.Children[index];
            }
            else
            {
                node = node.Children[index + 1];
            }
        }

        int leafIndex = node.FindIndex(key, _comparer);

        if (leafIndex < node.Keys.Count && _comparer.Compare(key, node.Keys[leafIndex]) == 0)
        {
            node.Keys.RemoveAt(leafIndex);
            node.Values.RemoveAt(leafIndex);

            success = true;
        }

        if (_root.Children.Count == 1 && !_root.IsLeaf)
        {
            _root = _root.Children[0];
        }

        return success;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        BPlusTreeNode<TKey, TValue> node = _root;

        while (!node.IsLeaf)
        {
            int index = node.FindIndex(key, _comparer);

            if (index == node.Keys.Count || _comparer.Compare(key, node.Keys[index]) != 0)
            {
                node = node.Children[index];
            }
            else
            {
                node = node.Children[index + 1];
            }
        }

        int leafIndex = node.FindIndex(key, _comparer);

        if (leafIndex < node.Keys.Count && _comparer.Compare(key, node.Keys[leafIndex]) == 0)
        {
            value = node.Values[leafIndex];
            return true;
        }
        else
        {
            value = default(TValue);
            return false;
        }
    }

    private BPlusTreeNode<TKey, TValue> SplitNode(BPlusTreeNode<TKey, TValue> node)
    {
        BPlusTreeNode<TKey, TValue> newNode = new BPlusTreeNode<TKey, TValue>(_degree);

        int midIndex = node.Keys.Count / 2;

        TKey midKey = node.Keys[midIndex];

        for (int i = midIndex + 1; i < node.Keys.Count; i++)
        {
            newNode.Keys.Add(node.Keys[i]);
            newNode.Values.Add(node.Values[i]);
        }

        int count = Math.Min(midIndex + 1, node.Children.Count - (midIndex + 1));
        if (count > 0)
        {
            newNode.Children.AddRange(node.Children.GetRange(midIndex + 1, count));
        }

        node.Keys.RemoveRange(midIndex, node.Keys.Count - midIndex);
        node.Values.RemoveRange(midIndex, node.Values.Count - midIndex);

        int removeCount = Math.Max(0, node.Children.Count - (midIndex + 1));
        node.Children.RemoveRange(midIndex + 1, removeCount);

        if (node == _root)
        {
            BPlusTreeNode<TKey, TValue> newRoot = new BPlusTreeNode<TKey, TValue>(_degree);
            newRoot.Children.Add(node);
            newRoot.Children.Add(newNode);
            newRoot.Keys.Add(midKey);

            _root = newRoot;
        }
        else
        {
            BPlusTreeNode<TKey, TValue> parent = node.Parent;

            int index = parent.Children.IndexOf(node);

            parent.Keys.Insert(index, midKey);
            parent.Children.Insert(index + 1, newNode);

            if (parent.IsFull)
            {
                return SplitNode(parent);
            }
        }

        return node;
    }
}

public class BPlusTreeNode<TKey, TValue>
{
    public BPlusTreeNode(int degree)
    {
        Keys = new List<TKey>(degree - 1);
        Values = new List<TValue>(degree - 1);
        Children = new List<BPlusTreeNode<TKey, TValue>>(degree + 1);
    }

    public List<TKey> Keys { get; set; }
    public List<TValue> Values { get; set; }
    public List<BPlusTreeNode<TKey, TValue>> Children { get; set; }
    public BPlusTreeNode<TKey, TValue> Parent { get; set; }

    public bool IsLeaf => Children.Count == 0;
    public bool IsFull => Keys.Count == Children.Capacity - 1;

    public int FindIndex(TKey key, IComparer<TKey> comparer)
    {
        int index = 0;

        while (index < Keys.Count && comparer.Compare(Keys[index], key) < 0)
        {
            index++;
        }

        return index;
    }
}
