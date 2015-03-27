using System;

namespace FFXIV.BalloonChat.Config
{
    [Serializable]
    public class BalloonChatMode
    {
        public BalloonChatMode()
        {
            this.Mode = Constants.ChatModes.Say;
            this.Enabled = true;
            this.Theme = Guid.Empty;
            this.EnabledSoundEffect = true;
            this.AutoDisplayTime = true;
            this.DisplayTime = 1500;
        }

        public ChatMode Mode { get; set; }
        public bool Enabled { get; set; }
        public Guid Theme { get; set; }
        public bool EnabledSoundEffect { get; set; }
        public bool AutoDisplayTime { get; set; }
        public long DisplayTime { get; set; }
    }
}
