using AnimeTime.Utilities.MathHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Colors
{
    internal class RgbaHslaConverter
    {
        #region To HSLA Conversion
        public static HslaColor ToHsla(RgbaColor color)
        {
            float rP = color.R / 255.0f;
            float gP = color.G / 255.0f;
            float bP = color.B / 255.0f;

            float cMax = MathUtil.Max(rP, gP, bP);
            float cMin = MathUtil.Min(rP, gP, bP);

            float delta = cMax - cMin;

            float hue = CalculateHue(rP, gP, bP, cMax, delta);
            float lightness = CalculateLightness(cMax, cMin);
            float saturation = CalculateSaturation(delta, lightness);

            return new HslaColor((int)hue, saturation * 100, lightness * 100, color.A);
        }

        private static float CalculateSaturation(float delta, float lightness)
        {
            if (delta == 0) return 0;
            else
                return delta / (1 - Math.Abs(2 * lightness - 1));
        }
        private static float CalculateLightness(float cMax, float cMin)
        {
            return (cMax + cMin) / 2;
        }
        private static float CalculateHue(float rP, float gP, float bP, float cMax, float delta)
        {
            if (delta == 0) return 0;
            else if (cMax == rP)
                return 60 * (((gP - bP) / delta) % 6);
            else if (cMax == gP)
                return 60 * ((bP - rP) / delta + 2);
            else if (cMax == bP)
                return 60 * ((rP - gP) / delta + 4);
            else
                throw new ArgumentException("Invalid cMax value. cMax must be a maxium of rP, gP, bP");
        }
        #endregion

        #region To RGBA Conversion
        public static RgbaColor ToRgba(HslaColor hsla)
        {
            int h = hsla.H;
            float l = hsla.L / 100.0f;
            float s = hsla.S / 100.0f;

            var c = (1 - Math.Abs(2 * l - 1)) * s;
            var x = c * (1 - Math.Abs((h / 60) % 2 - 1));
            var m = l - c / 2.0f;

            var (rP, gP, bP) = CalculateRgbPrimes(h, c, x);

            int r = (int)Math.Round((rP + m) * 255);
            int g = (int)Math.Round((gP + m) * 255);
            int b = (int)Math.Round((bP + m) * 255);

            return new RgbaColor(r, g, b, hsla.A);
        }
        private static (float rP, float gP, float bP) CalculateRgbPrimes(int hue, float c, float x)
        {
            float rP, gP, bP;
            if (hue < 60)
            {
                rP = c;
                gP = x;
                bP = 0;
            }
            else if (hue < 120)
            {
                rP = x;
                gP = c;
                bP = 0;
            }
            else if (hue < 180)
            {
                rP = 0;
                gP = c;
                bP = x;
            }
            else if (hue < 240)
            {
                rP = 0;
                gP = x;
                bP = c;
            }
            else if (hue < 300)
            {
                rP = x;
                gP = 0;
                bP = c;
            }
            else
            {
                rP = c;
                gP = 0;
                bP = x;
            }

            return (rP, gP, bP);
        }
        #endregion
    }
}
