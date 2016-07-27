using System;
using System.Collections.Generic;
using System.Linq;
using StegoSharp.Enums;

namespace StegoSharp.Models
{
    public class StegoPixel
    {
        private const string NoColorChannelMessage = "No color channels specified.";
        private const string DuplicateChannelMessage = "Duplicate color channels found.";

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

            if (channels.Distinct().Count() != channels.Length)
            {
                throw new ArgumentException(DuplicateChannelMessage);
            }

            foreach (var colorChannel in channels)
            {
                yield return Color.GetChannel(colorChannel);
            }
        }

        public bool ColorsEqual(StegoPixel otherPixel)
        {
            var areEqual = true;
            areEqual &= Color.R == otherPixel.Color.R;
            areEqual &= Color.G == otherPixel.Color.G;
            areEqual &= Color.B == otherPixel.Color.B;
            areEqual &= Color.A == otherPixel.Color.A;
            return areEqual;
        }
    }
}
