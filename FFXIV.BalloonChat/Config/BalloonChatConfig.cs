using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using FFXIV.PluginCore;

namespace FFXIV.BalloonChat.Config
{
    [Serializable]
    public class BalloonChatConfig
    {
        private static BalloonChatConfig instance;

        public static BalloonChatConfig Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new BalloonChatConfig();
                    instance.Load();
                }

                return instance;
            }
        }

        public static string FileName
        {
            get
            {
                return Path.Combine(
                    EnvironmentUtility.GetAppDataPath(),
                    "FFXIV.BalloonChat.config");
            }
        }

        public static bool ExistConfigFile
        {
            get
            {
                return File.Exists(FileName);
            }
        }

        public int Opacity { get; set; }

        public List<BalloonChatMode> ChatModeSettings { get; set; }

        public List<BalloonChatTheme> Themes { get; set; }

        public void Reset()
        {
            this.Opacity = 5;

            this.ChatModeSettings = new List<BalloonChatMode>();
            foreach (var mode in Constants.ChatModes.Array)
            {
                if (mode.Name == Constants.ChatModes.Public.Name)
                {
                    continue;
                }

                this.ChatModeSettings.Add(new BalloonChatMode()
                {
                    Mode = mode
                });
            }

            this.Themes = new List<BalloonChatTheme>(new BalloonChatTheme[]
            {
                new BalloonChatTheme()
            });
        }

        public void Load()
        {
            var file = FileName;

            if (!File.Exists(file))
            {
                this.Reset();
                return;
            }

            try
            {
                using (var sr = new StreamReader(file, new UTF8Encoding(false)))
                {
                    if (sr.BaseStream.Length <= 0)
                    {
                        this.Reset();
                        return;
                    }

                    var xs = new XmlSerializer(instance.GetType());
                    instance = xs.Deserialize(sr) as BalloonChatConfig;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        public void Save()
        {
            using (var sw = new StreamWriter(FileName, false, new UTF8Encoding(false)))
            {
                var xs = new XmlSerializer(instance.GetType());
                xs.Serialize(sw, instance);
            }
        }
    }
}
