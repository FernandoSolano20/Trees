using System.Collections.Generic;
using System.Text;

namespace bl.Structures.BPlus;
public class BPlusTree
{
    private readonly int order;
    private BPlusNode root;

    public BPlusTree(int order)
    {
        root = null;
        this.order = order;
    }

    public void Add(int x)
    {
        if (root == null)
        {
            root = new BPlusNode(order);
            root.Key[0] = x;
            root.IsLeaf = true;
            root.Size = 1;
        }

        else
        {
            var current = root;
            BPlusNode parent = null;

            while (current.IsLeaf == false)
            {
                parent = current;

                for (int i = 0; i < current.Size; i++)
                {
                    if (x < current.Key[i])
                    {
                        current = current.Children[i];
                        break;
                    }

                    if (i == current.Size - 1)
                    {
                        current = current.Children[i + 1];
                        break;
                    }
                }
            }

            // now we have reached leaf;
            if (current.Size < order)
            { // if the node to be inserted is
              // not filled
                int i = 0;

                // Traverse btree
                while (x > current.Key[i] && i < current.Size)
                    // goto pt where needs to be inserted.
                    i++;

                for (int j = current.Size; j > i; j--)
                    // adjust and insert element;
                    current.Key[j] = current.Key[j - 1];

                current.Key[i] = x;

                // size should be increased by 1
                current.Size++;

                current.Children[current.Size] = current.Children[current.Size - 1];
                current.Children[current.Size - 1] = null;
            }

            // if block does not have enough space;
            else
            {
                var newLeaf = new BPlusNode(order);
                var tempNode = new int[order + 1];

                for (int i = 0; i < order; i++)
                    // all elements of this block stored
                    tempNode[i] = current.Key[i];
                int z = 0;

                // find the right posn of num to be inserted
                while (x > tempNode[z] && z < order)
                    z++;

                //for (int j = order + 1; j > z; j--)
                //    tempNode[j] = tempNode[j - 1];
                tempNode[z] = x;
                // inserted element in its rightful position;

                newLeaf.IsLeaf = true;
                current.Size = (order + 1) / 2;
                newLeaf.Size = (order + 1) - (order + 1) / 2; // now rearrangement begins!

                current.Children[current.Size] = newLeaf;
                newLeaf.Children[newLeaf.Size] = current.Children[order];

                //current.Children[newLeaf.Size] = current.Children[order];
                current.Children[order] = null;

                for (int i = 0; i < current.Size; i++)
                    current.Key[i] = tempNode[i];

                for (int i = 0, j = current.Size; i < newLeaf.Size; i++, j++)
                    newLeaf.Key[i] = tempNode[j];

                // if this is root, then fine,
                // else we need to increase the height of tree;
                if (current == root)
                {
                    var newRoot = new BPlusNode(order);
                    newRoot.Key[0] = newLeaf.Key[0];
                    newRoot.Children[0] = current;
                    newRoot.Children[1] = newLeaf;
                    newRoot.IsLeaf = false;
                    newRoot.Size = 1;
                    root = newRoot;
                }
                else
                    ShiftLevel(newLeaf.Key[0],parent, newLeaf); // parent->original root
            }
        }
    }

    private void ShiftLevel(int x, BPlusNode current, BPlusNode child)
    { // insert or create an internal node;
        if (current.Size < order)
        { // if can fit in this level, do that
            int i = 0;
            while (x > current.Key[i] && i < current.Size)
                i++;
            for (int j = current.Size; j > i; j--)
                current.Key[j] = current.Key[j - 1];

            for (int j = current.Size + 1; j > i + 1; j--)
                current.Children[j] = current.Children[j - 1];

            current.Key[i] = x;
            current.Size++;
            current.Children[i + 1] = child;
        }

        // shift up
        else
        {
            var newInternal = new BPlusNode(order);
            var tempKey = new int[order + 1];
            var tempPtr = new BPlusNode[order + 2];

            for (int i = 0; i < order; i++)
                tempKey[i] = current.Key[i];

            for (int i = 0; i < order + 1; i++)
                tempPtr[i] = current.Children[i];

            int z = 0;
            while (x > tempKey[z] && z < order)
                z++;

            //for (int j = order + 1; j > z; j--)
            //    tempKey[j] = tempKey[j - 1];

            tempKey[z] = x;
            //for (int j = order + 2; j > z + 1; j--)
            //    tempPtr[j] = tempPtr[j - 1];

            tempPtr[z + 1] = child;
            newInternal.IsLeaf = false;
            current.Size = (order + 1) / 2;

            newInternal.Size = order - (order + 1) / 2;

            for (int i = 0, j = current.Size + 1; i < newInternal.Size; i++, j++)
                newInternal.Key[i] = tempKey[j];

            for (int i = 0, j = current.Size + 1;
                 i < newInternal.Size + 1; i++, j++)
                newInternal.Children[i] = tempPtr[j];

            if (current == root)
            {
                var newRoot = new BPlusNode(order);
                newRoot.Key[0] = current.Key[current.Size];
                newRoot.Children[0] = current;
                newRoot.Children[1] = newInternal;
                newRoot.IsLeaf = false;
                newRoot.Size = 1;
                root = newRoot;
            }

            else
                ShiftLevel(current.Key[current.Size],
                           FindParent(root, current),
                           newInternal);
        }
    }

    private BPlusNode FindParent(BPlusNode current, BPlusNode child)
    {
        BPlusNode parent = null;
        if (current.IsLeaf || (current.Children[0]).IsLeaf)
            return null;

        for (int i = 0; i < current.Size + 1; i++)
        {
            if (current.Children[i] == child)
            {
                parent = current;
                return parent;
            }
            else
            {
                parent = FindParent(current.Children[i], child);
                if (parent != null)
                    return parent;
            }
        }
        return parent;
    }

    public string Draw()
    {
        return Draw(root, new StringBuilder()).ToString();
    }

    private StringBuilder Draw(BPlusNode current, StringBuilder stringBuilder)
    {
        if (current == null)
            return stringBuilder;
        Queue<BPlusNode> q = new Queue<BPlusNode>();
        q.Enqueue(current);
        while (q.Count != 0)
        {
            int l;
            l = q.Count;

            for (int i = 0; i < l; i++)
            {
                BPlusNode tNode = q.Dequeue();

                for (int j = 0; j < tNode.Size; j++)
                    if (tNode != null)
                        stringBuilder.Append(tNode.Key[j] + " ");

                if (!tNode.IsLeaf)
                {
                    for (int j = 0; j < tNode.Size + 1; j++)
                        if (tNode.Children[j] != null)
                            q.Enqueue(tNode.Children[j]);
                }

                stringBuilder.Append("\t");
            }
            stringBuilder.Append("\n");
        }

        return stringBuilder;
    }

    public string Delete(int x)
    {
        if (root == null)
        {
            return "Arbol vacio\n";
        }
        else
        {
            var cursor = root;
            BPlusNode parent = null;
            int leftSibling = 0; 
            int rightSibling = 0;
            while (cursor.IsLeaf == false)
            {
                for (int i = 0; i < cursor.Size; i++)
                {
                    parent = cursor;
                    leftSibling = i - 1;
                    rightSibling = i + 1;
                    if (x < cursor.Key[i])
                    {
                        cursor = cursor.Children[i];
                        break;
                    }
                    if (i == cursor.Size - 1)
                    {
                        leftSibling = i;
                        rightSibling = i + 2;
                        cursor = cursor.Children[i + 1];
                        break;
                    }
                }
            }
            bool found = false;
            int pos;
            for (pos = 0; pos < cursor.Size; pos++)
            {
                if (cursor.Key[pos] == x)
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                return "No se encontro la llave\n";
            }

            //for (int i = pos; i < cursor.Size; i++)
            //{
            //    cursor.Key[i] = cursor.Key[i + 1];
            //}
            cursor.Size--;
            if (cursor == root)
            {
                for (int i = 0; i < order + 1; i++)
                {
                    cursor.Children[i] = null;
                }
                if (cursor.Size == 0)
                {
                    root = null;
                    return "Removido\n";
                }
            }
            cursor.Children[cursor.Size] = cursor.Children[cursor.Size + 1];
            cursor.Children[cursor.Size + 1] = null;
            if (cursor.Size >= (order + 1) / 2)
            {
                return "No removido";
            }
            if (leftSibling >= 0)
            {
                var leftNode = parent.Children[leftSibling];
                if (leftNode.Size >= (order + 1) / 2 + 1)
                {
                    for (int i = cursor.Size; i > 0; i--)
                    {
                        cursor.Key[i] = cursor.Key[i - 1];
                    }
                    cursor.Size++;
                    cursor.Children[cursor.Size] = cursor.Children[cursor.Size - 1];
                    cursor.Children[cursor.Size - 1] = null;
                    cursor.Key[0] = leftNode.Key[leftNode.Size - 1];
                    leftNode.Size--;
                    leftNode.Children[leftNode.Size] = cursor;
                    leftNode.Children[leftNode.Size + 1] = null;
                    parent.Key[leftSibling] = cursor.Key[0];
                    return "Removido";
                }
            }
            if (rightSibling <= parent.Size)
            {
                var rightNode = parent.Children[rightSibling];
                if (rightNode.Size >= (order + 1) / 2 + 1)
                {
                    cursor.Size++;
                    cursor.Children[cursor.Size] = cursor.Children[cursor.Size - 1];
                    cursor.Children[cursor.Size - 1] = null;
                    cursor.Key[cursor.Size - 1] = rightNode.Key[0];
                    rightNode.Size--;
                    rightNode.Children[rightNode.Size] = rightNode.Children[rightNode.Size + 1];
                    rightNode.Children[rightNode.Size + 1] = null;
                    for (int i = 0; i < rightNode.Size; i++)
                    {
                        rightNode.Key[i] = rightNode.Key[i + 1];
                    }
                    parent.Key[rightSibling - 1] = rightNode.Key[0];
                    return "Removido";
                }
            }
            if (leftSibling >= 0)
            {
                var leftNode = parent.Children[leftSibling];
                for (int i = leftNode.Size, j = 0; j < cursor.Size; i++, j++)
                {
                    leftNode.Key[i] = cursor.Key[j];
                }
                leftNode.Children[leftNode.Size] = null;
                leftNode.Size += cursor.Size;
                leftNode.Children[leftNode.Size] = cursor.Children[cursor.Size];
                RemoveInternal(parent.Key[leftSibling], parent, cursor);
            }
            else if (rightSibling <= parent.Size)
            {
                var rightNode = parent.Children[rightSibling];
                for (int i = cursor.Size, j = 0; j < rightNode.Size; i++, j++)
                {
                    cursor.Key[i] = rightNode.Key[j];
                }
                cursor.Children[cursor.Size] = null;
                cursor.Size += rightNode.Size;
                cursor.Children[cursor.Size] = rightNode.Children[rightNode.Size];
                RemoveInternal(parent.Key[rightSibling - 1], parent, rightNode);
            }
        }

        return "Removido";
    }

    private string RemoveInternal(int x, BPlusNode cursor, BPlusNode child)
    {
        if (cursor == root)
        {
            if (cursor.Size == 1)
            {
                if (cursor.Children[1] == child)
                {
                    root = cursor.Children[0];
                    return "Changed root node\n";
                }
                else if (cursor.Children[0] == child)
                {
                    root = cursor.Children[1];
                    return "Changed root node\n";
                }
            }
        }
        int pos;
        for (pos = 0; pos < cursor.Size; pos++)
        {
            if (cursor.Key[pos] == x)
            {
                break;
            }
        }
        for (int i = pos; i < cursor.Size; i++)
        {
            cursor.Key[i] = cursor.Key[i + 1];
        }
        for (pos = 0; pos < cursor.Size + 1; pos++)
        {
            if (cursor.Children[pos] == child)
            {
                break;
            }
        }
        for (int i = pos; i < cursor.Size + 1; i++)
        {
            cursor.Children[i] = cursor.Children[i + 1];
        }
        cursor.Size--;
        if (cursor.Size >= (order + 1) / 2 - 1)
        {
            return "";
        }
        if (cursor == root)
            return "";
        var parent = FindParent(root, cursor);
        int leftSibling = 0;
        int rightSibling = 0;
        for (pos = 0; pos < parent.Size + 1; pos++)
        {
            if (parent.Children[pos] == cursor)
            {
                leftSibling = pos - 1;
                rightSibling = pos + 1;
                break;
            }
        }
        if (leftSibling >= 0)
        {
            var leftNode = parent.Children[leftSibling];
            if (leftNode.Size >= (order + 1) / 2)
            {
                for (int i = cursor.Size; i > 0; i--)
                {
                    cursor.Key[i] = cursor.Key[i - 1];
                }
                cursor.Key[0] = parent.Key[leftSibling];
                parent.Key[leftSibling] = leftNode.Key[leftNode.Size - 1];
                for (int i = cursor.Size + 1; i > 0; i--)
                {
                    cursor.Children[i] = cursor.Children[i - 1];
                }
                cursor.Children[0] = leftNode.Children[leftNode.Size];
                cursor.Size++;
                leftNode.Size--;
                return "";
            }
        }
        if (rightSibling <= parent.Size)
        {
            var rightNode = parent.Children[rightSibling];
            if (rightNode.Size >= (order + 1) / 2)
            {
                cursor.Key[cursor.Size] = parent.Key[pos];
                parent.Key[pos] = rightNode.Key[0];
                for (int i = 0; i < rightNode.Size - 1; i++)
                {
                    rightNode.Key[i] = rightNode.Key[i + 1];
                }
                cursor.Children[cursor.Size + 1] = rightNode.Children[0];
                for (int i = 0; i < rightNode.Size; ++i)
                {
                    rightNode.Children[i] = rightNode.Children[i + 1];
                }
                cursor.Size++;
                rightNode.Size--;
                return "";
            }
        }
        if (leftSibling >= 0)
        {
            var leftNode = parent.Children[leftSibling];
            leftNode.Key[leftNode.Size] = parent.Key[leftSibling];
            for (int i = leftNode.Size + 1, j = 0; j < cursor.Size; j++)
            {
                leftNode.Key[i] = cursor.Key[j];
            }
            for (int i = leftNode.Size + 1, j = 0; j < cursor.Size + 1; j++)
            {
                leftNode.Children[i] = cursor.Children[j];
                cursor.Children[j] = null;
            }
            leftNode.Size += cursor.Size + 1;
            cursor.Size = 0;
            RemoveInternal(parent.Key[leftSibling], parent, cursor);
        }
        else if (rightSibling <= parent.Size)
        {
            var rightNode = parent.Children[rightSibling];
            cursor.Key[cursor.Size] = parent.Key[rightSibling - 1];
            for (int i = cursor.Size + 1, j = 0; j < rightNode.Size; j++)
            {
                cursor.Key[i] = rightNode.Key[j];
            }
            for (int i = cursor.Size + 1, j = 0; j < rightNode.Size + 1; j++)
            {
                cursor.Children[i] = rightNode.Children[j];
                rightNode.Children[j] = null;
            }
            cursor.Size += rightNode.Size + 1;
            rightNode.Size = 0;
            RemoveInternal(parent.Key[rightSibling - 1], parent, rightNode);
        }

        return "";
    }
}
