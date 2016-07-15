using StegoSharp.Attributes;
namespace StegoSharp.Enums
{
    public enum PropertyItemType : short
    {
        ByteArray = 1,
        String = 2,
        UnsignedShortArray = 3,
        UnsignedLongArray = 4,
        UnsignedLongFractionArray = 5,
        Custom = 6,
        SignedLongArray = 7,
        SignedLongFractionArray = 10
    }
}
