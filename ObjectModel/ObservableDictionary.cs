using System;
using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Utils.ObjectModel
{
    /// <summary>
    /// A dictionary that fires events when items are added, removed, or modified.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public class ObservableDictionary<TKey, TValue> : NotifyPropertyChangedBase, IDictionary<TKey, TValue>, INotifyCollectionChanged, ISerializable
    {
        private const string serializationName = "dictionary";

        private readonly Dictionary<TKey, TValue> dictionary;

        /// <summary>
        /// Gets the underlying Dictionary instance.
        /// </summary>
        protected Dictionary<TKey, TValue> Dictionary
        {
            get { return dictionary; }
        }

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="ObservableDictionary{TKey,TValue}"/> instance.
        /// </summary>
        public ObservableDictionary()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Creates a new <see cref="ObservableDictionary{TKey,TValue}"/> instance.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the dictionary can contain.</param>
        public ObservableDictionary(int capacity)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// Creates a new <see cref="ObservableDictionary{TKey,TValue}"/> instance.
        /// </summary>
        /// <param name="dictionary">The IDictionary whose elements are copied to the new dictionary.</param>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Creates a new <see cref="ObservableDictionary{TKey,TValue}"/> instance.
        /// </summary>
        /// <param name="comparer">The IEqualityComparer to use when comparing keys, or <value>null</value> to use the default comparer.</param>
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// Creates a new <see cref="ObservableDictionary{TKey,TValue}"/> instance.
        /// </summary>
        /// <param name="dictionary">The IDictionary whose elements are copied to the new dictionary.</param>
        /// <param name="comparer">The IEqualityComparer to use when comparing keys, or <value>null</value> to use the default comparer.</param>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// Creates a new <see cref="ObservableDictionary{TKey,TValue}"/> instance.
        /// </summary>
        protected ObservableDictionary(SerializationInfo info, StreamingContext context)
        {
            dictionary = info.GetValue(serializationName, typeof(Dictionary<TKey, TValue>)) as Dictionary<TKey, TValue>;
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        /// <summary>
        /// Adds an item to the dictionary.
        /// </summary>
        /// <param name="key">The key of the item to add.</param>
        /// <param name="value">The value to add.</param>
        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        /// <summary>
        /// Removes an item from the dictionary.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        /// <returns><value>true</value> if an item with the specified key was removed; <value>false</value> if the specified key was not found.</returns>
        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            TValue value;
            Dictionary.TryGetValue(key, out value);
            var removed = Dictionary.Remove(key);
            if (removed)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, value));

            return removed;
        }

        /// <summary>
        /// Determines whether the current dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns><value>true</value> if the key was found; otherwise <value>false</value>.</returns>
        public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Determines whether the current dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key if the key was found; otherwise the default value of the dictionary value type.</param>
        /// <returns><value>true</value> if the key was found; otherwise <value>false</value>.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection containing the keys in the dictionary.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return Dictionary.Keys; }
        }

        /// <summary>
        /// Gets a collection containing the values in the dictionary.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return Dictionary.Values; }
        }

        /// <summary>
        /// Gets or set a value associated with the specified key in the dictionary.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return Dictionary[key];
            }
            set
            {
                Insert(key, value, false);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Adds an entry to the dictionary.
        /// </summary>
        /// <param name="item">The key and value of the item to add.</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Insert(item.Key, item.Value, true);
        }

        /// <summary>
        /// Removes all entries from the dictionary.
        /// </summary>
        public void Clear()
        {
            if (Dictionary.Count == 0)
                return;
            Dictionary.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey,TValue>>)Dictionary).Contains(item);
        }

        void ICollection<KeyValuePair<TKey,TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey,TValue>>)Dictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of key/value pairs in the dictionary.
        /// </summary>
        public int Count
        {
            get { return Dictionary.Count; }
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey,TValue>>)Dictionary).IsReadOnly; }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the dictionary.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Dictionary).GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        /// <summary>
        /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
        /// </summary>
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        /// <summary>
        /// Adds multiple items to the dictionary and raises a single CollectionChanged event.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            KeyValuePair<TKey, TValue>? kvp = null;
            var count = 0;
            foreach (var item in items)
            {
                if (Dictionary.ContainsKey(item.Key))
                    throw new ArgumentException("An item with the same key has already been added.");
                Dictionary.Add(item.Key, item.Value);
                if (kvp == null)
                    kvp = item;
                ++count;
            }
            if (count == 0)
                return;
            // Apparently the .NET framework is not good at handling a single CollectionChanged Add event for multiple elements,
            // so we need to use Reset instead in that case.
            var e = count > 1 
                ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
                : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, kvp.Value);
            OnCollectionChanged(e);
        }

        /// <summary>
        /// Removes multiple items from the dictionary and raises a single CollectionChanged event.
        /// </summary>
        /// <param name="keys">The keys of the items to remove.</param>
        public void RemoveRange(IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            KeyValuePair<TKey, TValue>? kvp = null;
            var count = 0;
            foreach (var key in keys)
            {
                TValue currentValue;
                if (!Dictionary.TryGetValue(key, out currentValue))
                    continue;
                Dictionary.Remove(key);
                if (kvp == null)
                    kvp = new KeyValuePair<TKey, TValue>(key, currentValue);
                ++count;
            }
            // Apparently the .NET framework is not good at handling a single CollectionChanged Add event for multiple elements,
            // so we need to use Reset instead in that case.
            var e = count > 1
                ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
                : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, kvp.Value);
            OnCollectionChanged(e);
        }

        /// <summary>
        /// Inserts an item into the dictionary.
        /// </summary>
        /// <param name="key">The key of the item to insert.</param>
        /// <param name="value">The value to insert.</param>
        /// <param name="add">True if an addition is required; false if it is permissible to set an existing value.</param>
        protected virtual void Insert(TKey key, TValue value, bool add)
        {
            if (key == null) throw new ArgumentNullException("key");

            TValue item;
            if (Dictionary.TryGetValue(key, out item))
            {
                if (add)
                    throw new ArgumentException("An item with the same key has already been added.");
                if (Equals(item, value))
                    return;
                Dictionary[key] = value;
                var e = new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace,
                    new KeyValuePair<TKey, TValue>(key, value),
                    new KeyValuePair<TKey, TValue>(key, item));
                OnCollectionChanged(e);
            }
            else
            {
                Dictionary[key] = value;
                var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
                OnCollectionChanged(e);
            }
        }

        /// <summary>
        /// Raises the CollectionChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Replace)
            {
                RaisePropertyChanged(() => Count);
                RaisePropertyChanged(() => Keys);
            }
            RaisePropertyChanged(() => Values);
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var d = (ISerializable)dictionary;
            info.AddValue(serializationName, d, typeof(Dictionary<TKey, TValue>));
        }

        #endregion
    }
}