using AnimeTime.Utilities.MathHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Utilities.Colors
{
    public static class ColorExtensions
    {
        #region To HSLA Conversions
        public static HslaColor ToHslaColor(this Color color)
        {
            return RgbaHslaConverter.ToHsla(new RgbaColor(color.R, color.G, color.B, color.A));
        }
        public static HslaColor ToHslaColor(this System.Windows.Media.Color color)
        {
            return RgbaHslaConverter.ToHsla(new RgbaColor(color.R, color.G, color.B, color.A));
        }
        #endregion

        #region To RGBA Conversions
        
        public static System.Drawing.Color ToColor(this HslaColor hsla)
        {
            var rgba = RgbaHslaConverter.ToRgba(hsla);
            return Color.FromArgb(rgba.A, rgba.R, rgba.G, rgba.B);
        }
        public static System.Windows.Media.Color ToMediaColor(this HslaColor hsla)
        {
            var rgba = RgbaHslaConverter.ToRgba(hsla);
            return System.Windows.Media.Color.FromArgb(rgba.A, rgba.R, rgba.G, rgba.B);
        }
        #endregion
    }
}
