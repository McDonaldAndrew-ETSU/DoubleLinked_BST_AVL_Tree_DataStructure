///////////////////////////////////////////////////////////////////////////////
//
// Author: Andrew McDonald, mcdonaldai@etsu.edu
// Course: CSCI-2210-001 - Data Structures
// Assignment: 
// Description: Builds a Doubly Linked AVL Binary Search Tree. This Tree has nodes that 
// contains only nodes with two children and also self balances. The Visualize method
// represents a tree data structure much like how a UNIX/LINUX bash "tree" command
// represents different directories
//
///////////////////////////////////////////////////////////////////////////////
namespace DoubleLinked_BST_AVL_Tree_DataStructure;


/// <summary>
/// The tree class is generic to allow different data types
/// </summary>
/// <typeparam name="T"> any data type </typeparam>
internal class Tree<T> where T : IComparable
{
    public int Count { get; set; }
    public Node<T> Root { get; set; }

    /// <summary>
    /// The ctor for Tree with using a T type data to create a new Node and setting it as the Root
    /// </summary>
    /// <param name="data"> the type of data the tree will be using </param>
    public Tree(T data)
    {
        Node<T> rootNode = new() { Data = data };
        Root = rootNode;
        Count++;
    }

    /// <summary>
    /// Here is where we will insert a value that a Node's data may be equal to using
    /// the CompareTo method. < 0 means we need to check the left of the tree, since that means
    /// the value is less than the current node. == to 0 means that we found it and the tree does contain the data.
    /// > 0 means we need to check the right side of the tree since that means the value is greater than the current node.
    /// If we reach the end of the tree to a NIL or null node, we will return false, as that means the node does not exist.
    /// </summary>
    /// <param name="data"> the data to search for </param>
    /// <returns> a true or false if the tree contains the specific node </returns>
    public bool Contains(T data)
    {
        Node<T> currentNode = Root;
        while (currentNode != null)
        {
            int result = data.CompareTo(currentNode.Data);
            if (result == 0)
            {
                //if (currentNode.Parent != null) Console.Write($"\nParent:{currentNode.Parent.Data}\t");
                //if (currentNode.RightChild != null) Console.Write($"Right:{currentNode.RightChild.Data}\t");
                //if (currentNode.LeftChild != null) Console.Write($"Left:{currentNode.LeftChild.Data}\n");
                return true;
            }
            else if (result < 0)
            {
                currentNode = currentNode.LeftChild;
                
            }
            else currentNode = currentNode.RightChild;
        }
        return false;
    }

    /// <summary>
    /// Adds a new node as long as it doesn't exist in the tree.
    /// First we find a valid parent node without chidren based on the data's sort order. 
    /// The data will be either the parent's Left or Right depending if it is smaller or larger than the parent.
    /// </summary>
    /// <param name="data"> The data that the new node will carry </param>
    public void Add(T data)
    {
        Node<T> parent = GetParentForNewNode(data);
        Node<T> node = new() { Data = data, Parent = parent };
        if (parent == null) Root = node;
        else if (data.CompareTo(parent.Data) < 0) parent.LeftChild = node;
        else parent.RightChild = node;

        Root = BalanceTree(Root);
        Count++;
    }
    /// <summary>
    /// Checks to see if the data exists, if so, throw an exception, if not, check to see if it belongs on the left or right of each 
    /// current node until the current node has no children (null). At that point the current node will be returned as the parent node
    /// so that the data can be added as a child.
    /// </summary>
    /// <param name="data"> the data we are trying to add to a new node for the tree </param>
    /// <returns> a parent node for the new node we are adding for </returns>
    private Node<T> GetParentForNewNode(T data)
    {
        Node<T> currentNode = Root;
        Node<T> parent = null;
        while (currentNode != null)
        {
            parent = currentNode;
            int result = data.CompareTo(currentNode.Data);
            if (result == 0) throw new ArgumentException($"The node with {data} already exists. Will not add {data} to tree");    
            else if (result < 0) currentNode = currentNode.LeftChild;
            else currentNode = currentNode.RightChild;
        }
        return parent;
    }
    /// <summary>
    /// This method Balances the tree by using the balance factor method which
    /// finds the height of both Root subtrees. If more than one on either subtree,
    /// balancing will commence.
    /// </summary>
    /// <param name="node"> current node </param>
    /// <returns> the balanced node with balanced references </returns>
    public Node<T> BalanceTree(Node<T> node)
    {
        int balanceFactor = BalanceFactor(node);
        if (balanceFactor > 1)
        {
            if (BalanceFactor(node.LeftChild) > 0) {
                //Console.WriteLine($"LL called -> balance factor: {balanceFactor}");
                node = RotateLL(node);
            }
            else
            {
                //Console.WriteLine($"LR called -> balance factor: {balanceFactor}");
                node = RotateLR(node);
            }

        }
        else if (balanceFactor < -1)
        {
            if (BalanceFactor(node.RightChild) > 0)
            {
                //Console.WriteLine($"RL called -> balance factor: {balanceFactor}");
                node = RotateRL(node);
            }
            else
            {
                //Console.WriteLine($"RR called -> balance factor: {balanceFactor}");
                node = RotateRR(node);
            }

        }
        return node;
    }
    /// <summary>
    /// Finds the difference between the left and right subtrees.
    /// </summary>
    /// <param name="rootNode"> the parent node </param>
    /// <returns> an integer representing if one side is higher than the other </returns>
    private int BalanceFactor(Node<T> current)
    {
        int left = GetSubTreeHeight(current.LeftChild);
        int right = GetSubTreeHeight(current.RightChild);
        int factor = left - right;
        return factor;
    }
    /// <summary>
    /// Finds the maximum height of a subtree,
    /// </summary>
    /// <param name="current"> the current node </param>
    /// <returns> an integer representing the subtree height </returns>
    private int GetSubTreeHeight(Node<T> current)
    {
        int height = 0;
        if (current != null)
        {
            int l = GetSubTreeHeight(current.LeftChild);
            int r = GetSubTreeHeight(current.RightChild);
            int m = Math.Max(l, r);
            height = m + 1;
        }
        return height;
    }
    /// <summary>
    /// Completes a RR rotation for a node.
    /// </summary>
    /// <param name="parent"> the parent node </param>
    /// <returns> a new node and its references rotated RR </returns>
    private Node<T> RotateRR(Node<T> parent)
    {
        Node<T> pivot = parent.RightChild;
        parent.RightChild = pivot.LeftChild;
        if (parent.RightChild != null) parent.RightChild.Parent = parent;
        pivot.LeftChild = parent;
        pivot.Parent = parent.Parent;
        parent.Parent = pivot;
        return pivot;
    }
    /// <summary>
    /// Completes a RL rotation for a node.
    /// </summary>
    /// <param name="parent"> the parent node </param>
    /// <returns> a new node and its references rotated RL </returns>
    private Node<T> RotateRL(Node<T> parent)
    {
        Node<T> pivot = parent.RightChild;
        parent.RightChild = RotateLL(pivot);
        return RotateRR(parent);
    }
    /// <summary>
    /// Completes a LL rotation for a node.
    /// </summary>
    /// <param name="parent"> the parent node </param>
    /// <returns> a new node and its references rotated LL </returns>
    private Node<T> RotateLL(Node<T> parent)
    {
        Node<T> pivot = parent.LeftChild; 
        parent.LeftChild = pivot.RightChild;
        if (parent.LeftChild != null) parent.LeftChild.Parent = parent;
        pivot.RightChild = parent; 
        pivot.Parent = parent.Parent; 
        parent.Parent = pivot;
        return pivot;
    }
    /// <summary>
    /// Completes a LR rotation for a node.
    /// </summary>
    /// <param name="parent"> the parent node </param>
    /// <returns> a new node and its references rotated LR </returns>
    private Node<T> RotateLR(Node<T> parent)
    {
        Node<T> pivot = parent.LeftChild;
        parent.LeftChild = RotateRR(pivot);
        return RotateLL(parent);
    }

    /// <summary>
    /// This method starts with the Root node and moves down until the data 
    /// is found from a specific node by using the worker Remove method.
    /// </summary>
    /// <param name="data"> the data of the node to remove </param>
    public void Remove(T data)
    {
        Remove(Root, data);
    }
    /// <summary>
    /// The main worker method to Remove a node from any part of a tree.
    /// </summary>
    /// <param name="node"> the node to start at </param>
    /// <param name="data"> the data belonging to the specific node to remove </param>
    /// <exception cref="ArgumentException"></exception>
    private void Remove(Node<T> node, T data)
    {
        if (node == null) throw new ArgumentException($"The node {data} does not exist. Will not remove {data}");
        else if (data.CompareTo(node.Data) < 0) Remove(node.LeftChild, data);
        else if (data.CompareTo(node.Data) > 0) Remove(node.RightChild, data);
        else
        {
            if (node.LeftChild == null && node.RightChild == null)
            {
                Replace(node, null);
                Count--;
            }
            else if (node.RightChild == null)
            {
                Replace(node, node.LeftChild);
                Count--;
            }
            else if (node.LeftChild == null)
            {
                Replace(node, node.RightChild);
                Count--;
            }
            else
            {
                Node<T> guardian = FindMinInSubTree(node.RightChild);
                node.Data = guardian.Data;
                Remove(guardian, guardian.Data);
            }
        }
    }
    /// <summary>
    /// This method is to replace a selected node with the second node param.
    /// If the node that needs to be replaces is equal to its Parent's Left, the Parent's Left node will be replaced.
    /// Vice versa if the node is eqaul to its Parent's Right. 
    /// Else, this meanse the node does not have a Parent meaning that the new node will now be the Root.
    /// If the newNode happens to not be null, its Parent will be the Parent of the old node it replaced.
    /// </summary>
    /// <param name="nodeToBeRemoved"> the node to be replaced or removed </param>
    /// <param name="newNode"> the substituted node for the other's place </param>
    private void Replace(Node<T> nodeToBeRemoved, Node<T> newNode)
    {
        if (nodeToBeRemoved.Parent != null)
        {
            if (nodeToBeRemoved.Parent.LeftChild == nodeToBeRemoved) nodeToBeRemoved.Parent.LeftChild = newNode;
            else nodeToBeRemoved.Parent.RightChild = newNode;
        }
        else Root = newNode;

        if (newNode != null) newNode.Parent = nodeToBeRemoved.Parent;
    }
    /// <summary>
    /// If a node to remove has both Children, we must find a guardian to replace this parent (Like a new Guardian to adopt the Children).
    /// To do this we check to see if the node being replaced, the "largest" node in this subtree, has any Lefts. If so, we continue to the Left child
    /// until the next Left is null. This "Leftmost" child, although the smallest in the subtree, will STILL be "larger" than the orignial
    /// node being replaced's Left child, and smaller than the node being replaced's Right child. This "self balances" the tree.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private static Node<T> FindMinInSubTree(Node<T> node)
    {
        while (node.LeftChild != null)
        {
            node = node.LeftChild;
        }
        return node;
    }


    /// <summary>
    /// This method works by taking any traversal method, and finding the maximum node height based on
    /// each node in the tree using the Node<T> class's GetNodeHeight method.
    /// </summary>
    /// <returns> the height of the tree </returns>
    public int GetTreeHeight()
    {
        int h = 0;
        foreach (var node in Traverse(TraversalType.PREORDER))
        {
            h = Math.Max(h, node.GetNodeHeight());
        }
        return h;
    }
    /// <summary>
    /// This method creates a node list from the tree and depending on the traversal type
    /// the nodes will be added in that order type.
    /// </summary>
    /// <param name="type"> the traversal type </param>
    /// <returns> a node list sorted depending on the traversal type </returns>
    public List<Node<T>> Traverse(TraversalType type)
    {
        List<Node<T>> nodes = new();
        switch (type)
        {
            case TraversalType.PREORDER:
                PreOrderTraversal(Root, nodes);
                break;
            case TraversalType.INORDER:
                InOrderTraversal(Root, nodes);
                break;
            case TraversalType.POSTORDER:
                PostOrderTraversal(Root, nodes);
                break;
        }
        return nodes;
    }
    /// <summary>
    /// Traversal Type: PreOrder, we add the first node or the Root, and
    /// we proceed to do all LeftChilds until null, then from that Leftmost child thats not null, then do all of its
    /// Right children, recursively going all the way back up.
    /// </summary>
    /// <param name="node"> the starting node </param>
    /// <param name="result"> an empty list to add items to in a certain order </param>
    private void PreOrderTraversal(Node<T> node, List<Node<T>> result)
    {
        if (node != null)
        {
            result.Add(node);
            PreOrderTraversal(node.LeftChild, result);
            PreOrderTraversal(node.RightChild, result);
        }
    }
    /// <summary>
    /// Traversal Type: InOrder, we recursively start with the Leftmost child in the entire tree, once we reach null,
    /// we use the last Leftmost node and add it to the list. Once that is finished, we add the Root. Once that is finished,
    ///  we recursively proceed to do all RightChilds until null. Then from that Rightmost child thats not null, then do all
    ///  of its Rightchildren, recursively going all the way back up.
    /// </summary>
    /// <param name="node"> the starting node </param>
    /// <param name="result"> an empty list to add items to in a certain order </param>
    private void InOrderTraversal(Node<T> node, List<Node<T>> result)
    {
        if (node != null)
        {
            PreOrderTraversal(node.LeftChild, result);
            result.Add(node);
            PreOrderTraversal(node.RightChild, result);
        }
    }
    /// <summary>
    /// Traversal Type: PostOrder, we recursively start with the Leftmost child in the entire tree, once we reach null,
    /// we use the last Leftmost node and add it to the list. Once that is finished, we do the same recursively for the RightChildren. 
    /// Once that is finished, we finally add the Root.
    /// </summary>
    /// <param name="node"> the starting node </param>
    /// <param name="result"> an empty list to add items to in a certain order </param>
    private void PostOrderTraversal(Node<T> node, List<Node<T>> result)
    {
        if (node != null)
        {
            PreOrderTraversal(node.LeftChild, result);
            PreOrderTraversal(node.RightChild, result);
            result.Add(node);
        }
    }


    /// <summary>
    /// The visualize tree method calls the StoreTreeBranches recursive method.
    /// </summary>
    /// <returns> a tree string </returns>
    public string VisualizeTree()
    {
        string tree = $"{Root.Data}\n";
        tree += StoreTreeBranches(Root);
        return tree;
    }
    /// <summary>
    /// This methods stores each of the lines that represent each child node from the root. 
    /// After recursively passing the line, and is returned/
    /// </summary>
    /// <param name="node"> the current node </param>
    /// <param name="prefix"> the previous begining of the current node </param>
    /// <returns></returns>
    private string StoreTreeBranches(Node<T> node, string prefix = "")
    {
        string s = String.Empty;
        if (node.RightChild != null)
        {
            s += prefix + $"├── {node.RightChild.Data}\n";
            s += StoreTreeBranches(node.RightChild, prefix + "│   ");
        }

        if (node.LeftChild != null)
        {
            s += prefix + $"└── {node.LeftChild.Data}\n";
            s += StoreTreeBranches(node.LeftChild, prefix + "    ");
        }

        return s;
    }
}