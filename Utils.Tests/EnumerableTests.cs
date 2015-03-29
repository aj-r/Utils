using System;
using System.Linq;
using NUnit.Framework;
using Utils.Linq;

namespace Utils.Tests
{
    [TestFixture]
    public class EnumerableTests
    {
        [TestCase("1,2,3", 1, 0, 3, Result = "0")]
        [TestCase("1,2,3", 1, 0, -1, Result = "0")]
        [TestCase("1,2,3", 4, 0, 3, Result = "")]
        [TestCase("", 0, 0, 0, Result = "")]
        [TestCase("22,33,22", 22, 0, 3, Result = "0,2")]
        [TestCase("22,33,22", 22, 0, -1, Result = "0,2")]
        [TestCase("22,33,22", 22, 0, 0, Result = "")]
        [TestCase("22,33,22", 22, 1, 1, Result = "")]
        [TestCase("22,33,22", 22, 1, 2, Result = "2")]
        public string IndicesOfTest(string source, int value, int startIndex, int count)
        {
            var indices = source.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).IndicesOf(value, startIndex, count);
            return string.Join(",", indices);
        }

        [TestCase("1,2,3", 1, 0, 3, Result = 0)]
        [TestCase("1,2,3", 1, 0, -1, Result = 0)]
        [TestCase("1,2,3", 4, 0, 3, Result = -1)]
        [TestCase("", 0, 0, 0, Result = -1)]
        [TestCase("22,33,22", 22, 0, 3, Result = 0)]
        [TestCase("22,33,22", 22, 0, -1, Result = 0)]
        [TestCase("22,33,22", 22, 0, 0, Result = -1)]
        [TestCase("22,33,22", 22, 1, 1, Result = -1)]
        [TestCase("22,33,22", 22, 1, 2, Result = 2)]
        public int IndexOfTest(string source, int value, int startIndex, int count)
        {
            return source.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).IndexOf(value, startIndex, count);
        }
    }
}
