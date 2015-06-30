using System;
using System.Text;
using NUnit.Framework;

namespace SharpUtils.Tests
{
    [TestFixture]
    public class ConvertBytesTests
    {
        [Test]
        public void FromHexString()
        {
            var expected = new byte[] { 0x4a, 0x03, 0x57, 0x3d };
            var actual = ConvertBytes.FromHexString("4a03573d");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FromHexString0x()
        {
            var expected = new byte[] { 0xff, 0x04, 0x44, 0xad, 0x67 };
            var actual = ConvertBytes.FromHexString("0xff0444ad67");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void FromHexStringInvalidFormat()
        {
            ConvertBytes.FromHexString("4a03573");
            ConvertBytes.FromHexString("ag");
        }

        [Test]
        public void ToHexString()
        {
            var expected = "08957bccdefa";
            var actual = ConvertBytes.ToHexString(new byte[] { 0x08, 0x95, 0x7b, 0xcc, 0xde, 0xfa });
            Assert.AreEqual(expected, actual);
        }
    }
}
