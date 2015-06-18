using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// Contains helper methods for byte array conversions.
    /// </summary>
    public static class ConvertBytes
    {
        /// <summary>
        /// Creates a byte array from a string in hexadecimal format.
        /// </summary>
        /// <param name="s">A string in hexadecimal format.</param>
        /// <returns>A byte array.</returns>
        /// <exception cref="ArgumentException" />
        /// <exception cref="FormatException" />
        /// <exception cref="OverflowException" />
        public static byte[] FromHexString(string s)
        {
            if (s.Length % 2 == 1)
                throw new FormatException("Invalid length for a hex string.");
            int i = 0;
            int length = s.Length;
            if (s.StartsWith("0x"))
            {
                i = 2;
                length -= 2;
            }
            byte[] bytes = new byte[length / 2];
            for (int j = 0; j < bytes.Length; i += 2, j++)
                bytes[j] = Convert.ToByte(s.Substring(i, 2), 16);
            return bytes;
        }

        /// <summary>
        /// Creates a string that represents the byte array in hexadecimal format.
        /// </summary>
        /// <param name="bytes">The byte array to convert to a string.</param>
        /// <returns>A string in hexadecimal format.</returns>
        /// <exception cref="ArgumentNullException" />
        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.Append(b.ToString("x2"));
            return hex.ToString();
        }
    }
}
