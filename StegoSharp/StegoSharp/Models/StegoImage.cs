namespace StegoSharp.Models
{

    using ImagePropertyParsing;
    using Extensions;

    using System;
    using System.Collections.Generic;
    using System.Drawing;

    public class StegoImage
    {
        private const int BitsInAByte = 8;
        private readonly StegoImagePropertyParser _parser = new StegoImagePropertyParser();
        private readonly string _path;
        private readonly Bitmap _image;

        public StegoImage(string path)
        {
            _path = path;
            _image = new Bitmap(path);

            ImageProperties = new StegoImageProperty[_image.PropertyItems.Length];
            for (var i = 0; i < ImageProperties.Length; i++)
            {
                ImageProperties[i] = new StegoImageProperty(_image.PropertyItems[i]);
            }
        }

        public StegoImageProperty[] ImageProperties { get; private set; }

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

        public StegoPixel GetPixel(int index)
        {
            int x = index % Width;
            int y = index / Width;

            return new StegoPixel
            {
                Index = index,
                X = x,
                Y = y,
                Color = _image.GetPixel(x, y)
            };
        }

        public IEnumerable<byte> ExtractBits(int numberOfBits = 2)
        {
            if(numberOfBits < 1 || numberOfBits > BitsInAByte) {
                throw new ArgumentOutOfRangeException();
            }

            foreach (var pixel in Pixels)
            {
                var red = pixel.Color.R;
                var green = pixel.Color.G;
                var blue = pixel.Color.B;
                var alpha = pixel.Color.A;

                if (alpha < 255)
                {
                    throw new Exception("Alpha is less than 255. The alpha channel could be holding information.");
                }

                yield return (byte) red.LowestBits(numberOfBits);
                yield return (byte) green.LowestBits(numberOfBits);
                yield return (byte) blue.LowestBits(numberOfBits);
            }
        }

        public IEnumerable<byte> ExtractBytes(int numberOfBits = 2)
        {
            if (BitsInAByte % numberOfBits != 0 || numberOfBits > BitsInAByte)
            {
                throw new Exception("The number of bits must be less than 9 and must be a multiple of 8");
            }

            var bitCount = 0;
            var result = 0;

            foreach (var bits in ExtractBits(numberOfBits))
            {
                //var before = ((byte)result).ToBinaryString();
                result = result << numberOfBits;
                result = (result | bits);
                //var after = ((byte)result).ToBinaryString();

                bitCount += numberOfBits;

                if (bitCount >= BitsInAByte)
                {
                    yield return (byte) result;
                    bitCount = 0;
                    result = 0;
                }
            }

            // TODO: Figure out how to handle overflow (commented out below)
            //if (bitCount != 0)
            //{
            //    yield return (byte)result;
            //}
        }

        public bool PixelsAreEqual(StegoImage otherImage)
        {
            if (Width != otherImage.Width || Height != otherImage.Height)
            {
                return false;
            }

            foreach (var pixel in Pixels)
            {
                var otherPixel = otherImage.GetPixel(pixel.Index);

                if (!pixel.ColorsEqual(otherPixel))
                {
                    return false;
                }
            }

            return true;
        }

        public bool EmbedData(byte[] data, int numberOfBits)
        {
            var capacity = ByteCapacity(numberOfBits);
            if (data.Length > capacity)
            {
                var message = string.Format("Too much data, only {0} bytes can be embedded.", capacity);
                throw new Exception(message);
            }

            // pull apart the bytes into the chunks we want to embed
            // embed these chunks into each color of each pixel
            throw new NotImplementedException();
        }

        public double ByteCapacity(int numberOfBits) {
            return (3 * TotalPixels * numberOfBits) / 8.0;
        }

        public IEnumerable<Tuple<StegoPixel, StegoPixel>> PixelDifference(StegoImage otherImage)
        {
            if (Width != otherImage.Width || Height != otherImage.Height)
            {
                throw new Exception("Images do not share the same dimensions.");
            }

            foreach (var pixel in Pixels)
            {
                var otherPixel = otherImage.GetPixel(pixel.Index);

                if (!pixel.ColorsEqual(otherPixel))
                {
                    yield return Tuple.Create(pixel, otherPixel);
                }
            }
        }

        public override string ToString()
        {
            var result = string.Format("Image '{0}'\n", _path);

            for (var i = 0; i < ImageProperties.Length; i++)
            {
                var imageProperty = ImageProperties[i];
                result += string.Format("Property {0} - 0x{3} [{1}]\nResult : {2}\n", i, imageProperty.PropertyItemType, _parser.Parse(imageProperty), imageProperty.PropertyItem.Id);
            }

            return result + "\n";
        }
    }
}
