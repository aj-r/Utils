using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Utils.ObjectModel;

namespace Utils.Tests
{
    [TestFixture]
    public class TransactableCollectionTests
    {
        [Test]
        public void Add()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            Assert.AreEqual(3, coll[0]);
            coll.Add(5);
            Assert.AreEqual(5, coll[1]);
        }

        [Test]
        public void Insert()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            coll.Add(5);
            coll.Insert(1, 9);
            Assert.AreEqual(9, coll[1]);
            coll.Insert(3, -5);
            Assert.AreEqual(-5, coll[3]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertOutOfRange()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            coll.Add(5);
            coll.Insert(3, -5);
        }

        [Test]
        public void Remove()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            coll.Add(5);

            coll.Remove(3);
            Assert.AreEqual(1, coll.Count);
            Assert.AreEqual(5, coll[0]);

            coll.Remove(3);
            Assert.AreEqual(1, coll.Count);
            Assert.AreEqual(5, coll[0]);
        }

        [Test]
        public void RemoveAt()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(1);
            coll.Add(0);

            coll.RemoveAt(1);
            Assert.AreEqual(1, coll.Count);
            Assert.AreEqual(1, coll[0]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveAtOutOfRange()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(1);
            coll.Add(0);
            coll.RemoveAt(2);
        }

        [Test]
        public void AddRange()
        {
            var coll = new TransactableCollection<int>();
            coll.AddRange(Enumerable.Range(3, 6));
            Assert.AreEqual(6, coll.Count);
            Assert.AreEqual(3, coll[0]);
            Assert.AreEqual(8, coll[5]);
        }

        [Test]
        public void InsertRange()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            coll.Add(5);
            coll.Insert(1, 9);
            Assert.AreEqual(9, coll[1]);
            coll.Insert(3, -5);
            Assert.AreEqual(-5, coll[3]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertRangeOutOfRange()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            coll.Add(5);
            coll.InsertRange(3, new List<int> { 4, 7, 8 });
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertRangeOutOfRange2()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            coll.Add(5);
            coll.InsertRange(-1, new List<int> { 4, 7, 8 });
        }

        [Test]
        public void RemoveRange()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(5);
            coll.Add(6);
            coll.Add(7);
            coll.Add(8);
            coll.Add(9);
            coll.Add(10);
            coll.RemoveRange(1, 4);
            Assert.AreEqual(2, coll.Count);
            Assert.AreEqual(5, coll[0]);
            Assert.AreEqual(10, coll[1]);

            coll.RemoveRange(0, 2);
            Assert.AreEqual(0, coll.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveRangeOutOfRange()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            coll.Add(5);
            coll.RemoveRange(3, 1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveRangeOutOfRange2()
        {
            var coll = new TransactableCollection<int>();
            coll.Add(3);
            coll.Add(5);
            coll.RemoveRange(1, 4);
        }

        [Test]
        public void CollectionChangeAdd()
        {
            int collectionChangedCount = 0;
            int detailedCollectionChangedCount = 0;
            var coll = new TransactableCollection<int>();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;
            coll.DetailedCollectionChanged += (sender, e) => detailedCollectionChangedCount++;

            coll.Add(3);
            Assert.AreEqual(1, collectionChangedCount);
            Assert.AreEqual(1, detailedCollectionChangedCount);
        }

        [Test]
        public void CollectionChangeInsert()
        {
            int collectionChangedCount = 0;
            int detailedCollectionChangedCount = 0;
            var coll = new TransactableCollection<int>();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;
            coll.DetailedCollectionChanged += (sender, e) => detailedCollectionChangedCount++;

            coll.Insert(0, 3);
            Assert.AreEqual(1, collectionChangedCount);
            Assert.AreEqual(1, detailedCollectionChangedCount);
        }

        [Test]
        public void CollectionChangeRemove()
        {
            int collectionChangedCount = 0;
            int detailedCollectionChangedCount = 0;
            var coll = new TransactableCollection<int>();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;
            coll.DetailedCollectionChanged += (sender, e) => detailedCollectionChangedCount++;

            coll.Add(3);
            coll.Add(5);

            collectionChangedCount = 0;
            detailedCollectionChangedCount = 0;
            coll.Remove(3);
            Assert.AreEqual(1, collectionChangedCount);
            Assert.AreEqual(1, detailedCollectionChangedCount);

            collectionChangedCount = 0;
            detailedCollectionChangedCount = 0;
            coll.Remove(4);
            Assert.AreEqual(0, collectionChangedCount);
            Assert.AreEqual(0, detailedCollectionChangedCount);
        }

        [Test]
        public void CollectionChangeRemoveAt()
        {
            int collectionChangedCount = 0;
            int detailedCollectionChangedCount = 0;
            var coll = new TransactableCollection<int>();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;
            coll.DetailedCollectionChanged += (sender, e) => detailedCollectionChangedCount++;

            coll.Add(1);
            coll.Add(0);

            collectionChangedCount = 0;
            detailedCollectionChangedCount = 0;
            coll.RemoveAt(1);
            Assert.AreEqual(1, collectionChangedCount);
            Assert.AreEqual(1, detailedCollectionChangedCount);
        }

        [Test]
        public void CollectionChangeAddRange()
        {
            int collectionChangedCount = 0;
            int detailedCollectionChangedCount = 0;
            var coll = new TransactableCollection<int>();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;
            coll.DetailedCollectionChanged += (sender, e) => detailedCollectionChangedCount++;

            coll.AddRange(new List<int> { 4, 8, 5, 10 });
            Assert.AreEqual(1, collectionChangedCount);
            Assert.AreEqual(1, detailedCollectionChangedCount);

            collectionChangedCount = 0;
            detailedCollectionChangedCount = 0;
            coll.AddRange(new List<int>());
            Assert.AreEqual(0, collectionChangedCount);
            Assert.AreEqual(0, detailedCollectionChangedCount);
        }

        [Test]
        public void CollectionChangeInsertRange()
        {
            int collectionChangedCount = 0;
            int detailedCollectionChangedCount = 0;
            var coll = new TransactableCollection<int>();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;
            coll.DetailedCollectionChanged += (sender, e) => detailedCollectionChangedCount++;

            coll.InsertRange(0, new List<int> { 11, 12, 76, 45, 34, 1000 });
            Assert.AreEqual(1, collectionChangedCount);
            Assert.AreEqual(1, detailedCollectionChangedCount);

            collectionChangedCount = 0;
            detailedCollectionChangedCount = 0;
            coll.InsertRange(0, new List<int>());
            Assert.AreEqual(0, collectionChangedCount);
            Assert.AreEqual(0, detailedCollectionChangedCount);
        }

        [Test]
        public void CollectionChangeRemoveRange()
        {
            int collectionChangedCount = 0;
            int detailedCollectionChangedCount = 0;
            var coll = new TransactableCollection<int>();
            coll.CollectionChanged += (sender, e) => collectionChangedCount++;
            coll.DetailedCollectionChanged += (sender, e) => detailedCollectionChangedCount++;

            coll.Add(5);
            coll.Add(6);
            coll.Add(7);
            coll.Add(8);
            coll.Add(9);
            coll.Add(10);

            collectionChangedCount = 0;
            detailedCollectionChangedCount = 0;
            coll.RemoveRange(1, 4);
            Assert.AreEqual(1, collectionChangedCount);
            Assert.AreEqual(1, detailedCollectionChangedCount);

            collectionChangedCount = 0;
            detailedCollectionChangedCount = 0;
            coll.RemoveRange(2, 0);
            Assert.AreEqual(0, collectionChangedCount);
            Assert.AreEqual(0, detailedCollectionChangedCount);
        }
    }
}
