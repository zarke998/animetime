using AnimeTime.Utilities.MathHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnimeTime.Utilities.Colors
{
    public readonly struct HslaColor
    {
        /// <summary>
        /// Hue. In range 0 - 360 (exclusive).
        /// </summary>
        public short H { get; }
        /// <summary>
        /// Saturation. In range 0 - 100 percent.
        /// </summary>
        public float S { get; }
        /// <summary>
        /// Lightness. In range 0 - 100 percent.
        /// </summary>
        public float L { get; }
        public byte A { get; }

        public const byte ALPHA_DEFAULT = 100;
        public const byte ALPHA_MAX = 255;
        public const short HUE_MAX = 360;
        public const float SATURATION_MAX = 100.0f;
        public const float LIGHTNESS_MAX = 100.0f;

        /// <summary>
        /// Initialize color values.
        /// </summary>
        /// <param name="h">Hue. Value in range 0 - 360.</param>
        /// <param name="s">Saturation. Value in range 0 - 100.</param>
        /// <param name="l">Lightness. Value in range 0 - 100</param>
        /// <param name="a">Alpha. Value in range 0 - 255.</param>
        public HslaColor(int h, float s, float l, int a)
        {
            if (h < 0 && h >= HUE_MAX) 
                throw new ArgumentException($"Hue must be between 0 and {HUE_MAX}");
            if (s < 0 && s > SATURATION_MAX) 
                throw new ArgumentException($"Saturation must be beetween 0 and {SATURATION_MAX}");
            if (l < 0 && s > LIGHTNESS_MAX) 
                throw new ArgumentException($"Lightness must be beetween 0 and {LIGHTNESS_MAX}");
            if (a < 0 && a > ALPHA_MAX)
                throw new ArgumentException($"Alpha must be beetween 0 and {ALPHA_MAX}");

            H = (short)h;
            S = s;
            L = l;
            A = (byte)a;
        }

        /// <summary>
        /// Initialize color values. Set alpha to default of 255.
        /// </summary>
        /// <param name="h">Hue. Value in range 0 - 360.</param>
        /// <param name="s">Saturation. Value in range 0 - 100.</param>
        /// <param name="l">Lightness. Value in range 0 - 100</param>
        public HslaColor(int h, float s, float l) : this(h, s, l, ALPHA_DEFAULT) { }

        /// <summary>
        /// Set hsla color lightness.
        /// </summary>
        /// <param name="lightness"> Value in range 0 - 100.</param>
        /// <returns></returns>
        public HslaColor SetLightness(float lightness)
        {
            return new HslaColor(this.H, this.S, lightness, this.A);
        }
    }
}
