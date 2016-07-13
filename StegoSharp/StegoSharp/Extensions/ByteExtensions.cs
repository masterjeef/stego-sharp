namespace StegoSharp.Extensions
{
    public static class ByteExtensions
    {
        public static int ExtractLastBits(this byte value, int numberOfBits) {
            int mask = (1 << numberOfBits) - 1;
            var lastXbits = value & mask;
            return lastXbits;
        }
    }
}
