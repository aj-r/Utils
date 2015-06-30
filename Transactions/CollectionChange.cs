using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SharpUtils.Transactions
{
    /// <summary>
    /// Represents a change to a collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class CollectionChange<T> : ICollectionChange<T>
    {
		/// <summary>
        /// Creates a new CollectionChange instance.
		/// </summary>
		/// <param name="inserted">A value that indicates whether the items were added to the list or removed from the list.</param>
		/// <param name="startIndex">The index in the list where the listItems were added or removed.</param>
		/// <param name="items">The list of items that were added or removed.</param>
        public CollectionChange(bool inserted, int startIndex, IReadOnlyCollection<T> items)
		{
            if (items == null)
                throw new ArgumentNullException("listItems");
			Inserted = inserted;
			StartIndex = startIndex;
			Items = items;
		}

		/// <summary>
		/// Gets whether the items were added to the list.
		/// </summary>
        public bool Inserted { get; private set; }

		/// <summary>
		/// Gets whether the items were removed from the list.
		/// </summary>
		public bool Removed
		{
			get { return !Inserted; }
		}

		/// <summary>
        /// Gets the list of items that were added or removed.
		/// </summary>
        public IReadOnlyCollection<T> Items { get; private set; }

		/// <summary>
		/// Gets the start index of the range of items that were added or removed.
		/// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// Gets the end index of the range of items after they were added, or before they were removed.
        /// </summary>
        public int EndIndex
        {
            get { return StartIndex + Items.Count; }
        }
    }
}
