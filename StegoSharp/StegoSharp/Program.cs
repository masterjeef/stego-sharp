using StegoSharp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StegoSharp.Enums;

namespace StegoSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = @"1984-1.png";
            var path2 = @"1984-2.png";

            var image1 = new StegoImage(path);
            var image2 = new StegoImage(path2);

            var pixelDiff = image1.PixelDifference(image2).Select(p => p.Item1.Index).ToArray();

            var strategies = new[]
            {
                new StegoStrategy
                {
                    BitsPerChannel = 1,
                    ColorChannels = new []{ColorChannel.R, ColorChannel.G, ColorChannel.B},
                },
                new StegoStrategy
                {
                    BitsPerChannel = 1,
                    ColorChannels = new []{ColorChannel.B, ColorChannel.G, ColorChannel.R},
                },
                new StegoStrategy
                {
                    BitsPerChannel = 2,
                    ColorChannels = new []{ColorChannel.R, ColorChannel.G, ColorChannel.B},
                },
                new StegoStrategy
                {
                    BitsPerChannel = 2,
                    ColorChannels = new []{ColorChannel.B, ColorChannel.G, ColorChannel.R},
                },
            };

            var results = new List<string>();

            foreach (var stegoStrategy in strategies)
            {
                image1.Strategy = stegoStrategy;

                var extracted = image1.ExtractBytes().ToArray();

                results.Add(Encoding.Default.GetString(extracted));
                results.Add(Encoding.UTF8.GetString(extracted));
                results.Add(Encoding.UTF7.GetString(extracted));
                results.Add(Encoding.UTF32.GetString(extracted));
                results.Add(Encoding.Unicode.GetString(extracted));
                results.Add(Encoding.BigEndianUnicode.GetString(extracted));
                results.Add(Encoding.ASCII.GetString(extracted));
            }

            var finalResults = results.ToArray();
        }
    }
}
