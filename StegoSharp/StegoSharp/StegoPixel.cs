using System;
using System.Collections.Generic;
using StegoSharp.Enums;

namespace StegoSharp
{
    public class StegoPixel
    {
        private const string NoColorChannelMessage = "No color channels specified.";
        
        public int Index { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public StegoColor Color { get; set; }

        public IEnumerable<StegoColorChannel> GetColorChannels()
        {
            return GetColorChannels(ColorChannel.R, ColorChannel.G, ColorChannel.B);
        }

        public IEnumerable<StegoColorChannel> GetColorChannels(params ColorChannel [] channels)
        {
            if (channels.Length == 0)
            {
                throw new ArgumentOutOfRangeException(NoColorChannelMessage);
            }

            foreach (var colorChannel in channels)
            {
                yield return Color.GetChannel(colorChannel);
            }
        }

        public bool ColorsEqual(StegoPixel otherPixel)
        {
            return Color.FromArgb == otherPixel.Color.FromArgb;
        }
    }
}
