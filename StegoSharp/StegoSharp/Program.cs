using StegoSharp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StegoSharp.Enums;
using System.IO;

namespace StegoSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var p = @"pillars.png";
            var i = new StegoImage(p);
            i.Strategy.BitsPerChannel = 8;
            i.Strategy.PixelSelection = x => x.Index % 3 == 0;
            
            i.EmbedPayload(File.ReadAllBytes("iguana-embedded.png"));
            i.Save(@"test2.png");
            
        }
    }
}
