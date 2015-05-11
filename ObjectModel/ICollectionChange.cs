using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ObjectModel
{
    /// <summary>
    /// Represents a change to a collection.
    /// </summary>
    public interface ICollectionChange<out T>
    {
        /// <summary>
        /// Gets whether the items were added to the list.
        /// </summary>
        bool Inserted { get; }

        /// <summary>
        /// Gets whether the items were removed from the list.
        /// </summary>
        bool Removed { get; }

        /// <summary>
        /// Gets the list of items that were added or removed.
        /// </summary>
        IReadOnlyCollection<T> Items { get; }

        /// <summary>
        /// Gets the start index of the range of items that were added or removed.
        /// </summary>
        int StartIndex { get; }

        /// <summary>
        /// Gets the end index of the range of items after they were added, or before they were removed.
        /// </summary>
        int EndIndex { get; }
    }
}
