using System;
using StegoSharp.Models;
using StegoSharp.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using StegoSharp.Enums;
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
            image.Save(resultPath);
            
            var imageWithMessage = new StegoImage(resultPath);
            var embeddedString = Encoding.Default.GetString(imageWithMessage.ExtractBytes().ToArray());
            var embedded = embeddedString.Substring(0, message.Length);

            Assert.Equal(message, embedded);
        }

        [Fact]
        public void EmbeddedImages_should_work()
        {
            var message = "The probability of success is difficult to estimate; but if we never search the chance of success is zero.";

            var slothPath = @"images/sloth.png";
            var slothImage = new StegoImage(slothPath);
            slothImage.Strategy.PixelSelection = p => p.Index % 2 == 0;
            slothImage.EmbedData(Encoding.Default.GetBytes(message));

            var slothEmbeddedPath = @"sloth-embedded.png";
            slothImage.Save(slothEmbeddedPath);
            var slothBytes = File.ReadAllBytes(slothEmbeddedPath);

            // embed the sloth into the astronaut
            var astronautPath = @"images/astronaut.png";
            var astronautImage = new StegoImage(astronautPath);
            astronautImage.EmbedData(slothBytes);

            // save the astronaut
            var embeddedAstronautPath = @"astronaut-embedded.png";
            astronautImage.Save(embeddedAstronautPath);
            var astronautBytes = File.ReadAllBytes(embeddedAstronautPath);

            // embed the astronaut into space
            var spacePath = @"images/space.png";
            var spaceImage = new StegoImage(spacePath);
            spaceImage.EmbedData(astronautBytes);

            // save space
            var spaceEmbeddedPath = @"space-embedded.png";
            spaceImage.Save(spaceEmbeddedPath);

            // extract the astronaut
            var spaceEmbeddedImage = new StegoImage(spaceEmbeddedPath);
            var embeddedAstronautBytes = spaceEmbeddedImage.ExtractBytes()
                .Take(astronautBytes.Length)
                .ToArray();

            Assert.Equal(astronautBytes, embeddedAstronautBytes);

            var astronautBytesPath = @"astronaut-embedded-bytes.png";
            File.WriteAllBytes(astronautBytesPath, embeddedAstronautBytes);

            var astronautBytesImage = new StegoImage(astronautBytesPath);
            var embeddedSlothBytes = astronautBytesImage.ExtractBytes().Take(slothBytes.Length).ToArray();

            Assert.Equal(slothBytes, embeddedSlothBytes);

            var slothBytesPath = @"sloth-embedded-bytes.png";
            File.WriteAllBytes(slothBytesPath, embeddedSlothBytes);

            var slothBytesImage = new StegoImage(slothBytesPath);
            slothBytesImage.Strategy.PixelSelection = p => p.Index % 2 == 0;
            var embeddedMessage = Encoding.Default.GetString(slothBytesImage.ExtractBytes().ToArray())
                .Substring(0, message.Length);

            Assert.Equal(message, embeddedMessage);
        }
    }
}
