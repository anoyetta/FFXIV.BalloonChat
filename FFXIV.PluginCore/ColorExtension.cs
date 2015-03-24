using System.Windows.Media;

namespace FFXIV.PluginCore
{
    public static class ColorExtension
    {
        public static Color ToColor(
            this string colorByHTML)
        {
            if (string.IsNullOrWhiteSpace(colorByHTML))
            {
                return Colors.White;
            }

            return (Color)ColorConverter.ConvertFromString(colorByHTML);
        }
    }
}
