using StegoSharp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StegoSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var path = @"1984-1.png";
            var path2 = @"1984-2.png";

            var image1 = new StegoImage(path);
            var image2 = new StegoImage(path2);

            var pixelDiff = image1.PixelDifference(image2).Select(p => p.Item1.Index).ToArray();

            int numberOfBits = 1;
            //var extracted1 = image1.ExtractBytes(numberOfBits);
            var extracted = image1.ExtractBytes(numberOfBits).ToArray();

            //foreach (var b in extracted)
            //{
            //    var character = (char)b;
            //    Console.Write(character);
            //}

            var result = Encoding.Default.GetString(extracted);
            var r2 = Encoding.UTF8.GetString(extracted);
            var r3 = Encoding.UTF7.GetString(extracted);
            var r4 = Encoding.UTF32.GetString(extracted);
            var r5 = Encoding.Unicode.GetString(extracted);
            var r6 = Encoding.BigEndianUnicode.GetString(extracted);
            var r7 = Encoding.ASCII.GetString(extracted);
            Console.WriteLine(result);
        }
    }
}
