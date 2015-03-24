using System;
using System.Diagnostics;
using System.IO;

namespace FFXIV.PluginCore
{
    public static class TraceUtility
    {
        static TraceUtility()
        {
            SetupListener();
        }

        public static void SetupListener()
        {
            if (Trace.Listeners.Count > 0)
            {
                var listener = Trace.Listeners[0] as DefaultTraceListener;
                if (listener != null)
                {
                    var file = Path.Combine(
                        EnvironmentUtility.GetAppDataPath(),
                        EnvironmentUtility.GetProductName() + ".log");

                    listener.LogFileName = file;
                }
            }
        }

        public static void WriteLog(
            string text,
            params object[] args)
        {
            Trace.WriteLine(
                DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss.fff]") + " " +
                string.Format(text, args));
        }

        public static void WriteExceptionLog(
            Exception ex)
        {
            WriteExceptionLog(null, ex);
        }

        public static void WriteExceptionLog(
            string text,
            Exception ex)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Trace.WriteLine(
                    DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss.fff]") + " " +
                    ex.ToString());
            }
            else
            {
                Trace.WriteLine(
                    DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss.fff]") + " " +
                    text);
                Trace.WriteLine(
                    ex.ToString());
            }
        }
    }
}
