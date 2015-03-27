using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

using FFXIV.BalloonChat.Balloon;
using FFXIV.BalloonChat.Config;
using FFXIV.PluginCore;

namespace FFXIV.BalloonChat
{
    public partial class App : Application
    {
        private TaskTrayComponent componet;

        public App()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            TraceUtility.SetupListener(Assembly.GetExecutingAssembly());

            this.Startup += this.App_Startup;
            this.Exit += this.App_Exit;
            this.DispatcherUnhandledException += this.App_DispatcherUnhandledException;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Trace.WriteLine("App_Startup begin.");

                this.componet = new TaskTrayComponent();

                if (!BalloonChatConfig.ExistConfigFile)
                {
                    BalloonChatConfig.Default.Save();
                    this.componet.ShowMainWindow();
                }

                BalloonWindowController.Default.Initialize();
            }
            finally
            {
                Trace.WriteLine("App_Startup end.");
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Trace.WriteLine("App_Exit begin.");

                // タスクトレイアイコンを閉じる
                try
                {
                    if (this.componet != null)
                    {
                        this.componet.Dispose();
                        this.componet = null;
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
            }
            finally
            {
                Trace.WriteLine("App_Exit end.");
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Trace.WriteLine(
                "App_DispatcherUnhandledException" + Environment.NewLine +
                e.Exception.ToString());

            var m = string.Empty;
            m += "予期しない例外が発生しました。アプリケーションを終了します。" + Environment.NewLine;
            m += Environment.NewLine;
            m += e.Exception.ToString();

            MessageBox.Show(
                m,
                EnvironmentUtility.GetProductName(),
                MessageBoxButton.OK,
                MessageBoxImage.Error,
                MessageBoxResult.OK);

            Application.Current.Shutdown();
        }
    }
}
