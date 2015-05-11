using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.ObjectModel
{
    public delegate void DetailedCollectionChangedEventHandler<T>(object sender, DetailedCollectionChangedEventArgs<T> e);

    public class DetailedCollectionChangedEventArgs<T> : EventArgs
    {
        public DetailedCollectionChangedEventArgs(IReadOnlyCollection<ICollectionChange<T>> changes)
        {
            Changes = changes;
        }

        public IReadOnlyCollection<ICollectionChange<T>> Changes { get; private set; }
    }
}
