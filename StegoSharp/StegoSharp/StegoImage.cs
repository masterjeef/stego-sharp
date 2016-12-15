using System.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using StegoSharp.Extensions;
using Encoder = System.Drawing.Imaging.Encoder;
using System.Text;

namespace StegoSharp
{

    public class StegoImage
    {

        public const int BitsInAByte = 8;
        private readonly string _path;
        private readonly Bitmap _image;

        public StegoImage(string path)
        {
            _path = path;
            _image = new Bitmap(path);
            

            Strategy = Strategy ?? new StegoStrategy();
        }
        
        public StegoStrategy Strategy { get; set; }

        public int Width
        {
            get
            {
                return _image.Width;
            }
        }

        public int Height
        {
            get
            {
                return _image.Height;
            }
        }

        public int TotalPixels
        {
            get
            {
                return Width * Height;
            }
        }

        public double ByteCapacity
        {
            get
            {
                return Strategy.ColorChannels.Length * Pixels.Count(Strategy.PixelSelection) * Strategy.BitsPerChannel / (double)BitsInAByte;
            }
        }

        /// <summary>
        /// Iterates over the pixels from left to right, top to bottom
        /// </summary>
        public IEnumerable<StegoPixel> Pixels 
        {
            get
            {
                for (var i = 0; i < TotalPixels; i++)
                {
                    yield return GetPixel(i);
                }
            }
        }

        /// <summary>
        /// Returns a pixel at the provided index
        /// NOTE: First index is 0 and the last index is at (width * height) - 1
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public StegoPixel GetPixel(int index)
        {
            var x = index % Width;
            var y = index / Width;
            var color = _image.GetPixel(x, y);

            return new StegoPixel
            {
                Index = index,
                X = x,
                Y = y,
                Color = new StegoColor(color)
            };
        }

        #region Reading the payload

        /// <summary>
        /// Reads the raw bytes from the pixels in the image
        /// </summary>
        /// <returns></returns>
        public IEnumerable<byte> ExtractBytes()
        {
            var bitCount = 0;
            var result = 0;

            foreach (var bits in ExtractBits())
            {
                result = result << Strategy.BitsPerChannel;
                result = (result | bits);

                bitCount += Strategy.BitsPerChannel;

                if (bitCount >= BitsInAByte)
                {
                    yield return (byte)result;
                    bitCount = 0;
                    result = 0;
                }
            }
        }

        private IEnumerable<byte> ExtractBits()
        {
            foreach (var pixel in Pixels.Where(Strategy.PixelSelection))
            {
                foreach (var stegoColorChannel in pixel.GetColorChannels(Strategy.ColorChannels))
                {
                    yield return (byte) stegoColorChannel.Value.LowestBits(Strategy.BitsPerChannel);
                }
            }
        }

        #endregion

        #region Embedding the payload

        public StegoImage EmbedPayload(string payload)
        {
            var bytes = Encoding.Default.GetBytes(payload);

            EmbedPayload(bytes);

            return this;
        }

        /// <summary>
        /// Embeds the raw bytes into the images pixels
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public StegoImage EmbedPayload(byte[] payload)
        {
            if (payload.Length > ByteCapacity)
            {
                var message = string.Format("Too much data, only {0} bytes can be embedded.", ByteCapacity);
                throw new ArgumentException(message);
            }

            // pull apart the bytes into the chunks that we can to embed
            var bitIndex = 0;
            var bits = payload.SelectMany(BreakIntoBits).ToArray();

            foreach (var pixel in Pixels.Where(Strategy.PixelSelection))
            {
                foreach (var color in pixel.GetColorChannels(Strategy.ColorChannels))
                {
                    if (bitIndex >= bits.Length)
                    {
                        break;
                    }

                    var result = (int)color.Value;
                    result = ((result >> Strategy.BitsPerChannel) << Strategy.BitsPerChannel) | bits[bitIndex];
                    pixel.Color.SetChannel(color.ColorChannel, (byte)result);
                    bitIndex++;
                }

                _image.SetPixel(pixel.X, pixel.Y, pixel.Color.FromArgb);

                if (bitIndex >= bits.Length)
                {
                    break;
                }
            }

            return this;
        }

        private IEnumerable<byte> BreakIntoBits(byte payloadChunk)
        {
            var bitCount = 0;
            while (bitCount < BitsInAByte)
            {
                var result = (int) payloadChunk;
                result = result >> (BitsInAByte - Strategy.BitsPerChannel - bitCount);
                var bits = (byte)result;
                yield return (byte)bits.LowestBits(Strategy.BitsPerChannel);
                bitCount += Strategy.BitsPerChannel;
            }
        }

        #endregion

        /// <summary>
        /// Saves the image at the specified path
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public StegoImage Save(string filename)
        {
            var encoder = LocateEncoder(_image.RawFormat);

            var encodingParams = new[]
            {
                new EncoderParameter(Encoder.Quality, 100L),
                new EncoderParameter(Encoder.Compression, (long) EncoderValue.CompressionNone)
            };

            var encoderParameters = new EncoderParameters(encodingParams.Length)
            {
                Param = encodingParams
            };

            _image.Save(filename, encoder, encoderParameters);

            return this;
        }

        private ImageCodecInfo LocateEncoder(ImageFormat format)
        {
            foreach (var codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        public bool PixelsAreEqual(StegoImage otherImage, Func<StegoPixel, bool> pixelsToCompare = null)
        {
            if (Width != otherImage.Width || Height != otherImage.Height)
            {
                return false;
            }

            if (pixelsToCompare == null)
            {
                pixelsToCompare = p => true;
            }

            foreach (var pixel in Pixels.Where(pixelsToCompare))
            {
                var otherPixel = otherImage.GetPixel(pixel.Index);

                if (!pixel.ColorsEqual(otherPixel))
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<Tuple<StegoPixel, StegoPixel>> PixelDiff(StegoImage otherImage, Func<StegoPixel, bool> pixelsToCompare = null)
        {
            if (Width != otherImage.Width || Height != otherImage.Height)
            {
                throw new Exception("Images do not share the same dimensions.");
            }

            if (pixelsToCompare == null)
            {
                pixelsToCompare = p => true;
            }

            foreach (var pixel in Pixels.Where(pixelsToCompare))
            {
                var otherPixel = otherImage.GetPixel(pixel.Index);

                if (!pixel.ColorsEqual(otherPixel))
                {
                    yield return Tuple.Create(pixel, otherPixel);
                }
            }
        }
    }
}
