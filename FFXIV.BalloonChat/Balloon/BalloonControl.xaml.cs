using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FFXIV.BalloonChat.Config;

using FFXIV.PluginCore;

namespace FFXIV.BalloonChat.Balloon
{
    public partial class BalloonControl : UserControl
    {
        private FontInfo font;
        private BalloonChatTheme theme;

        public BalloonControl()
        {
            this.InitializeComponent();
        }

        public string Speaker
        {
            get { return this.SpeakerTextBlock.Text; }
            set { this.SpeakerTextBlock.Text = value; }
        }

        public string ChatMode
        {
            get { return this.ChatModeTextBlock.Text; }
            set { this.ChatModeTextBlock.Text = value; }
        }

        public string Message
        {
            get { return this.MessageTextBlock.Text; }
            set { this.MessageTextBlock.Text = value; }
        }

        public SolidColorBrush FontBrush
        {
            get { return this.Resources["FontBrush"] as SolidColorBrush; }
            set { this.Resources["FontBrush"] = value; }
        }

        public SolidColorBrush FontOutlineBrush
        {
            get { return this.Resources["FontOutlineBrush"] as SolidColorBrush; }
            set { this.Resources["FontOutlineBrush"] = value; }
        }

        public Color FontShadowColor
        {
            get { return (Color)this.Resources["FontShadowColor"]; }
            set { this.Resources["FontShadowColor"] = value; }
        }

        public new SolidColorBrush BorderBrush
        {
            get { return this.Resources["BorderBrush"] as SolidColorBrush; }
            set { this.Resources["BorderBrush"] = value; }
        }

        public SolidColorBrush BorderBackground
        {
            get { return this.Resources["BorderBackground"] as SolidColorBrush; }
            set { this.Resources["BorderBackground"] = value; }
        }

        public new double BorderThickness
        {
            get { return ((Thickness)this.Resources["BorderThickness"]).Left; }
            set { this.Resources["BorderThickness"] = new Thickness(value); }
        }

        public BalloonChatTheme Theme
        {
            get { return this.theme; }
            set
            {
                this.theme = value;

                this.FontBrush.Color = this.theme.FontColor.ToColor();
                this.FontOutlineBrush.Color = this.theme.FontOutlineColor.ToColor();
                this.FontShadowColor = this.theme.FontShadowColor.ToColor();
                this.BorderBrush.Color = this.theme.BorderColor.ToColor();
                this.BorderThickness = this.theme.BorderThickness;
                this.BorderBackground.Color = this.theme.BackgroundColor.ToColor();
                this.Font = this.theme.Font;
            }
        }

        public FontInfo Font
        {
            get { return this.font; }
            set
            {
                this.font = value;

                this.MessageTextBlock.SetFontInfo(this.font);

                var thickness = this.MessageTextBlock.FontSize / 100.0d * 0.35d;
                if (thickness < 0.1d)
                {
                    thickness = 0.1d;
                }

                this.MessageTextBlock.StrokeThickness = thickness;
            }
        }
    }
}
