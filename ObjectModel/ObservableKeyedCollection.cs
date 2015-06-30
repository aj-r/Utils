using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SharpUtils.ObjectModel
{
    /// <summary>
    /// A KeyedCollection that fires events when items are added, removed, or modified.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection.</typeparam>
    /// <typeparam name="TItem">The type of the values in the collection.</typeparam>
    public abstract class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged, INotifyPropertyChanged, IXmlSerializable
    {
        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ObservableKeyedCollection{TKey,TItem}"/> instance.
        /// </summary>
        public ObservableKeyedCollection()
        { }

        /// <summary>
        /// Creates a new <see cref="ObservableKeyedCollection{TKey,TItem}"/> instance.
        /// </summary>
        /// <param name="comparer">The IEqualityComparer to use when comparing keys, or <value>null</value> to use the default comparer.</param>
        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer)
            : base(comparer)
        { }

        /// <summary>
        /// Creates a new <see cref="ObservableKeyedCollection{TKey,TItem}"/> instance.
        /// </summary>
        /// <param name="comparer">The IEqualityComparer to use when comparing keys, or <value>null</value> to use the default comparer.</param>
        /// <param name="dictionaryCreationThreshold">The number of elements that the collection can hold without creating a lookup dictionary.</param>
        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
            : base(comparer, dictionaryCreationThreshold)
        { }

        /// <summary>
        /// Creates a new <see cref="ObservableKeyedCollection{TKey,TItem}"/> instance.
        /// </summary>
        /// <param name="items">The items that are copied to the new collection.</param>
        public ObservableKeyedCollection(IEnumerable<TItem> items)
        {
            if (items == null)
                return;
            int index = 0;
            foreach (var item in items)
            {
                base.InsertItem(index, item);
                index++;
            }
        }

        #endregion

        private bool deferNotify = false;

        /// <summary>
        /// Replaces the item at the specified index with the specified item.
        /// </summary>
        /// <param name="index">The zero-based index of the item to be replaced.</param>
        /// <param name="item">The new item.</param>
        protected override void SetItem(int index, TItem item)
        {
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, index));
        }

        /// <summary>
        /// Inserts an item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="item">The item to insert.</param>
        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        /// <summary>
        /// Adds multiple items to the collection and raises a single CollectionChanged event.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public void AddRange(IEnumerable<TItem> items)
        {
            if (items == null)
                return;
            var added = false;
            try
            {
                deferNotify = true;
                foreach (var item in items)
                {
                    Add(item);
                    added = true;
                }
            }
            finally
            {
                deferNotify = false;
            }
            if (!added)
                return;
            // UIElement doesn't support changing multiple items at once, so use the reset action.
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes multiple items from the dictionary and raises a single CollectionChanged event.
        /// </summary>
        /// <param name="keys">The keys of the items to remove.</param>
        public void RemoveRange(IEnumerable<TKey> keys)
        {
            if (keys == null)
                return;

            var removed = false;
            try
            {
                deferNotify = true;
                foreach (var key in keys)
                {
                    if (Remove(key))
                        removed = true;
                }
            }
            finally
            {
                deferNotify = false;
            }
            if (!removed)
                return;
            // UIElement doesn't support changing multiple items at once, so use the reset action.
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes multiple items from the dictionary and raises a single CollectionChanged event.
        /// </summary>
        /// <param name="items">The the items to remove.</param>
        public void RemoveRange(IEnumerable<TItem> items)
        {
            if (items == null)
                return;

            var removed = false;
            try
            {
                deferNotify = true;
                foreach (var item in items)
                {
                    if (Remove(item))
                        removed = true;
                }
            }
            finally
            {
                deferNotify = false;
            }
            if (!removed)
                return;
            // UIElement doesn't support changing multiple items at once, so use the reset action.
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes all elements from the collection.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="e">Arguments for the event.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

        /// <summary>
        /// Raises the OnCollectionChanged event.
        /// </summary>
        /// <param name="e">Arguments for the event.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (deferNotify)
                return;

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region INotifyCollectionChanged Members

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
        /// </summary>
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.ReadStartElement();
            if (reader.IsEmptyElement)
                return;
            var ser = new XmlSerializer(typeof(TItem));
            var nodeType = reader.MoveToContent();
            if (nodeType == XmlNodeType.None)
                return;
            while (nodeType != XmlNodeType.EndElement)
            {
                var item = (TItem)ser.Deserialize(reader);
                if (item == null)
                    continue;
                Add(item);
                nodeType = reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            var ser = new XmlSerializer(typeof(TItem));
            foreach (TItem item in this)
            {
                ser.Serialize(writer, item);
            }
        }

        #endregion
    }
}
