using StegoSharp.Models;
using StegoSharp.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace UnitTests.Tests
{
    public class StegoImageTests
    {
        
        [Fact]
        public void ExtractBytes_should_extract_bytes_from_the_lowest_bits()
        {
            var path = @"test-image.jpg";

            var image = new StegoImage(path);

            var bytes = new List<byte>();
            
            foreach (var pixel in image.Pixels)
            {
                bytes.Add(pixel.Color.R);
                bytes.Add(pixel.Color.G);
                bytes.Add(pixel.Color.B);
            }

            var result = new List<byte>();

            var workingByte = 0;
            var bitCount = 0;
            foreach (var b in bytes)
            {
                workingByte = workingByte << 1;
                workingByte = workingByte | b.LowestBits(1);
                bitCount++;

                if (bitCount == 8)
                {
                    result.Add((byte)workingByte);
                    workingByte = 0;
                    bitCount = 0;
                }
            }

            //if (bitCount > 0)
            //{
            //    result.Add((byte)workingByte);
            //}

            var otherResult = image.ExtractBytes(1).ToArray();

            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i], otherResult[i]);
            }
        }
    }
}
