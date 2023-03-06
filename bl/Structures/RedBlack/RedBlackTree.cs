using System.Drawing;
using System.Text;
using System.Xml.Linq;

namespace bl.Structures.RedBlack;

public class RedBlackTree
{
    private RedBlackNode root;

    public RedBlackTree()
    {
        root = null;
    }

    public void Add(int value)
    {
        var node = new RedBlackNode(value);
        // Root init
        if (root == null)
        {
            root = node;
            node.Color = Color.Black;
            return;
        }

        var current = root;
        while (current != null)
        {
            // Insert in the left
            if (node.Value < current.Value)
            {
                if (current.Left == null)
                {
                    current.Left = node;
                    node.Parent = current;
                    break;
                }
                current = current.Left;
            }
            else
            {
                // Insert in the right
                if (current.Right == null)
                {
                    current.Right = node;
                    node.Parent = current;
                    break;
                }
                current = current.Right;
            }
        }

        FixViolation(node);
    }

    private void FixViolation(RedBlackNode node)
    {
        /// Find the root of sub tree
        while (node.Parent?.Color == Color.Red)
        {
            // If parent is the same to uncle of left
            // make nodes black and grandfather red
            if (node.Parent == node.Parent.Parent?.Left)
            {
                var uncle = node.Parent.Parent.Right;
                // If my uncle is a leaf
                if (uncle?.Color == Color.Red)
                {
                    // Make uncle and parent black
                    node.Parent.Color = Color.Black;
                    uncle.Color = Color.Black;
                    node.Parent.Parent.Color = Color.Red;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Right)
                    {
                        node = node.Parent;
                        RotateLeft(node);
                    }
                    node.Parent.Color = Color.Black;
                    node.Parent.Parent.Color = Color.Red;
                    RotateRight(node.Parent.Parent);
                }
            }
            else
            {
                var uncle = node.Parent.Parent.Left;
                // If uncle is red make parent and uncle black
                // make grandfather red
                if (uncle?.Color == Color.Red)
                {
                    node.Parent.Color = Color.Black;
                    uncle.Color = Color.Black;
                    node.Parent.Parent.Color = Color.Red;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Left)
                    {
                        // Make right-left rotate
                        node = node.Parent;
                        RotateRight(node);
                    }
                    // Make the new parent black
                    node.Parent.Color = Color.Black;
                    // Make the grandFather red, since it will be rotate
                    node.Parent.Parent.Color = Color.Red;
                    // Since balance to right is bad, rotate the grandFather
                    RotateLeft(node.Parent.Parent);
                }
            }
        }

        // Change the root to black
        root.Color = Color.Black;
    }

    private void RotateLeft(RedBlackNode node)
    {
        // New root
        var rightChild = node.Right;
        // Move to right the left part of the new root
        node.Right = rightChild.Left;
        if (rightChild.Left != null)
        {
            // New parent of the right node will be the node rotate it
            rightChild.Left.Parent = node;
        }
        // set the parent element to the new root
        rightChild.Parent = node.Parent;
        if (node.Parent == null)
        {
            // set the new root of the tree
            root = rightChild;
        }
        // if the old root is the uncle left
        else if (node == node.Parent.Left)
        {
            // assign the new root
            node.Parent.Left = rightChild;
        }
        else
        {
            // assign the new root
            node.Parent.Right = rightChild;
        }
        // Set to the left of new root
        rightChild.Left = node;
        // set the parent
        node.Parent = rightChild;
    }

    private void RotateRight(RedBlackNode node)
    {
        // new root
        var leftChild = node.Left;
        // Move to left the right part of the new root
        node.Left = leftChild.Right;
        if (leftChild.Right != null)
        {
            leftChild.Right.Parent = node;
        }
        // add new parent to new root
        leftChild.Parent = node.Parent;
        if (node.Parent == null)
        {
            root = leftChild;
        }
        // if the old root is the uncle right
        else if (node == node.Parent.Right)
        {
            // assign the new root
            node.Parent.Right = leftChild;
        }
        else
        {
            node.Parent.Left = leftChild;
        }
        // Set to the right of new root
        leftChild.Right = node;
        node.Parent = leftChild;
    }

    public RedBlackNode Search(int value)
    {
        var current = root;
        while (current != null)
        {
            if (value == current.Value)
            {
                return current;
            }
            else if (value < current.Value)
            {
                current = current.Left;
            }
            else
            {
                current = current.Right;
            }
        }
        return null;
    }

    public string Delete(int n)
    {
        if (root == null)
            // Tree is empty
            return "Arbol vacio";

        var v = Search(n);

        if (v?.Value != n)
        {
            return "No se encontro";
        }

        DeleteNode(v);

        return "Removido";
    }

    private void DeleteNode(RedBlackNode v)
    {
        var u = BSTreplace(v);

        // True when u and v are both black
        bool uvBlack = ((u == null || u.Color == Color.Black) && (v.Color == Color.Black));
        var parent = v.Parent;

        if (u == null)
        {
            // u is NULL therefore v is leaf
            if (v == root)
            {
                // v is root, making root null
                root = null;
            }
            else
            {
                if (uvBlack)
                {
                    // u and v both black
                    // v is leaf, fix double black at v
                    FixDoubleBlack(v);
                }
                else
                {
                    // u or v is red
                    if (v.Sibling() != null)
                        // sibling is not null, make it red"
                        v.Sibling().Color = Color.Red;
                }

                // delete v from the tree
                if (v.IsOnLeft())
                {
                    parent.Left = null;
                }
                else
                {
                    parent.Right = null;
                }
            }
            return;
        }

        if (v.Left == null || v.Right == null) {
            // v has 1 child
            if (v == root)
            {
                // v is root, assign the value of u to v, and delete u
                v.Value = u.Value;
                v.Left = v.Right = null;
            }
            else
            {
                // Detach v from tree and move u up
                if (v.IsOnLeft())
                {
                    parent.Left = u;
                }
                else
                {
                    parent.Right = u;
                }
                
                u.Parent = parent;
                if (uvBlack)
                {
                    // u and v both black, fix double black at u
                    FixDoubleBlack(u);
                }
                else
                {
                    // u or v red, color u black
                    u.Color = Color.Black;
                }
            }
            return;
        }

        // v has 2 children, swap values with successor and recurse
        SwapValues(u, v);
        DeleteNode(u);
    }

    private void FixDoubleBlack(RedBlackNode x)
    {
        if (x == root)
            // Reached root
            return;

        var sibling = x.Sibling();
        var parent = x.Parent;
        if (sibling == null)
        {
            // No sibiling, double black pushed up
            FixDoubleBlack(parent);
        }
        else
        {
            if (sibling.Color == Color.Red)
            {
                // Sibling red
                parent.Color = Color.Red;
                sibling.Color = Color.Black;
                if (sibling.IsOnLeft())
                {
                    // left case
                    RotateRight(parent);
                }
                else
                {
                    // right case
                    RotateLeft(parent);
                }
                FixDoubleBlack(x);
            }
            else
            {
                // Sibling black
                if (sibling.HasRedChild())
                {
                    // at least 1 red children
                    if (sibling.Left != null && sibling.Left.Color == Color.Red) {
                        if (sibling.IsOnLeft())
                        {
                            // left left
                            sibling.Left.Color = sibling.Color;
                            sibling.Color = parent.Color;
                            RotateRight(parent);
                        }
                        else
                        {
                            // right left
                            sibling.Left.Color = parent.Color;
                            RotateRight(sibling);
                            RotateLeft(parent);
                        }
                    } else
                    {
                        if (sibling.IsOnLeft())
                        {
                            // left right
                            sibling.Right.Color = parent.Color;
                            RotateLeft(sibling);
                            RotateRight(parent);
                        }
                        else
                        {
                            // right right
                            sibling.Right.Color = sibling.Color;
                            sibling.Color = parent.Color;
                            RotateLeft(parent);
                        }
                    }
                    parent.Color = Color.Black;
                }
                else
                {
                    // 2 black children
                    sibling.Color = Color.Red;
                    if (parent.Color == Color.Black)
                        FixDoubleBlack(parent);
                    else
                        parent.Color = Color.Black;
                }
            }
        }
    }

    private RedBlackNode Successor(RedBlackNode x)
    {
        var temp = x;

        while (temp.Left != null)
            temp = temp.Left;

        return temp;
    }

    private RedBlackNode BSTreplace(RedBlackNode x)
    {
        // when node have 2 children
        if (x.Left != null && x.Right != null) 
            return Successor(x.Right);

        // when leaf
        if (x.Left == null && x.Right == null) 
            return null;

        // when single child
        if (x.Left != null)
            return x.Left;
        else
            return x.Right;
    }

    private void SwapValues(RedBlackNode u, RedBlackNode v)
    {
        int temp;
        temp = u.Value;
        u.Value = v.Value;
        v.Value = temp;
    }

    public string Draw()
    {
        return PrintTreeHelper(root, 0, new StringBuilder()).ToString();
    }

    private StringBuilder PrintTreeHelper(RedBlackNode root, int space, StringBuilder stringBuilder)
    {
        int i;
        if (root != null)
        {
            space = space + 10;
            PrintTreeHelper(root.Right, space, stringBuilder);
            stringBuilder.Append("\n");
            for (i = 10; i < space; i++)
            {
                stringBuilder.Append(" ");
            }
            stringBuilder.Append(root.Value);
            stringBuilder.Append("\n");
            PrintTreeHelper(root.Left, space, stringBuilder);
        }
        return stringBuilder;
    }
}