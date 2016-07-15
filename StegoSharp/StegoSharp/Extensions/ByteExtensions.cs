namespace StegoSharp.Extensions
{
    public static class ByteExtensions
    {
        public static int LowestBits(this byte value, int numberOfBits) {
            int mask = (1 << numberOfBits) - 1;
            return value & mask;
        }
    }
}
