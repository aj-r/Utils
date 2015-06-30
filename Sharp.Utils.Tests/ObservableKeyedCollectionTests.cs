using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Sharp.Utils.ObjectModel;

namespace Sharp.Utils.Tests
{
    [TestFixture]
    public class ObservableKeyedCollectionTests
    {
        [Test]
        public void Add()
        {
            var coll = new TestKeyedCollection();
            coll.Add(3);
            Assert.AreEqual(3, coll[3]);
            coll.Add(5);
            Assert.AreEqual(5, coll[5]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDuplicate()
        {
            var coll = new TestKeyedCollection();
            coll.Add(3);
            coll.Add(3);
        }

        [Test]
        public void Remove()
        {
            var coll = new TestKeyedCollection();
            coll.Add(3);
            coll.Add(1);

            bool success = coll.Remove(3);
            Assert.IsTrue(success);
            Assert.AreEqual(1, coll.Count);
            Assert.AreEqual(1, coll[1]);
        }

        [Test]
        public void RemoveOutOfRange()
        {
            var coll = new TestKeyedCollection();
            coll.Add(1);
            coll.Add(2);
            bool success = coll.Remove(0);
            Assert.IsFalse(success);
        }

        [Test]
        public void AddRange()
        {
            var coll = new TestKeyedCollection();
            coll.AddRange(new []
            {
                10,
                6,
                1090,
                -3009,
            });
            Assert.AreEqual(4, coll.Count);
            Assert.AreEqual(10, coll[10]);
            Assert.AreEqual(6, coll[6]);
            Assert.AreEqual(1090, coll[1090]);
            Assert.AreEqual(-3009, coll[-3009]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRangeDuplicate()
        {
            var coll = new TestKeyedCollection();
            coll.AddRange(new []
            {
                10,
                6,
                -3009,
                6,
            });
        }

        [Test]
        public void CollectionChangeAdd()
        {
            int collectionChangedCount = 0;
            var coll = new TestKeyedCollection();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;

            coll.Add(9);
            Assert.AreEqual(1, collectionChangedCount);
        }

        [Test]
        public void CollectionChangeRemove()
        {
            int collectionChangedCount = 0;
            var coll = new TestKeyedCollection();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;

            coll.Add(8);
            coll.Add(6);

            collectionChangedCount = 0;
            coll.Remove(8);
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            coll.Remove(8);
            Assert.AreEqual(0, collectionChangedCount);
        }

        [Test]
        public void CollectionChangeAddRange()
        {
            int collectionChangedCount = 0;
            var coll = new TestKeyedCollection();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;

            coll.AddRange(new []
            {
                4, 
                10,
            });
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            coll.AddRange(new int[0]);
            Assert.AreEqual(0, collectionChangedCount);
        }
    }

    public class TestKeyedCollection : ObservableKeyedCollection<int, int>
    {
        protected override int GetKeyForItem(int item)
        {
            return item;
        }
    }
}
