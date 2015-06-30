using System;
using System.Collections.Generic;
using System.Linq;
using SharpUtils.Transactions;

namespace SharpUtils.Linq
{
    /// <summary>
    /// Contains methods for querying objects that implements IEnumerable.
    /// </summary>
    public static class UtilsEnumerable
    {
        /// <summary>
        /// Determines whether the number of elements in a sequence is equal to a certain value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The expected number of elements.</param>
        /// <returns><value>true</value> if <paramref name="source"/> contains the specified number of elements exactly; otherwise <value>false</value>.</returns>
        /// <remarks>
        /// This method is more efficient than Enumerable.Count() because it will stop enumerating items after it passes the count.
        /// Thus, if the actual number of elements is much more than count, this method will perform faster.
        /// However, it will be the same speed if <paramref name="source"/> is an ICollection&lt;T&gt;.
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
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The expected number of elements.</param>
        /// <returns><value>true</value> if <paramref name="source"/> contains the specified number of elements exactly; otherwise <value>false</value>.</returns>
        public static bool HasCount<T>(this ICollection<T> source, int count)
        {
            return source.Count == count;
        }

        /// <summary>
        /// Determines whether the sequence contains at least a certain number of elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The minimum number of elements.</param>
        /// <returns><value>true</value> if <paramref name="source"/> contains at least the specified number of elements; otherwise <value>false</value>.</returns>
        /// <remarks>
        /// This method is more efficient than Enumerable.Count() because it will stop enumerating items after it passes the count.
        /// Thus, if the actual number of elements is much more than count, this method will perform faster.
        /// However, it will be the same speed if <paramref name="source"/> is an ICollection&lt;T&gt;.
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
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The minimum number of elements.</param>
        /// <returns><value>true</value> if <paramref name="source"/> contains at least the specified number of elements; otherwise <value>false</value>.</returns>
        public static bool HasAtLeast<T>(this ICollection<T> source, int count)
        {
            return source.Count >= count;
        }

        /// <summary>
        /// Determines whether the sequence contains at most a certain number of elements.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The minimum number of elements.</param>
        /// <returns><value>true</value> if <paramref name="source"/> contains at most the specified number of elements; otherwise <value>false</value>.</returns>
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
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements to be counted.</param>
        /// <param name="count">The minimum number of elements.</param>
        /// <returns><value>true</value> if <paramref name="source"/> contains at most the specified number of elements; otherwise <value>false</value>.</returns>
        public static bool HasAtMost<T>(this ICollection<T> source, int count)
        {
            return source.Count <= count;
        }

        /// <summary>
        /// Gets a range of items in an IEnumerable.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="startIndex">The index at which to start taking elements.</param>
        /// <param name="endIndex">The index to stop taking elements. The element at this index is not included.</param>
        /// <returns>A subset of source.</returns>
        public static IEnumerable<T> Range<T>(this IEnumerable<T> source, int startIndex, int endIndex)
        {
            return source.Skip(startIndex).Take(endIndex - startIndex);
        }

        /// <summary>
        /// Returns the first element in a sequence, or a default value if the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="defaultValue">The value to return if the sequence is empty.</param>
        /// <returns>The first item in source.</returns>
        public static T FirstOr<T>(this IEnumerable<T> source, T defaultValue)
        {
            foreach (var item in source)
                return item;
            return defaultValue;
        }

        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition, or a default value if no value matches the condition.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="defaultValue">The value to return if the sequence is empty.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The first item in source.</returns>
        public static T FirstOr<T>(this IEnumerable<T> source, T defaultValue, Func<T, bool> predicate)
        {
            foreach (var item in source)
                if (predicate(item))
                    return item;
            return defaultValue;
        }

        /// <summary>
        /// Gets the first index of the item in source that equals the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="value">The value to look for.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            return IndexOf(source, value, 0);
        }

        /// <summary>
        /// Gets the first index of the item in source that matches the given criteria.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The index of the first item in source that match the given criteria.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return IndexOf(source, predicate, 0);
        }

        /// <summary>
        /// Gets the first index of the item in source that equals the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="value">The value to look for.</param>
        /// <param name="startIndex">The index at which to start the search.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T value, int startIndex)
        {
            return IndexOf(source, value, startIndex, -1);
        }

        /// <summary>
        /// Gets the first index of the item in source that matches the given criteria.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="startIndex">The index at which to start the search.</param>
        /// <returns>The index of the first item in source that match the given criteria.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate, int startIndex)
        {
            return IndexOf(source, predicate, startIndex, -1);
        }

        /// <summary>
        /// Gets the first index of the item in source that equals the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="value">The value to look for.</param>
        /// <param name="startIndex">The index at which to start the search.</param>
        /// <param name="count">The number of items to search.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T value, int startIndex, int count)
        {
            return IndexOf(source, t => EqualityComparer<T>.Default.Equals(t, value), startIndex, count);
        }

        /// <summary>
        /// Gets the first index of the item in source that matches the given criteria.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="startIndex">The index at which to start the search.</param>
        /// <param name="count">The number of items to search.</param>
        /// <returns>The index of the first item in source that match the given criteria.</returns>
        public static int IndexOf<T>(this IEnumerable<T> source, Func<T, bool> predicate, int startIndex, int count)
        {
            return IndicesOf(source, predicate, startIndex, count).FirstOr(-1);
        }

        /// <summary>
        /// Gets the indices of all items in source that are equal to the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="value">The value to look for.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> source, T value)
        {
            return IndicesOf(source, value, 0);
        }

        /// <summary>
        /// Gets the indices of all items in source that match the given criteria.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return IndicesOf(source, predicate, 0);
        }

        /// <summary>
        /// Gets the indices of all items in source that are equal to the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="value">The value to look for.</param>
        /// <param name="startIndex">The index at which to start the search.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> source, T value, int startIndex)
        {
            return IndicesOf(source, value, startIndex, -1);
        }

        /// <summary>
        /// Gets the indices of all items in source that match the given criteria.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="startIndex">The index at which to start the search.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> source, Func<T, bool> predicate, int startIndex)
        {
            return IndicesOf(source, predicate, startIndex, -1);
        }

        /// <summary>
        /// Gets the indices of all items in source that are equal to the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="value">The value to look for.</param>
        /// <param name="startIndex">The index at which to start the search.</param>
        /// <param name="count">The number of items to search.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> source, T value, int startIndex, int count)
        {
            return IndicesOf(source, t => EqualityComparer<T>.Default.Equals(t, value), startIndex, count);
        }

        /// <summary>
        /// Gets the indices of all items in source that match the given criteria.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="startIndex">The index at which to start the search.</param>
        /// <param name="count">The number of items to search.</param>
        /// <returns>An enumerable contaning the indices of all items in source that match the given criteria.</returns>
        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> source, Func<T, bool> predicate, int startIndex, int count)
        {
            var itemsToSearch = source;
            if (startIndex > 0)
                itemsToSearch = itemsToSearch.Skip(startIndex);
            if (count >= 0)
                itemsToSearch = itemsToSearch.Take(count);

            int index = startIndex;
            foreach (var item in itemsToSearch)
            {
                if (predicate(item))
                    yield return index;
                ++index;
            }
        }

        /// <summary>
        /// Adds multiple items to the end of a collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence that contains elements.</param>
        /// <param name="items">The items to add.</param>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> items)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (source is List<T>)
                ((List<T>)source).AddRange(items);
            else if (source is TransactableCollection<T>)
                ((TransactableCollection<T>)source).AddRange(items);
            else
                foreach (var item in items)
                    source.Add(item);
        }
    }
}
