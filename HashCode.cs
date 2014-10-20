using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class HashCode
    {
        /// <summary>
        /// Combines hashes using the FNV algorithm.
        /// </summary>
        /// <param name="hashes">The hashes to combine.</param>
        /// <returns>The combined hash code.</returns>
        public static int FnvCombine(params int[] hashes)
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
        /// Combines hashes using the ELF algorithm.
        /// </summary>
        /// <param name="hashes">The hashes to combine.</param>
        /// <returns>The combined hash code.</returns>
        /// <remarks>
        /// NOTE: this implementation has not been tested.
        /// </remarks>
        public static int ElfCombine(params int[] hashes)
        {
            unchecked // Overflow is ok
            {
                uint h = 0, g;
                foreach (var hash in hashes)
                {
                    h = (h << 4) + (uint)hash;
                    g = h & (uint)0xf000000L;
                    if (g != 0)
                        h ^= g >> 24;
                    h &= ~g;
                }
                return (int)h;
            }
        }

        /// <summary>
        /// Gets a hash code for the specified bytes using the FNV algorithmpublic
        /// </summary>
        /// <param name="b">A byte array.</param>
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
