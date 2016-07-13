using StegoSharp.ImagePropertyParsing.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StegoSharp.ImagePropertyParsing.Strategies
{
    public class ULongArrayImagePropertyParser : IImagePropertyParser
    {

        public string Parse(byte[] bytes)
        {
            var numberOfBytes = 8;
            var remainingBytes = bytes.Length % numberOfBytes;

            if (remainingBytes != 0)
            {
                var bytesToFill = new byte [numberOfBytes - remainingBytes];

                for (var i = 0; i < bytesToFill.Length; i++)
                {
                    bytesToFill[i] = new byte();
                }

                bytes = bytes.Concat(bytesToFill).ToArray();
            }

            var longArray = new List<ulong>();

            for (var i = 0; i < bytes.Length; i += numberOfBytes)
            {
                longArray.Add(BitConverter.ToUInt64(bytes, i));
            }

            return string.Join(", ", longArray);
        }
    }
}
