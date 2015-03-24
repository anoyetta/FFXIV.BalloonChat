using System.Windows;
using System.Windows.Controls;

using FFXIV.BalloonChat.Config;

namespace FFXIV.BalloonChat.Pages
{
    public partial class SettingsGeneralPage : UserControl
    {
        public SettingsGeneralPage()
        {
            this.InitializeComponent();

            this.ApplyButton.Click += this.ApplyButton_Click;
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            BalloonChatConfig.Default.Save();
        }
    }
}
