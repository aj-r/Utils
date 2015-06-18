using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ObjectModel
{
    /// <summary>
    /// Contains methods for querying LinkedList objects.
    /// </summary>
    public static class LinkedListExtensions
    {
        /// <summary>
        /// Gets all nodes in the LinkedList.
        /// </summary>
        /// <typeparam name="T">The type of elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A linked list that contains elements.</param>
        /// <returns>The nodes in the linked list.</returns>
        public static IEnumerable<LinkedListNode<T>> Nodes<T>(this LinkedList<T> source)
        {
            var node = source.First;
            while (node != null)
            {
                yield return node;
                node = node.Next;
            }
        }
    }
}
