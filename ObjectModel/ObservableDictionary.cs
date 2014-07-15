using System;
using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Utils.ObjectModel
{
    public class ObservableDictionary<TKey, TValue> : NotifyPropertyChangedBase, IDictionary<TKey, TValue>, INotifyCollectionChanged, ISerializable
    {
        private const string serializationName = "dictionary";

        private Dictionary<TKey, TValue> dictionary;

        protected Dictionary<TKey, TValue> Dictionary
        {
            get { return dictionary; }
        }

        #region Constructors

        public ObservableDictionary()
        {
            dictionary = new Dictionary<TKey, TValue>();
        }

        public ObservableDictionary(int capacity)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        protected ObservableDictionary(SerializationInfo info, StreamingContext context)
        {
            dictionary = info.GetValue(serializationName, typeof(Dictionary<TKey, TValue>)) as Dictionary<TKey, TValue>;
        }

        #endregion

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return Dictionary.Keys; }
        }

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

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return Dictionary.Values; }
        }

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

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Insert(item.Key, item.Value, true);
        }

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

        public int Count
        {
            get { return Dictionary.Count; }
        }

        bool ICollection<KeyValuePair<TKey,TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey,TValue>>)Dictionary).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

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

        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        public void AddRange(IDictionary<TKey, TValue> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (items.Count == 0)
                return;
            if (items.Keys.Any((k) => Dictionary.ContainsKey(k)))
            {
                throw new ArgumentException("An item with the same key has already been added.");
            }
            else
            {
                foreach (var item in items)
                {
                    Dictionary.Add(item.Key, item.Value);
                }
            }
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items.ToArray());
            OnCollectionChanged(e);
        }

        private void Insert(TKey key, TValue value, bool add)
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var d = (ISerializable)dictionary;
            info.AddValue(serializationName, d, typeof(Dictionary<TKey, TValue>));
        }

        #endregion
    }
}