using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Controls;

namespace FFXIV.PluginCore
{
    public static class TraceUtility
    {
        private static CustomTraceListener listener;

        public static CustomTraceListener TraceListner
        {
            get { return listener; }
        }

        public static void SetupListener(
            Assembly assembly)
        {
            Trace.Listeners.Remove("Default");

            listener = new CustomTraceListener(assembly)
            {
                LogFileName = Path.Combine(
                    EnvironmentUtility.GetAppDataPath(),
                    EnvironmentUtility.GetProductName() + ".log")
            };

            Trace.Listeners.Add(listener);
        }
    }

    public class CustomTraceListener : TraceListener
    {
        private DefaultTraceListener defaultTraceListener = new DefaultTraceListener();
        private List<TextBox> textBoxes = new List<TextBox>();
        private string logFile;

        public CustomTraceListener(
            Assembly assembly)
        {
            this.Name = assembly.GetName().Name;
        }

        private CustomTraceListener()
        {
        }

        public List<TextBox> TextBoxes
        {
            get { return this.textBoxes; }
        }

        public string LogFileName
        {
            get { return this.logFile; }
            set { this.logFile = value; }
        }

        public override void Write(string message)
        {
            try
            {
                message = DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss.fff]") + " " + message;

                if (this.defaultTraceListener != null)
                {
                    this.defaultTraceListener.Write(message);
                }

                foreach (var tb in this.textBoxes)
                {
                    if (tb != null &&
                        tb.IsLoaded)
                    {
                        tb.AppendText(message);
                        tb.ScrollToEnd();
                    }
                }

                if (!string.IsNullOrWhiteSpace(this.logFile))
                {
                    var dir = Path.GetDirectoryName(this.logFile);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.AppendAllText(this.logFile, message);
                }
            }
            catch
            {
            }
        }

        public override void WriteLine(string message)
        {
            this.Write(message + Environment.NewLine);
        }
    }
}
