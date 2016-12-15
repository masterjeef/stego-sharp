using System;

namespace StegoSharp.Extensions
{
    public static class ByteExtensions
    {
        public static int LowestBits(this byte value, int numberOfBits) {
            var mask = (1 << numberOfBits) - 1;
            return value & mask;
        }

        public static string ToBinaryString(this byte value)
        {
            var result = "";
            var theByte = value;

            for (var power = 7; power >= 0; power--)
            {
                var raised = (byte) Math.Pow(2, power);
                if (theByte - raised >= 0)
                {
                    theByte -= raised;
                    result += "1";
                } 
                else
                {
                    result += "0";
                }
            }

            return result;
        }
    }
}
