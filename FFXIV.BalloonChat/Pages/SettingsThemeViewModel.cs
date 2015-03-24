using System.Collections.Generic;
using FFXIV.BalloonChat.Config;

namespace FFXIV.BalloonChat.Pages
{
    public class SettingsThemeViewModel
    {
        public List<BalloonChatTheme> Items
        {
            get
            {
                return BalloonChatConfig.Default.Themes;
            }
        }
    }
}
