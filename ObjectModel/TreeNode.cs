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

        /// <summary>
        /// Gets the flattened tree in depth-first order.
        /// </summary>
        /// <returns>The list of the values of all tree notes starting with the current node.</returns>
        public IEnumerable<T> Flatten()
        {
            yield return Value;
            foreach (var child in Children)
                foreach (var item in child.Flatten())
                    yield return item;
        }
    }
}
