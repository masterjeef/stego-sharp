using StegoSharp.Enums;
using StegoSharp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


            
            //Console.WriteLine(image.ToString());

            var path1 = @"acid_burn.jpg";
            path2 = @"acid_burn_hackers_desktop.jpg";

            var data1 = File.ReadAllBytes(path1);
            var data2 = File.ReadAllBytes(path2);

            var unknownBytes = new List<byte>();
            for (int i = 0; i < data1.Length; i++)
            {
                if (i >= data2.Length || data1[i] != data2[i])
                {
                    //throw new Exception("bytes not equal");
                    unknownBytes.Add(data1[i]);
                }
            }

            var unknown = unknownBytes.ToArray();
            var message = Encoding.Default.GetString(unknown);
            Console.WriteLine(message);
            var reverse = string.Join("", message.Reverse());
            Console.WriteLine(reverse);

        }
    }
}
