using System;
using System.Diagnostics;
using System.Windows;

using FFXIV.BalloonChat.Balloon;
using FFXIV.BalloonChat.Config;
using FFXIV.PluginCore;

namespace FFXIV.BalloonChat
{
    public partial class DebugWindow : Window
    {
        public DebugWindow()
        {
            this.InitializeComponent();

            this.SpeakButton.Click += this.SpeakButton_Click;
            this.Loaded += this.DebugWindow_Loaded;
        }

        private void DebugWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TraceUtility.TraceListner.TextBoxes.Add(this.LogTextBox);

            this.Closed += (s1, e1) =>
            {
                TraceUtility.TraceListner.TextBoxes.Remove(this.LogTextBox);
            };
        }

        private void SpeakButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BalloonWindow.ShowBalloon(
                    this.ChatModeComboBox.SelectedItem as BalloonChatMode,
                    this.SpeakerTextBox.Text,
                    this.MessageTextBox.Text);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
