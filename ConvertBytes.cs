using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class ConvertBytes
    {
        public static byte[] FromHexString(string s)
        {
            if (s.Length % 2 == 1)
                throw new FormatException("Invalid length for a hex string.");
            byte[] bytes = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                bytes[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            return bytes;
        }

        public static string ToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static int GetHashCode(byte[] bytes)
        {
            return HashCode.FnvCombine(bytes);
        }
    }
}
