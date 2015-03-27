using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

using FFXIV.BalloonChat.Config;
using FFXIV.PluginCore;

namespace FFXIV.BalloonChat
{
    public partial class TaskTrayComponent : Component
    {
        public TaskTrayComponent()
        {
            this.InitializeComponent();

            this.notifyIcon.Text =
                EnvironmentUtility.GetProductName() + " " +
                EnvironmentUtility.GetVersion().ToStringShort();

            this.notifyIcon.DoubleClick += (s, e) => this.ShowMainWindow();
            this.SettingsMenuItem.Click += (s, e) => this.ShowMainWindow();

            this.ExitMenuItem.Click += (s, e) =>
            {
                BalloonChatConfig.Default.Save();
                Application.Current.Shutdown();
            };
        }

        public void ShowMainWindow()
        {
            if (MainWindow.Default.WindowState == WindowState.Minimized)
            {
                MainWindow.Default.WindowState = WindowState.Normal;
            }

            MainWindow.Default.Show();
            MainWindow.Default.Activate();

            MainWindow.Default.ShowInTaskbar = true;

            // デバッグWindowを表示する？
            if (AppSettings.DebugMode)
            {
                new DebugWindow().Show();
            }
        }
    }
}
