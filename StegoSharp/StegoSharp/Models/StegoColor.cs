using System;
using System.Collections.Generic;
using System.Drawing;
using StegoSharp.Enums;

namespace StegoSharp.Models
{
    public class StegoColor
    {
        private readonly Dictionary<ColorChannel, StegoColorChannel> _colorChannels = new Dictionary<ColorChannel, StegoColorChannel>();

        public StegoColor() { }

        public StegoColor(Color color)
        {
            SetChannel(ColorChannel.R, color.R);
            SetChannel(ColorChannel.G, color.G);
            SetChannel(ColorChannel.B, color.B);
            SetChannel(ColorChannel.A, color.A);
        }

        public StegoColorChannel R
        {
            get { return _colorChannels[ColorChannel.R]; }
        }

        public StegoColorChannel G
        {
            get { return _colorChannels[ColorChannel.G]; }
        }

        public StegoColorChannel B
        {
            get { return _colorChannels[ColorChannel.B]; }
        }

        public StegoColorChannel A
        {
            get { return _colorChannels[ColorChannel.A]; }
        }

        public Color Color
        {
            get
            {
                if (R == null || G == null || B == null)
                {
                    const string message = "Missing a required color channel. Red, Green, and Blue color channels are required.";
                    throw new ArgumentNullException(message);
                }

                var alpha = A ?? new StegoColorChannel(ColorChannel.A, 255);

                return Color.FromArgb(alpha.Value, R.Value, G.Value, B.Value);
            }
        }

        public void SetChannel(ColorChannel colorChannel, byte colorValue)
        {
            var stegoColorChannel = new StegoColorChannel(colorChannel, colorValue);
            SetChannel(stegoColorChannel);
        }

        public void SetChannel(StegoColorChannel colorChannel)
        {
            if (!_colorChannels.ContainsKey(colorChannel.ColorChannel))
            {
                _colorChannels.Add(colorChannel.ColorChannel, colorChannel);
            }
            else
            {
                _colorChannels[colorChannel.ColorChannel] = colorChannel;
            }
        }

        public StegoColorChannel GetChannel(ColorChannel channel)
        {
            if (!_colorChannels.ContainsKey(channel))
            {
                var message = string.Format("Missing color channel [{0}].", channel);
                throw new ArgumentNullException(message);
            }

            return _colorChannels[channel];
        }
    }
}
