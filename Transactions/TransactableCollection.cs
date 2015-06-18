using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Utils.Linq;

namespace Utils.Transactions
{
    /// <summary>
    /// An ObservableCollection that supports deferring the CollectionChanged notification until a set of multiple changes is complete.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class TransactableCollection<T> : ObservableCollection<T>, ITransactable
    {
        private readonly List<ICollectionChange<T>> changes = new List<ICollectionChange<T>>();

        private int transactionLevel;

        /// <summary>
        /// Creates a new <see cref="TransactableCollection{T}"/> instance.
        /// </summary>
        public TransactableCollection() { }

        /// <summary>
        /// Creates a new <see cref="TransactableCollection{T}"/> instance.
        /// </summary>
        /// <param name="collection">The collection from which elements are copied.</param>
        public TransactableCollection(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// Creates a new <see cref="TransactableCollection{T}"/> instance.
        /// </summary>
        /// <param name="list">The list from which elements are copied.</param>
        public TransactableCollection(List<T> list) : base(list) { }

        /// <summary>
        /// Occurs immediately after the CollectionChanged event(s). Contains details of every change that occured.
        /// </summary>
        public event DetailedCollectionChangedEventHandler<T> DetailedCollectionChanged;

        /// <summary>
        /// Begins a transaction. CollectionChanged events will be suppressed until the transaction is complete.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The return value implements IDisposable. As such, the general usage of this function is in the definition of a using block.
        /// If you call the method in this way, then you should never call EndTransaction(); it will be called automatically when the
        /// transaction is disposed.
        /// 
        /// Nested/concurrent transactions are allowed. If you use nested/concurrent transactions, the CollectionChanged event will not be raised until
        /// all transactions are complete.
        /// </remarks>
        public virtual IDisposable BeginTransaction()
        {
            return ((ITransactable)this).BeginTransaction();
        }

        void ITransactable.IncreaseTransactionLevel()
        {
            ++transactionLevel;
        }

        void ITransactable.DecreaseTransactionLevel()
        {
            if (!TransactionInProgress)
                return;
            --transactionLevel;
            if (TransactionInProgress || changes.Count == 0)
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
        /// Gets whether a transaction is currently in progress for the current collection.
        /// </summary>
        /// <returns><value>true</value> if a transaction is in progress; otherwise <value>false</value>.</returns>
        public bool TransactionInProgress
        {
            get { return transactionLevel > 0; }
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
            if (items == null)
                throw new ArgumentNullException("items");
            var collection = items as IReadOnlyCollection<T> ?? new ReadOnlyCollection<T>(items as IList<T> ?? items.ToList());
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
            var collection = new ReadOnlyCollection<T>(this.Skip(index).Take(count).ToList());
            changes.Add(new CollectionChange<T>(false, index, collection));
            using (BeginTransaction())
            {
                for (int i = 0; i < count; i++)
                {
                    base.RemoveItem(index);
                }
            }
        }

        /// <summary>
        /// Raises the CollectionChanged event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
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

        /// <summary>
        /// Raises the DetailedCollectionChanged event with the provided arguments.
        /// </summary>
        /// <param name="e">Arguments of the event being raised.</param>
        protected virtual void OnDetailedCollectionChanged(DetailedCollectionChangedEventArgs<T> e)
        {
            if (DetailedCollectionChanged != null)
                DetailedCollectionChanged(this, e);
        }

        /// <summary>
        /// Inserts an item into the collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            changes.Add(new CollectionChange<T>(true, index, new List<T> { item }));
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Removes an item from the specified index of the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            changes.Add(new CollectionChange<T>(false, index, new List<T> { this[index] }));
            base.RemoveItem(index);
        }

        /// <summary>
        /// Moves an item from one index to another.
        /// </summary>
        /// <param name="oldIndex">The zero-based index of the item to move.</param>
        /// <param name="newIndex">The zero-based index specifying the new location of the item.</param>
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

        /// <summary>
        /// Sets the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element.</param>
        /// <param name="item">The new value for the element.</param>
        protected override void SetItem(int index, T item)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException("index");
            var oldItem = this[index];
            changes.Add(new CollectionChange<T>(false, index, new List<T> { oldItem }));
            changes.Add(new CollectionChange<T>(true, index, new List<T> { item }));
            using (BeginTransaction())
                base.SetItem(index, item);
        }
    }
}
