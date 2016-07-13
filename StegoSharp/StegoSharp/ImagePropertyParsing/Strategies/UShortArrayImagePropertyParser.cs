using StegoSharp.ImagePropertyParsing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoSharp.ImagePropertyParsing.Strategies
{
    public class UShortArrayImagePropertyParser : IImagePropertyParser
    {

        public string Parse(byte[] bytes)
        {
            var shortArray = new List<ushort>();

            for (var i = 0; i < bytes.Length; i += 2)
            {
                shortArray.Add(BitConverter.ToUInt16(bytes, i));
            }

            return string.Join(", ", shortArray);
        }
    }
}
