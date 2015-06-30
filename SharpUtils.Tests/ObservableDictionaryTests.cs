using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SharpUtils.ObjectModel;

namespace SharpUtils.Tests
{
    [TestFixture]
    public class ObservableDictionaryTests
    {
        [Test]
        public void Add()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.Add(3, 1);
            Assert.AreEqual(1, dic[3]);
            dic.Add(5, 80);
            Assert.AreEqual(80, dic[5]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddDuplicate()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.Add(3, 1);
            dic.Add(3, 80);
        }

        [Test]
        public void Set()
        {
            var dic = new ObservableDictionary<int, int>();
            dic[3] = 1;
            Assert.AreEqual(1, dic[3]);
            dic[3] = -6;
            Assert.AreEqual(-6, dic[3]);
        }

        [Test]
        public void Remove()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.Add(3, 6);
            dic.Add(1, 3);

            bool success = dic.Remove(3);
            Assert.IsTrue(success);
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual(3, dic[1]);
        }

        [Test]
        public void RemoveOutOfRange()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.Add(1, 1);
            dic.Add(2, 2);
            bool success = dic.Remove(0);
            Assert.IsFalse(success);
        }

        [Test]
        public void AddRange()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.AddRange(new Dictionary<int, int>
            {
                { 3, 10 },
                { 5, 6 },
                { 1090, 10 },
                { -3009, 44 },
            });
            Assert.AreEqual(4, dic.Count);
            Assert.AreEqual(10, dic[3]);
            Assert.AreEqual(6, dic[5]);
            Assert.AreEqual(10, dic[1090]);
            Assert.AreEqual(44, dic[-3009]);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddRangeDuplicate()
        {
            var dic = new ObservableDictionary<int, int>();
            dic.AddRange(new Dictionary<int, int>
            {
                { 3, 10 },
                { 5, 6 },
                { 3, 11 },
                { -3009, 44 },
            });
        }

        [Test]
        public void CollectionChangeAdd()
        {
            int collectionChangedCount = 0;
            var dic = new ObservableDictionary<int, int>();
            dic.CollectionChanged += (sender, e) => collectionChangedCount++;

            dic.Add(3, 9);
            Assert.AreEqual(1, collectionChangedCount);
        }

        [Test]
        public void CollectionChangeSet()
        {
            int collectionChangedCount = 0;
            var dic = new ObservableDictionary<int, int>();
            dic.CollectionChanged += (sender, e) => collectionChangedCount++;

            dic[3] = 9;
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            dic[3] = 10;
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            dic[3] = 10;
            Assert.AreEqual(0, collectionChangedCount);
        }

        [Test]
        public void CollectionChangeRemove()
        {
            int collectionChangedCount = 0;
            var dic = new ObservableDictionary<int, int>();
            dic.CollectionChanged += (sender, e) => collectionChangedCount++;

            dic.Add(3, 8);
            dic.Add(5, 8);

            collectionChangedCount = 0;
            dic.Remove(3);
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            dic.Remove(3);
            Assert.AreEqual(0, collectionChangedCount);
        }

        [Test]
        public void CollectionChangeAddRange()
        {
            int collectionChangedCount = 0;
            var dic = new ObservableDictionary<int, int>();
            dic.CollectionChanged += (sender, e) => collectionChangedCount++;

            dic.AddRange(new Dictionary<int, int>
            {
                { 4, 8 }, 
                { 5, 10 },
            });
            Assert.AreEqual(1, collectionChangedCount);

            collectionChangedCount = 0;
            dic.AddRange(new Dictionary<int, int>());
            Assert.AreEqual(0, collectionChangedCount);
        }
    }
}
