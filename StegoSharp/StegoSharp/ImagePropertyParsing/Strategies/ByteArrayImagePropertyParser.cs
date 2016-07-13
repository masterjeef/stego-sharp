using StegoSharp.ImagePropertyParsing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoSharp.ImagePropertyParsing.Strategies
{
    public class ByteArrayImagePropertyParser : IImagePropertyParser
    {

        public string Parse(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }
    }
}
