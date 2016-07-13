using StegoSharp.Enums;
using StegoSharp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            var path = @"acid_burn.jpg";
            //var path = @"1984-1.png";
            //var path2 = @"1984-2.png";

            var image = new StegoImage(path);
            //var image2 = new StegoImage(path2);

            int numberOfBits = 2;
            var extracted1 = image.ExtractBytes(numberOfBits);
            var extracted2 = image.ExtractBytes2(numberOfBits).ToArray();

            //foreach (var bits in extracted1)
            //{
            //    Console.Write((char)bits);
            //}

            Console.WriteLine("\n\n\n" + Encoding.Default.GetString(extracted2));

            //Console.WriteLine(image.ToString());

            //var image = new Bitmap(path);

            //for (var i = 0; i < image.PropertyItems.Length; i++)
            //{
            //    var propertyItem = image.PropertyItems[i];

            //    Console.WriteLine("PROPERTY: {0}", i);
            //    Console.WriteLine("ID: 0x{0}", propertyItem.Id);
            //    Console.WriteLine("Type: {0}", (PropertyItemType) propertyItem.Type);
            //    Console.WriteLine("Length: {0}", propertyItem.Len);
            //    Console.WriteLine("Value: {0}", System.Text.Encoding.Default.GetString(propertyItem.Value));
            //    Console.WriteLine();
            //}
        }
    }
}
