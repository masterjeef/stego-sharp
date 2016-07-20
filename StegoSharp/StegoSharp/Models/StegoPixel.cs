using System.Drawing;
namespace StegoSharp.Models
{
    public class StegoPixel
    {
        public int Index { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Color Color { get; set; }

        public bool ColorsEqual(StegoPixel otherPixel)
        {
            bool areEqual = true;
            areEqual &= Color.R == otherPixel.Color.R;
            areEqual &= Color.G == otherPixel.Color.G;
            areEqual &= Color.B == otherPixel.Color.B;
            areEqual &= Color.A == otherPixel.Color.A;
            return areEqual;
        }
    }
}
