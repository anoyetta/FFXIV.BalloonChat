using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

using FFXIV.BalloonChat.Balloon;
using FFXIV.BalloonChat.Config;
using FFXIV.PluginCore;

namespace FFXIV.BalloonChat
{
    public partial class DebugWindow : Window
    {
        private ViewTraceListener traceListener;

        public DebugWindow()
        {
            this.InitializeComponent();

            this.SpeakButton.Click += this.SpeakButton_Click;
            this.Loaded += this.DebugWindow_Loaded;
        }

        private void DebugWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.traceListener = new ViewTraceListener(this.LogTextBox);
            Trace.Listeners.Add(this.traceListener);

            this.Closed += (s1, e1) =>
            {
                Trace.Listeners.Remove(this.traceListener);
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
                TraceUtility.WriteExceptionLog(ex);

                MessageBox.Show(
                    ex.ToString());
            }
        }
    }

    internal class ViewTraceListener : TraceListener
    {
        private TextBox textBox;

        public ViewTraceListener(
            TextBox textBox)
        {
            this.textBox = textBox;
        }

        public override void Write(string message)
        {
            this.textBox.Text += message;
            this.textBox.ScrollToEnd();
        }

        public override void WriteLine(string message)
        {
            this.textBox.Text += message + Environment.NewLine;
            this.textBox.ScrollToEnd();
        }
    }
}
