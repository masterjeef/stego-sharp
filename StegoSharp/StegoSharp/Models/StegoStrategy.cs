using StegoSharp.Enums;

namespace StegoSharp.Models
{
    public class StegoStrategy
    {
        public int NumberOfBits { get; set; }

        public ColorChannel [] ColorChannels { get; set; }
    }
}
