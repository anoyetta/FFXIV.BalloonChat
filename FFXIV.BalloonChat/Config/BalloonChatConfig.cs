using System;
using System.Collections.Generic;
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
            this.Opacity = 8;

            this.ChatModeSettings = new List<BalloonChatMode>(new BalloonChatMode[]
            {
                new BalloonChatMode()
                {
                    Mode = ChatMode.Say
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.Yell
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.Shout
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.Tell
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.Party
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.Alliance
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.FreeCompany
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.LinkShell1
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.LinkShell2
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.LinkShell3
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.LinkShell4
                },

                new BalloonChatMode()
                {
                    Mode = ChatMode.LinkShell5
                },
            });

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
                this.Save();

                return;
            }

            try
            {
                using (var sr = new StreamReader(file, new UTF8Encoding(false)))
                {
                    if (sr.BaseStream.Length > 0)
                    {
                        var xs = new XmlSerializer(instance.GetType());
                        instance = xs.Deserialize(sr) as BalloonChatConfig;
                    }
                }
            }
            catch (Exception ex)
            {
                TraceUtility.WriteExceptionLog(ex);
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
