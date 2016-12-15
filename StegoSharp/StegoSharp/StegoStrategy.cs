using System;
using System.Linq;
using StegoSharp.Enums;

namespace StegoSharp
{
    public class StegoStrategy
    {
        private const string DuplicateChannelMessage = "Duplicate color channels found.";
        private ColorChannel[] _colorChannels;
        private int _bitsPerChannel;

        public StegoStrategy()
        {
            BitsPerChannel = 1;
            ColorChannels = new [] {ColorChannel.R, ColorChannel.G, ColorChannel.B};
            PixelSelection = p => true;
        }

        public int BitsPerChannel
        {
            get { return _bitsPerChannel; }
            set
            {
                if (StegoImage.BitsInAByte % value != 0 || value > StegoImage.BitsInAByte)
                {
                    throw new Exception("The number of bits must be less than 9 and must be a multiple of 8");
                }

                _bitsPerChannel = value;
            }
        }

        public ColorChannel[] ColorChannels
        {
            get
            {
                return _colorChannels;
            }
            set
            {
                if (value.Distinct().Count() != value.Length)
                {
                    throw new ArgumentException(DuplicateChannelMessage);
                }

                _colorChannels = value;
            }
        }

        public Func<StegoPixel, bool> PixelSelection { get; set; }
    }
}
