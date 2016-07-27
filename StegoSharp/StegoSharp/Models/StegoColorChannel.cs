using StegoSharp.Enums;

namespace StegoSharp.Models
{
    public class StegoColorChannel
    {
        public byte Value { get; set; }

        public ColorChannel ColorChannel {get; set;}

        public StegoColorChannel(ColorChannel colorChannel, byte value)
        {
            Value = value;
            ColorChannel = colorChannel;
        }
    }
}
