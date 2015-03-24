using System.Collections.Generic;
using FFXIV.BalloonChat.Config;

namespace FFXIV.BalloonChat.Pages
{
    public class SettingsGeneralViewModel
    {
        public List<BalloonChatMode> Items
        {
            get
            {
                return BalloonChatConfig.Default.ChatModeSettings;
            }
        }
    }
}
