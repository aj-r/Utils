using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Linq
{
    public static class Enumerable
    {
        /// <summary>
        /// Determines whether the number of elements in a sequence is equal to a certain value.
        /// </summary>
        /// <typeparam name="T">The type of elements or source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The expected number of elements.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is more efficient than Enumerable.Count() because it will stop enumerating items after it passes the count.
        /// This, if the actual number of elements is much more than count, this method will perform faster.
        /// However, it will be the same speed if source is an ICollection&lt;T&gt;.
        /// </remarks>
        public static bool HasCount<T>(this IEnumerable<T> source, int count)
        {
            var collection = source as ICollection<T>;
            if (collection != null)
                return collection.Count == count;
            return source.Take(count + 1).Count() == count;
        }

        /// <summary>
        /// Determines whether the number of elements in a sequence is equal to a certain value.
        /// </summary>
        /// <typeparam name="T">The type of elements or source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The expected number of elements.</param>
        /// <returns></returns>
        public static bool HasCount<T>(this ICollection<T> source, int count)
        {
            return source.Count == count;
        }

        /// <summary>
        /// Determines whether the sequence contains at least a certain number of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements or source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The minimum number of elements.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is more efficient than Enumerable.Count() because it will stop enumerating items after it passes the count.
        /// This, if the actual number of elements is much more than count, this method will perform faster.
        /// However, it will be the same speed if source is an ICollection&lt;T&gt;.
        /// </remarks>
        public static bool HasAtLeast<T>(this IEnumerable<T> source, int count)
        {
            var collection = source as ICollection<T>;
            if (collection != null)
                return collection.Count >= count;
            return source.Take(count).Count() == count;
        }

        /// <summary>
        /// Determines whether the sequence contains at least a certain number of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements or source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The minimum number of elements.</param>
        /// <returns></returns>
        public static bool HasAtLeast<T>(this ICollection<T> source, int count)
        {
            return source.Count >= count;
        }

        /// <summary>
        /// Determines whether the sequence contains at most a certain number of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements or source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The minimum number of elements.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method is more efficient than Enumerable.Count() because it will stop enumerating items after it passes the count.
        /// This, if the actual number of elements is much more than count, this method will perform faster.
        /// However, it will be the same speed if source is an ICollection&lt;T&gt;.
        /// </remarks>
        public static bool HasAtMost<T>(this IEnumerable<T> source, int count)
        {
            var collection = source as ICollection<T>;
            if (collection != null)
                return collection.Count <= count;
            return source.Take(count + 1).Count() <= count;
        }

        /// <summary>
        /// Determines whether the sequence contains at most a certain number of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements or source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The minimum number of elements.</param>
        /// <returns></returns>
        public static bool HasAtMost<T>(this ICollection<T> source, int count)
        {
            return source.Count <= count;
        }

        /// <summary>
        /// Determines whether the sequence contains at most a certain number of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements or source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="startIndex">The index to start taking elements.</param>
        /// <param name="endIndex">The index to stop taking elements. The element at this index is not included.</param>
        /// <returns></returns>
        public static IEnumerable<T> Range<T>(this IEnumerable<T> source, int startIndex, int endIndex)
        {
            return source.Skip(startIndex).Take(endIndex - startIndex);
        }
    }
}
