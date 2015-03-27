using System;
using System.Windows.Media;

namespace FFXIV.PluginCore
{
    public static class ColorExtension
    {
        public static Color ToColor(
            this string colorByHTML)
        {
            var c = Colors.White;

            if (string.IsNullOrWhiteSpace(colorByHTML))
            {
                return c;
            }

            try
            {
                c = (Color)ColorConverter.ConvertFromString(colorByHTML);
            }
            catch
            {
            }

            return c;
        }
    }
}
