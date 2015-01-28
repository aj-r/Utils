using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Utils.Linq;

namespace Utils.ObjectModel
{
    /// <summary>
    /// An ObservableCollection that supports deferring the CollectionChanged notification until a set of multiple changes is complete.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class TransactableCollection<T> : ObservableCollection<T>
    {
        public TransactableCollection() { }
        public TransactableCollection(IEnumerable<T> collection) : base(collection) { }
        public TransactableCollection(List<T> list) : base(list) { }

        public event DetailedCollectionChangedEventHandler<T> DetailedCollectionChanged;

        /// <summary>
        /// Gets whether a transaction is currently in progress.
        /// </summary>
        public bool TransactionInProgress { get; private set; }

        /// <summary>
        /// Begins a transaction. CollectionChanged events will be suppressed until the transaction is complete.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The return value implements IDisposable. As such, the general usage of this function is in the definition of a using block.
        /// If you call the method in this way, then you should never call EndTransaction(); it will be called automatically when the
        /// transaction is disposed.
        /// </remarks>
        public Transaction BeginTransaction()
        {
            return new Transaction(this);
        }

        /// <summary>
        /// Ends a transaction. If the collection was modified while the transaction was active, then a CollectionChanged event will be fired.
        /// </summary>
        public void EndTransaction()
        {
            if (!TransactionInProgress)
                return;
            TransactionInProgress = false;
            if (changes.Count == 0)
                return;
            // Note: apparently some built-in WPF code assumes that a CollectionChanged event will add/remove at most one item.
            // As such, if we add/remove more then one item, we should use NotifyCollectionChangedAction.Reset.
            NotifyCollectionChangedEventArgs e = null;
            if (changes.Count == 1)
            {
                var change = changes[0];
                if (change.Items.HasCount(1))
                {
                    e = new NotifyCollectionChangedEventArgs(
                        change.Inserted ? NotifyCollectionChangedAction.Add : NotifyCollectionChangedAction.Remove,
                        change.Items.First(),
                        change.StartIndex);
                }
            }
            if (e == null)
                e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(e);
        }

        /// <summary>
        /// Adds the specified elements to the end of the collection.
        /// </summary>
        /// <param name="items">The elements to add.</param>
        public void AddRange(IEnumerable<T> items)
        {
            InsertRange(Count, items);
        }

        /// <summary>
        /// Inserts the specified elements at the specified index of the collection.
        /// </summary>
        /// <param name="index">The index at which to insert the elements.</param>
        /// <param name="items">The elements to insert</param>
        public void InsertRange(int index, IEnumerable<T> items)
        {
            if (index > Count || index < 0)
                throw new ArgumentOutOfRangeException("index");
            var collection = items as IReadOnlyCollection<T> ?? new ReadOnlyCollection<T>(new List<T>(items));
            if (collection.Count == 0)
                return;
            changes.Add(new CollectionChange<T>(true, index, collection));
            using (BeginTransaction())
            {
                foreach (var item in items)
                {
                    base.InsertItem(index, item);
                    index++;
                }
            }
        }

        /// <summary>
        /// Removes a range of elements from the collection.
        /// </summary>
        /// <param name="index">The index of the first element to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        public void RemoveRange(int index, int count)
        {
            if (count == 0)
                return;
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("index");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (index + count > Count)
                throw new ArgumentException("index and count do not denote a valid range of elements in the collection.");
            var collection = new ReadOnlyCollection<T>(new List<T>(this.Skip(index).Take(count)));
            changes.Add(new CollectionChange<T>(false, index, collection));
            using (BeginTransaction())
            {
                for (int i = 0; i < count; i++)
                {
                    base.RemoveItem(index);
                }
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!TransactionInProgress)
            {
                base.OnCollectionChanged(e);
                if (changes.Count > 0)
                {
                    var copy = changes.ToList();
                    changes.Clear();
                    OnDetailedCollectionChanged(new DetailedCollectionChangedEventArgs<T>(copy));
                }
            }
        }

        protected virtual void OnDetailedCollectionChanged(DetailedCollectionChangedEventArgs<T> e)
        {
            if (DetailedCollectionChanged != null)
                DetailedCollectionChanged(this, e);
        }

        protected override void InsertItem(int index, T item)
        {
            changes.Add(new CollectionChange<T>(true, index, new List<T> { item }));
            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index)
        {
            changes.Add(new CollectionChange<T>(false, index, new List<T> { this[index] }));
            base.RemoveItem(index);
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= Count)
                throw new ArgumentOutOfRangeException("oldIndex");
            if (newIndex < 0 || newIndex >= Count)
                throw new ArgumentOutOfRangeException("newIndex");
            if (oldIndex == newIndex)
                return;
            var item = this[oldIndex];
            changes.Add(new CollectionChange<T>(false, oldIndex, new List<T> { item }));
            changes.Add(new CollectionChange<T>(true, newIndex, new List<T> { item }));
            using (BeginTransaction())
                base.MoveItem(oldIndex, newIndex);
        }

        public class Transaction : IDisposable
        {
            private TransactableCollection<T> collection;

            internal Transaction(TransactableCollection<T> collection)
            {
                this.collection = collection;
                collection.TransactionInProgress = true;
            }

            public void Dispose()
            {
                collection.EndTransaction();
            }
        }

        private List<CollectionChange<T>> changes = new List<CollectionChange<T>>();
    }
}
