using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Utils.ObjectModel
{
    public abstract class ObservableKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>, INotifyCollectionChanged, INotifyPropertyChanged, IXmlSerializable
    {
        public ObservableKeyedCollection()
        { }

        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer)
            : base(comparer)
        { }

        public ObservableKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
            : base(comparer, dictionaryCreationThreshold)
        { }

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

        #region IXmlSerializable Members

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
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

        public void WriteXml(XmlWriter writer)
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
