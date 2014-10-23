using System;
using System.Collections.Generic;

namespace Utils.ObjectModel
{
    /// <summary>
    /// A tree node.
    /// </summary>
    /// <typeparam name="T">The type of elements in the graph.</typeparam>
    public class TreeNode<T>
    {
        public TreeNode()
        { }

        public TreeNode(T data)
        {
            Value = data;
            Children = new LinkedList<TreeNode<T>>();
        }

        /// <summary>
        /// Gets or sets the value in the current node.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the children of the current node.
        /// </summary>
        public LinkedList<TreeNode<T>> Children { get; private set; }
    }
}
