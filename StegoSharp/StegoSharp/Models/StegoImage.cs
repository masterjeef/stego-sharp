namespace StegoSharp.Models
{

    using StegoSharp.ImagePropertyParsing;
    using StegoSharp.Extensions;

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    
    public class StegoImage
    {
        private const int BitsInAByte = 8;
        private readonly StegoImagePropertyParser _parser = new StegoImagePropertyParser();

        private readonly string _path;
        private Bitmap _image;

        public StegoImageProperty [] ImageProperties { get; private set; }

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

        public IEnumerable<Color> GetPixels()
        {
            var totalPixels = _image.Width * _image.Height;

            for (var i = 0; i < totalPixels; i++)
            {
                int x = i % _image.Width;
                int y = i / _image.Width;

                yield return _image.GetPixel(x, y);
            }
        }

        public IEnumerable<byte> ExtractBits(int numberOfBits = 2)
        {
            if(numberOfBits < 1 || numberOfBits > BitsInAByte) {
                throw new ArgumentOutOfRangeException();
            }

            foreach (var pixel in GetPixels())
            {
                var red = pixel.R;
                var green = pixel.G;
                var blue = pixel.B;
                var alpha = pixel.A;

                if (alpha < 255)
                {
                    throw new Exception("Alpha is less than 255. The alpha channel could be holding information.");
                }

                yield return (byte) red.LowestBits(numberOfBits);
                yield return (byte) green.LowestBits(numberOfBits);
                yield return (byte) blue.LowestBits(numberOfBits);
            }
        }

        public byte[] ExtractBytes(int numberOfBits = 2)
        {
            if (numberOfBits < 1 || numberOfBits > BitsInAByte)
            {
                throw new ArgumentOutOfRangeException();
            }

            var bytes = new List<byte>();

            foreach (var pixel in GetPixels())
            {

                var red = pixel.R;
                var green = pixel.G;
                var blue = pixel.B;
                var alpha = pixel.A;

                if (alpha < 255)
                {
                    throw new Exception("Alpha is less than 255. The alpha channel could be holding information.");
                }

                int bits = 0;
                bits = (bits | red.LowestBits(numberOfBits)) << numberOfBits;
                bits = (bits | green.LowestBits(numberOfBits)) << numberOfBits;
                bits = bits | blue.LowestBits(numberOfBits);

                byte result = (byte) bits;
                bytes.Add(result);
            }

            return bytes.ToArray();
        }

        public IEnumerable<byte> ExtractBytes2(int numberOfBits = 2)
        {
            if (BitsInAByte % numberOfBits != 0 || numberOfBits > BitsInAByte)
            {
                throw new Exception("The number of bits must be less than 9 and must be a multiple of 8");
            }

            var bitCount = 0;
            int result = 0;

            foreach (var bits in ExtractBits(numberOfBits))
            {
                if (bitCount >= BitsInAByte)
                {
                    yield return (byte) result;
                    bitCount = 0;
                    result = 0;
                    continue;
                }

                result = result << numberOfBits;
                result = (result | bits);
                bitCount += numberOfBits;
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
