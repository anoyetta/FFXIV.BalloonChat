using System;
using System.Reflection;
using System.Windows;

using FFXIV.PluginCore;
using FirstFloor.ModernUI.Windows.Controls;

namespace FFXIV.BalloonChat
{
    public partial class MainWindow : ModernWindow
    {
        private static MainWindow instance;

        public static MainWindow Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainWindow();
                }

                return instance;
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();

            this.Title =
                EnvironmentUtility.GetProductName() + " " +
                EnvironmentUtility.GetVersion().ToStringShort();

            this.Closing += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = false;
                e.Cancel = true;
            };
        }
    }
}
