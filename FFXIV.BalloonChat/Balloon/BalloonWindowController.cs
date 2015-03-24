using System;
using System.Threading;
using System.Threading.Tasks;

using FFXIV.PluginCore;
using ffxivlib;

namespace FFXIV.BalloonChat.Balloon
{
    public class BalloonWindowController
    {
        private const int PollingInterval = 100;

        #region Singleton

        private static BalloonWindowController instance;

        public static BalloonWindowController Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new BalloonWindowController();
                }

                return instance;
            }
        }

        #endregion

        #region PollingTask

        private Task pollingTask;
        private CancellationTokenSource pollingTaskCTS;

        public void Start()
        {
            this.pollingTaskCTS = new CancellationTokenSource();

            this.pollingTask = new Task(() =>
            {
                while (true)
                {
                    Thread.Sleep(PollingInterval);

                    if (this.pollingTaskCTS.Token.IsCancellationRequested)
                    {
                        this.pollingTaskCTS.Token.ThrowIfCancellationRequested();
                    }

                    try
                    {
                        this.PollingTaskCore();
                    }
                    catch (Exception ex)
                    {
                        TraceUtility.WriteExceptionLog(ex);
                        Thread.Sleep(3 * 1000);
                    }
                }
            }, this.pollingTaskCTS.Token);

            // ポーリングを開始する
            this.pollingTask.Start();
            TraceUtility.WriteLog("監視タスクを開始しました。");
        }

        public void End()
        {
            if (this.pollingTask == null ||
                this.pollingTaskCTS == null)
            {
                return;
            }

            this.pollingTaskCTS.Cancel();

            try
            {
                this.pollingTask.Wait();
            }
            catch (AggregateException)
            {
                TraceUtility.WriteLog("監視タスクを終了しました。");
            }

            this.pollingTask.Dispose();
            this.pollingTask = null;

            this.pollingTaskCTS.Dispose();
            this.pollingTaskCTS = null;
        }

        #endregion

        private FFXIVLIB ffivlib = new FFXIVLIB();

        private void PollingTaskCore()
        {
            var chatLog = this.ffivlib.GetChatlog();
            if (chatLog == null)
            {
                return;
            }

            if (!chatLog.IsNewLine())
            {
                return;
            }

            foreach (var entry in chatLog.GetChatLogLines())
            {
                TraceUtility.WriteLog(entry.RawString);
            }
        }
    }
}
