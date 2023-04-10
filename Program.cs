///////////////////////////////////////////////////////////////////////////////
//
// Author: Andrew McDonald, mcdonaldai@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: 
// Description: An AVL tree is a binary search tree with the additional requirement that,
// for each node, the height of its left and right subtrees cannot differ by more than one.
// RBT:
// Each node must be colored either red or black. Thus, you need to add additional data for a node that stores a color.
// All nodes with values cannot be leaf nodes.
// For this reason, the NIL pseudo-nodes should be used as leaves in the tree, while all other nodes are internal ones.
// Moreover, all NIL pseudo-nodes must be black.
// If a node is red, both its children must be black.
// For any node, the number of black nodes on the route to a descendant leaf (that is, the NIL pseudo-node) must be the same.
//
///////////////////////////////////////////////////////////////////////////////
namespace DoubleLinked_BST_AVL_Tree_DataStructure;

/// <summary>
/// This program tests the tree class.
/// </summary>
internal class Program
{
    /// <summary>
    /// Main method adds 50 (or other preset number) random integers Nodes
    /// to a new instance of a tree and then shows the visualization of tree
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        Random r = new Random();
        List<int> randoms = new();


        for (int i = 0; i < 50; i++)
        {
            int ran = r.Next(0, 500);
            if (!(randoms.Contains(ran)))
            {
                randoms.Add(ran);
            }
        }



        Tree<int> t = new(randoms[0]);


        for (int i = 1; i < randoms.Count; i++)
        {
            t.Add(randoms[i]);

        }


        Console.WriteLine(t.VisualizeTree() + "\n");
    }
}