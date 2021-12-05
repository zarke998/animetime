using System;

namespace AnimeTime.Utilities.Colors
{
    internal readonly struct RgbaColor
    {
        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public byte A { get; }

        public RgbaColor(int r, int g, int b) : this(r, g, b, 255) { }

        public RgbaColor(int r, int g, int b, int a)
        {
            if (r < 0 || r > 255)
                throw new ArgumentException("Value of red must be between 0 and 255.");
            if (g < 0 || g > 255)
                throw new ArgumentException("Value of green must be between 0 and 255.");
            if (b < 0 || b > 255)
                throw new ArgumentException("Value of blue must be between 0 and 255.");
            if (a < 0 || a > 255)
                throw new ArgumentException("Value of alpha must be between 0 and 255.");

            R = (byte)r;
            G = (byte)g;
            B = (byte)b;
            A = (byte)a;
        }
    }
}