using System;
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

            this.Startup += this.App_Startup;
            this.Exit += this.App_Exit;
            this.DispatcherUnhandledException += this.App_DispatcherUnhandledException;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                TraceUtility.WriteLog("App_Startup begin.");

                this.componet = new TaskTrayComponent();

                if (!BalloonChatConfig.ExistConfigFile)
                {
                    this.componet.ShowMainWindow();
                }

                // 監視タスクを開始する
                BalloonWindowController.Default.Start();
            }
            finally
            {
                TraceUtility.WriteLog("App_Startup end.");
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                TraceUtility.WriteLog("App_Exit begin.");

                // 監視タスクを終了する
                try
                {
                    BalloonWindowController.Default.End();
                }
                catch (Exception ex)
                {
                    TraceUtility.WriteExceptionLog(ex);
                }

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
                    TraceUtility.WriteExceptionLog(ex);
                }
            }
            finally
            {
                TraceUtility.WriteLog("App_Exit end.");
            }
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            TraceUtility.WriteExceptionLog(
                "App_DispatcherUnhandledException",
                e.Exception);

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
