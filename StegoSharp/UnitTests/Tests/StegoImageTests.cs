using StegoSharp;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests.Tests
{
    public class StegoImageTests
    {
        [Fact]
        public void EmbedBytes_should_embed_a_byte_array_into_the_image()
        {
            var path = @"images/iguana.png";
            var resultPath = @"iguana-embedded.png";
            var message = "We exist without skin color," +
                          "without nationality, without religious bias... and you call us criminals." +
                          "You build atomic bombs, you wage wars, you murder, cheat, and lie to us" +
                          "and try to make us believe it's for our own good, yet we're the criminals.";

            using (var image = new StegoImage(path))
            {
                image.EmbedPayload(message)
                    .Save(resultPath);
            }

            using (var imageWithMessage = new StegoImage(resultPath))
            {
                var embeddedString = Encoding.Default.GetString(imageWithMessage.ExtractBytes().ToArray());
                var embedded = embeddedString.Substring(0, message.Length);

                Assert.Equal(message, embedded);
            }
        }

        [Fact]
        public void EmbeddedImages_should_work()
        {
            var message = "The probability of success is difficult to estimate; but if we never search the chance of success is zero.";

            var slothPath = @"images/sloth.png";
            var slothEmbeddedPath = @"sloth-embedded.png";
            using (var slothImage = new StegoImage(slothPath))
            {
                slothImage.Strategy.PixelSelection = p => p.Index % 2 == 0;
                slothImage.EmbedPayload(Encoding.Default.GetBytes(message));

                slothImage.Save(slothEmbeddedPath);
            }

            var slothBytes = File.ReadAllBytes(slothEmbeddedPath);

            // embed the sloth into the astronaut
            var astronautPath = @"images/astronaut.png";
            var embeddedAstronautPath = @"astronaut-embedded.png";
            using (var astronautImage = new StegoImage(astronautPath))
            {
                astronautImage.EmbedPayload(slothBytes);

                // save the astronaut
                astronautImage.Save(embeddedAstronautPath);
            }

            var astronautBytes = File.ReadAllBytes(embeddedAstronautPath);

            // embed the astronaut into space
            var spacePath = @"images/space.png";
            var spaceEmbeddedPath = @"space-embedded.png";
            using (var spaceImage = new StegoImage(spacePath))
            {
                spaceImage.EmbedPayload(astronautBytes);

                // save space
                spaceImage.Save(spaceEmbeddedPath);
            }

            // extract the astronaut
            using (var spaceEmbeddedImage = new StegoImage(spaceEmbeddedPath))
            {
                var embeddedAstronautBytes = spaceEmbeddedImage.ExtractBytes()
                    .Take(astronautBytes.Length)
                    .ToArray();

                Assert.Equal(astronautBytes, embeddedAstronautBytes);

                var astronautBytesPath = @"astronaut-embedded-bytes.png";
                File.WriteAllBytes(astronautBytesPath, embeddedAstronautBytes);

                using (var astronautBytesImage = new StegoImage(astronautBytesPath))
                {
                    var embeddedSlothBytes = astronautBytesImage.ExtractBytes().Take(slothBytes.Length).ToArray();

                    Assert.Equal(slothBytes, embeddedSlothBytes);

                    var slothBytesPath = @"sloth-embedded-bytes.png";
                    File.WriteAllBytes(slothBytesPath, embeddedSlothBytes);

                    using (var slothBytesImage = new StegoImage(slothBytesPath))
                    {
                        slothBytesImage.Strategy.PixelSelection = p => p.Index % 2 == 0;
                        var embeddedMessage = Encoding.Default.GetString(slothBytesImage.ExtractBytes().ToArray())
                            .Substring(0, message.Length);

                        Assert.Equal(message, embeddedMessage);
                    }
                }
            }
        }
    }
}
