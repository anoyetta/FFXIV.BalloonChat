using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace FFXIV.PluginCore.Controls
{
    public class FontFamilyToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as FontFamily;
            var currentLang = XmlLanguage.GetLanguage(culture.IetfLanguageTag);
            return v.FamilyNames.FirstOrDefault(o => o.Key == currentLang).Value ?? v.Source;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
