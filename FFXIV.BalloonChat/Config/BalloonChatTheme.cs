using System;
using System.Windows;
using System.Windows.Media;

using FFXIV.PluginCore;

namespace FFXIV.BalloonChat.Config
{
    [Serializable]
    public class BalloonChatTheme
    {
        public BalloonChatTheme()
        {
            this.ID = Guid.Empty;
            this.Name = "Default Theme";
            this.FontColor = Colors.Gainsboro.ToString();
            this.FontOutlineColor = Colors.CornflowerBlue.ToString();
            this.FontShadowColor = Colors.Gainsboro.ToString();
            this.BorderColor = Colors.Gainsboro.ToString();
            this.BorderThickness = 2.4d;
            this.BackgroundColor = "#F0252525";
            this.Font = new FontInfo(
                new FontFamily("メイリオ"),
                13.0d,
                FontStyles.Normal,
                FontWeights.Normal,
                FontStretches.Normal);
        }

        public Guid ID { get; set; }
        public string Name { get; set; }
        public string FontColor { get; set; }
        public string FontOutlineColor { get; set; }
        public string FontShadowColor { get; set; }
        public string BorderColor { get; set; }
        public double BorderThickness { get; set; }
        public string BackgroundColor { get; set; }
        public FontInfo Font { get; set; }
    }
}
