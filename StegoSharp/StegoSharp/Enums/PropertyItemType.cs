using StegoSharp.Attributes;
namespace StegoSharp.Enums
{
    public enum PropertyItemType : short
    {
        [ParsesTo(typeof(byte[]))]
        ByteArray = 1,

        [ParsesTo(typeof(string))]
        String = 2,

        [ParsesTo(typeof(ushort[]))]
        UnsignedShortArray = 3,

        [ParsesTo(typeof(ulong[]))]
        UnsignedLongArray = 4,

        [ParsesTo(typeof(ulong[]))]
        UnsignedLongFractionArray = 5,

        [ParsesTo(typeof(byte[]))]
        Custom = 6,

        [ParsesTo(typeof(long[]))]
        SignedLongArray = 7,

        [ParsesTo(typeof(long[]))]
        SignedLongFractionArray = 10
    }
}
