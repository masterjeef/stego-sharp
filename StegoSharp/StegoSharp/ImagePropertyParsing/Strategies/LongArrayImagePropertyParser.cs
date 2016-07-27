using StegoSharp.ImagePropertyParsing.Interfaces;
using System.Text;

namespace StegoSharp.ImagePropertyParsing.Strategies
{
    public class LongArrayImagePropertyParser : IImagePropertyParser
    {

        public string Parse(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }
    }
}
