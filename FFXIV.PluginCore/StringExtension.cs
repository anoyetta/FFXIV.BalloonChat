using System;

namespace FFXIV.PluginCore
{
    public static class StringExtension
    {
        public static int ToIntOrDefault(
            this string t)
        {
            int v;
            if (!int.TryParse(t, out v))
            {
                v = 0;
            }

            return v;
        }

        public static long ToLongOrDefault(
            this string t)
        {
            long v;
            if (!long.TryParse(t, out v))
            {
                v = 0;
            }

            return v;
        }

        public static decimal ToDecimalOrDefault(
            this string t)
        {
            decimal v;
            if (!decimal.TryParse(t, out v))
            {
                v = 0;
            }

            return v;
        }

        public static float ToFloatOrDefault(
            this string t)
        {
            float v;
            if (!float.TryParse(t, out v))
            {
                v = 0;
            }

            return v;
        }

        public static double ToDoubleOrDefault(
            this string t)
        {
            double v;
            if (!double.TryParse(t, out v))
            {
                v = 0;
            }

            return v;
        }

        public static DateTime ToDateTimeOrDefault(
            this string t)
        {
            DateTime v;
            if (!DateTime.TryParse(t, out v))
            {
                v = DateTime.MinValue;
            }

            return v;
        }

        public static bool ToBoolOrDefault(
            this string t)
        {
            bool v;
            if (!bool.TryParse(t, out v))
            {
                v = false;
            }

            return v;
        }
    }
}
