﻿using System;
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
                TraceUtility.WriteExceptionLog(ex);

                MessageBox.Show(
                    ex.ToString());
            }
        }
    }
}
