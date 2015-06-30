using System.Collections.Generic;
using System.Linq;

namespace Sharp.Utils
{
    /// <summary>
    /// Contains methods for combining hash codes.
    /// </summary>
    public static class HashCode
    {
        /// <summary>
        /// Combines the hash codes of the objects using the FNV algorithm.
        /// </summary>
        /// <param name="objects">The objects for which to combine the hash codes.</param>
        /// <returns>The combined hash code.</returns>
        public static int FnvCombine(params object[] objects)
        {
            return FnvCombine(objects.Select(o => o != null ? o.GetHashCode() : int.MinValue));
        }

        /// <summary>
        /// Combines hashes using the FNV algorithm.
        /// </summary>
        /// <param name="hashes">The hashes to combine.</param>
        /// <returns>The combined hash code.</returns>
        public static int FnvCombine(params int[] hashes)
        {
            return FnvCombine((IEnumerable<int>)hashes);
        }

        /// <summary>
        /// Combines hashes using the FNV algorithm.
        /// </summary>
        /// <param name="hashes">The hashes to combine.</param>
        /// <returns>The combined hash code.</returns>
        public static int FnvCombine(IEnumerable<int> hashes)
        {
            unchecked // Overflow is ok
            {
                uint h = 2166136261;
                foreach (var hash in hashes)
                {
                    h = (h * 16777619) ^ (uint)hash;
                }
                return (int)h;
            }
        }

        /// <summary>
        /// Gets a hash code for the specified byte array using the FNV algorithm.
        /// </summary>
        /// <param name="bytes">A byte array.</param>
        /// <returns>The combined hash code.</returns>
        public static int FnvCombine(byte[] bytes)
        {
            unchecked // Overflow is ok
            {
                uint h = 2166136261;
                foreach (var b in bytes)
                {
                    h = (h * 16777619) ^ (uint)b;
                }
                return (int)h;
            }
        }
    }
}
