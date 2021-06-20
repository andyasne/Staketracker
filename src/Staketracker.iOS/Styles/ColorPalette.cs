using System;
using UIKit;

namespace Staketracker.iOS.Styles
{
    public static class ColorPalette
    {
        public static UIColor Primary => FromHexString("#124B8F");
        public static UIColor PrimaryDark => FromHexString("#052B59");
        public static UIColor PrimaryLight => FromHexString("#4B78AE");

        public static UIColor Accent => FromHexString("#DAAD0B");
        public static UIColor AccentDark => FromHexString("#886A00");
        public static UIColor AccentLight => FromHexString("#FFDC60");

        public static UIColor PrimaryText => FromHexString("#001023");
        public static UIColor SecondaryText => FromHexString("#ffffff");

        private static UIColor FromHexString(string hexValue)
        {
            var colorString = hexValue.Replace("#", string.Empty);
            float red, green, blue;

            switch (colorString.Length)
            {
                case 3: // #RGB
                    {
                        red = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(0, 1)), 16) / 255f;
                        green = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(1, 1)), 16) / 255f;
                        blue = Convert.ToInt32(string.Format("{0}{0}", colorString.Substring(2, 1)), 16) / 255f;
                        return UIColor.FromRGB(red, green, blue);
                    }
                case 6: // #RRGGBB
                    {
                        red = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                        green = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                        blue = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                        return UIColor.FromRGB(red, green, blue);
                    }
                case 8: // #AARRGGBB
                    {
                        var alpha = Convert.ToInt32(colorString.Substring(0, 2), 16) / 255f;
                        red = Convert.ToInt32(colorString.Substring(2, 2), 16) / 255f;
                        green = Convert.ToInt32(colorString.Substring(4, 2), 16) / 255f;
                        blue = Convert.ToInt32(colorString.Substring(6, 2), 16) / 255f;
                        return UIColor.FromRGBA(red, green, blue, alpha);
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(hexValue), $"Color value {hexValue} is invalid. Expected format #RBG, #RRGGBB, or #AARRGGBB");
            }
        }
    }
}