using System.Text;
using System.Xml.Linq;

namespace bl.Structures.B;
public class BTree
{
    private int order;
    private BNode root;

    public BTree(int order)
    {
        this.order = order;
        this.root = new BNode(order, true);
    }

    public void Add(int key)
    {
        var node = root;
        if (node.numKeys == 2 * order - 1)
        {
            var newRoot = new BNode(order, false);
            root = newRoot;
            newRoot.children[0] = node;
            SplitChild(newRoot, 0, node);
            InsertNonFull(newRoot, key);
        }
        else
        {
            InsertNonFull(node, key);
        }
    }

    private void SplitChild(BNode parent, int index, BNode node)
    {
        var newNode = new BNode(order, node.isLeaf);
        newNode.numKeys = order - 1;
        for (int i = 0; i < order - 1; i++)
        {
            newNode.keys[i] = node.keys[i + order];
        }
        if (!node.isLeaf)
        {
            for (int i = 0; i < order; i++)
            {
                newNode.children[i] = node.children[i + order];
            }

            // Reset the node
            for (int i = 0; i < order; i++)
            {
                node.children[i + order] = null;
            }
        }
        node.numKeys = order - 1;
        for (int i = parent.numKeys; i > index+1; i--)
        {
            parent.children[i + 1] = parent.children[i];
        }
        parent.children[index + 1] = newNode;
        for (int i = parent.numKeys - 1; i >= index; i--)
        {
            parent.keys[i + 1] = parent.keys[i];
        }
        parent.keys[index] = node.keys[order - 1];

        // Reset the node
        for (int i = -1; i < order - 1; i++)
        {
            node.keys[i + order] = 0;
        }
        parent.numKeys++;
    }

    private void InsertNonFull(BNode node, int key)
    {
        int i = node.numKeys - 1;
        if (node.isLeaf)
        {
            while (i >= 0 && key < node.keys[i])
            {
                node.keys[i + 1] = node.keys[i];
                i--;
            }
            node.keys[i + 1] = key;
            node.numKeys++;
        }
        else
        {
            while (i >= 0 && key < node.keys[i])
            {
                i--;
            }
            i++;
            if (node.children[i].numKeys == 2 * order - 1)
            {
                SplitChild(node, i, node.children[i]);
                if (key > node.keys[i])
                {
                    i++;
                }
            }
            InsertNonFull(node.children[i], key);
        }
    }

    public bool Search(int key)
    {
        return Search(root, key);
    }

    private bool Search(BNode node, int key)
    {
        int i = 0;
        while (i < node.numKeys && key > node.keys[i])
        {
            i++;
        }
        if (i < node.numKeys && key == node.keys[i])
        {
            return true;
        }
        if (node.isLeaf)
        {
            return false;
        }
        return Search(node.children[i], key);
    }

    public string Delete(int key)
    {
        return Remove(key, root);
    }

    private int FindKey(int k, BNode node)
    {
        int idx = 0;
        while (idx < node.numKeys && node.keys[idx] < k)
            ++idx;
        return idx;
    }

    private string Remove(int k, BNode node)
    {
        int idx = FindKey(k, node);

        // The key to be removed is present in this node
        if (idx < node.numKeys && node.keys[idx] == k)
        {

            // If the node is a leaf node - removeFromLeaf is called
            // Otherwise, removeFromNonLeaf function is called
            if (node.isLeaf)
                RemoveFromLeaf(idx, node);
            else
                RemoveFromNonLeaf(idx, node);
        }
        else
        {

            // If this node is a leaf node, then the key is not present in tree
            if (node.isLeaf)
            {
                return "La llave " + k + " no existe en el arbol\n";
            }

            // The key to be removed is present in the sub-tree rooted with this node
            // The flag indicates whether the key is present in the sub-tree rooted
            // with the last child of this node
            bool flag = ((idx == node.numKeys) ? true : false);

            // If the child where the key is supposed to exist has less that t keys,
            // we fill that child
            if (node.children[idx].numKeys < order)
                fill(idx, node);

            // If the last child has been merged, it must have merged with the previous
            // child and so we recurse on the (idx-1)th child. Else, we recurse on the
            // (idx)th child which now has atleast t keys
            if (flag && idx > node.numKeys)
                return Remove(k, node.children[idx - 1]);
            else
                return Remove(k, node.children[idx]);
        }
        return "Removido";
    }

    private void RemoveFromLeaf(int idx, BNode node)
    {

        // Move all the keys after the idx-th pos one place backward
        for (int i = idx + 1; i < node.numKeys; ++i)
            node.keys[i - 1] = node.keys[i];

        // Reduce the count of keys
        node.numKeys--;

        return;
    }

    private void RemoveFromNonLeaf(int idx, BNode node)
    {

        int k = node.keys[idx];

        // If the child that precedes k (C[idx]) has atleast t keys,
        // find the predecessor 'pred' of k in the subtree rooted at
        // C[idx]. Replace k by pred. Recursively delete pred
        // in C[idx]
        if (node.children[idx].numKeys >= order)
        {
            int pred = getPred(idx, node);
            node.keys[idx] = pred;
            Remove(pred, node.children[idx]);
        }

        // If the child C[idx] has less that t keys, examine C[idx+1].
        // If C[idx+1] has atleast t keys, find the successor 'succ' of k in
        // the subtree rooted at C[idx+1]
        // Replace k by succ
        // Recursively delete succ in C[idx+1]
        else if (node.children[idx + 1].numKeys >= order)
        {
            int succ = getSucc(idx, node);
            node.keys[idx] = succ;
            Remove(succ, node.children[idx + 1]);
        }

        // If both C[idx] and C[idx+1] has less that t keys,merge k and all of C[idx+1]
        // into C[idx]
        // Now C[idx] contains 2t-1 keys
        // Free C[idx+1] and recursively delete k from C[idx]
        else
        {
            merge(idx, node);
            Remove(k, node.children[idx]);
        }
        return;
    }

    private int getPred(int idx, BNode node)
    {
        // Keep moving to the right most node until we reach a leaf
        var cur = node.children[idx];
        while (cur.isLeaf)
            cur = cur.children[cur.numKeys];

        // Return the last key of the leaf
        return cur.keys[cur.numKeys - 1];
    }

    private int getSucc(int idx, BNode node)
    {

        // Keep moving the left most node starting from C[idx+1] until we reach a leaf
        var cur = node.children[idx + 1];
        while (cur.isLeaf)
            cur = cur.children[0];

        // Return the first key of the leaf
        return cur.keys[0];
    }

    private void fill(int idx, BNode node)
    {

        // If the previous child(C[idx-1]) has more than t-1 keys, borrow a key
        // from that child
        if (idx != 0 && node.children[idx - 1].numKeys >= order)
            borrowFromPrev(idx, node);

        // If the next child(C[idx+1]) has more than t-1 keys, borrow a key
        // from that child
        else if (idx != node.numKeys && node.children[idx + 1].numKeys >= order)
            borrowFromNext(idx, node);

        // Merge C[idx] with its sibling
        // If C[idx] is the last child, merge it with its previous sibling
        // Otherwise merge it with its next sibling
        else
        {
            if (idx != node.numKeys)
                merge(idx, node);
            else
                merge(idx - 1, node);
        }
        return;
    }

    private void borrowFromPrev(int idx, BNode node)
    {

        var child = node.children[idx];
        var sibling = node.children[idx - 1];

        // The last key from C[idx-1] goes up to the parent and key[idx-1]
        // from parent is inserted as the first key in C[idx]. Thus, the  loses
        // sibling one key and child gains one key

        // Moving all key in C[idx] one step ahead
        for (int i = child.numKeys - 1; i >= 0; --i)
            child.keys[i + 1] = child.keys[i];

        // If C[idx] is not a leaf, move all its child pointers one step ahead
        if (!child.isLeaf)
        {
            for (int i = child.numKeys; i >= 0; --i)
                child.children[i + 1] = child.children[i];
        }

        // Setting child's first key equal to keys[idx-1] from the current node
        child.keys[0] = node.keys[idx - 1];

        // Moving sibling's last child as C[idx]'s first child
        if (!child.isLeaf)
            child.children[0] = sibling.children[sibling.numKeys];

        // Moving the key from the sibling to the parent
        // This reduces the number of keys in the sibling
        node.keys[idx - 1] = sibling.keys[sibling.numKeys - 1];

        child.numKeys += 1;
        sibling.numKeys -= 1;

        return;
    }

    private void borrowFromNext(int idx, BNode node)
    {

        var child = node.children[idx];
        var sibling = node.children[idx + 1];

        // keys[idx] is inserted as the last key in C[idx]
        child.keys[(child.numKeys)] = node.keys[idx];

        // Sibling's first child is inserted as the last child
        // into C[idx]
        if (!(child.isLeaf))
            child.children[(child.numKeys) + 1] = sibling.children[0];

        //The first key from sibling is inserted into keys[idx]
        node.keys[idx] = sibling.keys[0];

        // Moving all keys in sibling one step behind
        for (int i = 1; i < sibling.numKeys; ++i)
            sibling.keys[i - 1] = sibling.keys[i];

        // Moving the child pointers one step behind
        if (!sibling.isLeaf)
        {
            for (int i = 1; i <= sibling.numKeys; ++i)
                sibling.children[i - 1] = sibling.children[i];
        }

        // Increasing and decreasing the key count of C[idx] and C[idx+1]
        // respectively
        child.numKeys += 1;
        sibling.numKeys -= 1;

        return;
    }

    private void merge(int idx, BNode node)
    {
        var child = node.children[idx];
        var sibling = node.children[idx + 1];

        // Pulling a key from the current node and inserting it into (t-1)th
        // position of C[idx]
        child.keys[order - 1] = node.keys[idx];

        // Copying the keys from C[idx+1] to C[idx] at the end
        for (int i = 0; i < sibling.numKeys; ++i)
            child.keys[i + order] = sibling.keys[i];

        // Copying the child pointers from C[idx+1] to C[idx]
        if (!child.isLeaf)
        {
            for (int i = 0; i <= sibling.numKeys; ++i)
                child.children[i + order] = sibling.children[i];
        }

        // Moving all keys after idx in the current node one step before -
        // to fill the gap created by moving keys[idx] to C[idx]
        for (int i = idx + 1; i < node.numKeys; ++i)
            node.keys[i - 1] = node.keys[i];

        // Moving the child pointers after (idx+1) in the current node one
        // step before
        for (int i = idx + 2; i <= node.numKeys; ++i)
            node.children[i - 1] = node.children[i];

        // Updating the key count of child and the current node
        child.numKeys += sibling.numKeys + 1;
        node.numKeys--;

        return;
    }

    public string Traverse()
    {
        return Traverse(root, new StringBuilder()).ToString();
    }

    private StringBuilder Traverse(BNode node, StringBuilder stringBuilder)
    {
        // There are n keys and n+1 children, traverse through n keys
        // and first n children
        int i;
        for (i = 0; i < node.numKeys; i++)
        {
            // If this is not leaf, then before printing key[i],
            // traverse the subtree rooted with child C[i].
            if (node.isLeaf == false)
                Traverse(node.children[i], stringBuilder);
            stringBuilder.Append(" " + node.keys[i] + ",");
        }

        // Print the subtree rooted with last child
        if (node.isLeaf == false)
            Traverse(node.children[i], stringBuilder);

        return stringBuilder;
    }

    public string Draw()
    {
        return Draw(root, new StringBuilder()).ToString();
    }

    private StringBuilder Draw(BNode current, StringBuilder stringBuilder)
    {
        if (current == null)
            return stringBuilder;
        Queue<BNode> q = new Queue<BNode>();
        q.Enqueue(current);
        while (q.Count != 0)
        {
            int l;
            l = q.Count;

            for (int i = 0; i < l; i++)
            {
                var tNode = q.Dequeue();

                for (int j = 0; j < tNode.numKeys; j++)
                    if (tNode != null)
                        stringBuilder.Append(tNode.keys[j] + " ");

                if (!tNode.isLeaf)
                {
                    for (int j = 0; j < tNode.children.Length; j++)
                        if (tNode.children[j] != null)
                            q.Enqueue(tNode.children[j]);
                }

                stringBuilder.Append("\t");
            }
            stringBuilder.Append("\n");
        }

        return stringBuilder;
    }
}
