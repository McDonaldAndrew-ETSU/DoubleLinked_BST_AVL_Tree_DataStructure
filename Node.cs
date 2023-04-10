///////////////////////////////////////////////////////////////////////////////
//
// Author: Andrew McDonald, mcdonaldai@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: 
// Description:
// This class creates a node of any type for a tree. A node will have the
// possibility of a parent and the possibility to have children. In BinarySearch Trees
// a node cannot have more than two children.
//
///////////////////////////////////////////////////////////////////////////////
namespace DoubleLinked_BST_AVL_Tree_DataStructure;

/// <summary>
/// This generates a node with a certain Data type to
/// be used in a tree.
/// </summary>
/// <typeparam name="T"> any data type </typeparam>
internal class Node<T>
{
    public T? Data { get; set; }
    public Node<T>? Parent { get; set; }
    public Node<T>[] Children { get; set; } = new Node<T>[2] { null, null };
    public Node<T> LeftChild { get => Children[0]; set { Children[0] = value; } }
    public Node<T> RightChild { get => Children[1]; set { Children[1] = value; } }

    /// <summary>
    /// ctor for Node to input the type of data
    /// </summary>
    /// <param name="data"> the data for the node </param>
    public Node(T? data) { Data = data; }
    /// <summary>
    /// Parameterless ctor for Node
    /// </summary>
    public Node() { }


    /// <summary>
    /// This returns the height of the node by incrementing the height once every time the node has a parent.
    /// This happens until the node reaches the Root.
    /// </summary>
    /// <returns> an int representing the height from the node to the root </returns>
    public int GetNodeHeight()
    {
        int height = 0;
        if (this == null)
        {
            return height;
        }
        else
        {
            height = 1;
            Node<T> current = this;
            while (current.Parent != null)
            {
                height++;
                current = current.Parent;
            }
        }

        return height;


    }

}
