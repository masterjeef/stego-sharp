using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using StegoSharp.Extensions;

namespace UnitTests
{
    public class ExtensionsTests
    {
        [Fact]
        public static void LowestBits_should_return_the_lowest_bits()
        {
            byte testByte = 255;

            Assert.Equal(1, testByte.LowestBits(1));
            Assert.Equal(3, testByte.LowestBits(2));
            Assert.Equal(7, testByte.LowestBits(3));
            Assert.Equal(15, testByte.LowestBits(4));

            testByte = 5;

            Assert.Equal(1, testByte.LowestBits(1));
            Assert.Equal(1, testByte.LowestBits(2));
            Assert.Equal(5, testByte.LowestBits(3));
        }

        [Fact]
        public static void ToBinaryString_should_return_a_binary_representation_of_a_byte()
        {
            Assert.Equal("00000101", ((byte)5).ToBinaryString());
            Assert.Equal("11111111", ((byte)255).ToBinaryString());
            Assert.Equal("00000000", ((byte)0).ToBinaryString());
        }
    }
}
