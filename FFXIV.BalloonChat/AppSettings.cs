using System.Configuration;

using FFXIV.PluginCore;

namespace FFXIV.BalloonChat
{
    public static class AppSettings
    {
        public static bool DebugMode
        {
            get
            {
                return ConfigurationManager.AppSettings["DebugMode"].ToBoolOrDefault();
            }
        }
    }
}
