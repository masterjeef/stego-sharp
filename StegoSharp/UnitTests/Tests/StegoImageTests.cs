using System;
using StegoSharp.Models;
using StegoSharp.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests.Tests
{
    public class StegoImageTests
    {
        
        [Fact]
        public void ExtractBytes_should_extract_bytes_from_the_lowest_bits()
        {
            var path = @"images/test-image.jpg";

            var image = new StegoImage(path);

            var bytes = new List<byte>();
            
            foreach (var pixel in image.Pixels)
            {
                bytes.Add(pixel.Color.R.Value);
                bytes.Add(pixel.Color.G.Value);
                bytes.Add(pixel.Color.B.Value);
            }

            var result = new List<byte>();

            var numberOfBits = 2;
            var workingByte = 0;
            var bitCount = 0;
            foreach (var b in bytes)
            {
                workingByte = workingByte << numberOfBits;
                workingByte = workingByte | b.LowestBits(numberOfBits);
                bitCount+=numberOfBits;

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
            image.Strategy.BitsPerChannel = numberOfBits;
            var otherResult = image.ExtractBytes().ToArray();

            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i], otherResult[i]);
            }
        }

        [Fact]
        public void EmbedBytes_should_embed_a_byte_array_into_the_image()
        {
            var path = @"images/iguana.png";

            var image = new StegoImage(path);

            var message = Guid.NewGuid().ToString();

            image.EmbedData(Encoding.Default.GetBytes(message));

            var resultPath = @"iguana-embedded.png";

            if (File.Exists(resultPath))
            {
                File.Delete(resultPath);
            }

            image.Save(resultPath);
            
            var imageWithMessage = new StegoImage(resultPath);
            var embeddedMessage = Encoding.Default.GetString(imageWithMessage.ExtractBytes().ToArray());

            Assert.True(embeddedMessage.Contains(message));
            
        }
    }
}
