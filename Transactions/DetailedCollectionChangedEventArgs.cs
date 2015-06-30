using System;
using System.Collections.Generic;

namespace SharpUtils.Transactions
{
    /// <summary>
    /// Represents a method that will handle a DetailedCollectionChanged event.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="sender">The collection for which the event was raised.</param>
    /// <param name="e">Arguments for the event.</param>
    public delegate void DetailedCollectionChangedEventHandler<T>(object sender, DetailedCollectionChangedEventArgs<T> e);

    /// <summary>
    /// Event data that contains details for each item that changed in the collection.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class DetailedCollectionChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Creates a new <see cref="DetailedCollectionChangedEventArgs{T}"/> instance.
        /// </summary>
        /// <param name="changes">The changes to the collection that occurred for the current event.</param>
        public DetailedCollectionChangedEventArgs(IReadOnlyCollection<ICollectionChange<T>> changes)
        {
            Changes = changes;
        }

        /// <summary>
        /// Gets the changes to the collection that occurred for the current event.
        /// </summary>
        public IReadOnlyCollection<ICollectionChange<T>> Changes { get; private set; }
    }
}
