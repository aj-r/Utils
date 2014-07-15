﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Utils.ObjectModel
{
    public abstract class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        public ObservableKeyedCollection()
        { }

        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer)
            : base(comparer)
        { }

        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
            : base(comparer, dictionaryCreationThreshold)
        { }

        private bool deferNotify = false;

        protected override void SetItem(int index, TItem item)
        {
            base.SetItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, index));
        }

        protected override void InsertItem(int index, TItem item)
        {
            base.InsertItem(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            if (items == null)
                return;
            deferNotify = true;
            bool added = false;
            foreach (var item in items)
            {
                Add(item);
                added = true;
            }
            deferNotify = false;
            if (!added)
                return;
            // UIElement doesn't support changing multiple items at once, so use the reset action.
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(IEnumerable<TKey> keys)
        {
            if (keys == null)
                return;
            deferNotify = true;
            bool added = false;
            foreach (var key in keys)
            {
                Remove(key);
                added = true;
            }
            deferNotify = false;
            if (!added)
                return;
            // UIElement doesn't support changing multiple items at once, so use the reset action.
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveRange(IEnumerable<TItem> items)
        {
            if (items == null)
                return;
            deferNotify = true;
            bool added = false;
            foreach (var item in items)
            {
                Remove(item);
                added = true;
            }
            deferNotify = false;
            if (!added)
                return;
            // UIElement doesn't support changing multiple items at once, so use the reset action.
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void RemoveItem(int index)
        {
            TItem item = this[index];
            base.RemoveItem(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

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

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region INotifyCollectionChanged Members

        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }
}
