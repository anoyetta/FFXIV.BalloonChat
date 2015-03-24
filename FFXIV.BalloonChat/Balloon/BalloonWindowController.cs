using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using FFXIV.PluginCore;
using ffxivlib;

namespace FFXIV.BalloonChat.Balloon
{
    public class BalloonWindowController
    {
        private const int DefaultPollingInterval = 100;

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
        private int pollingInterval;

        public void Start()
        {
            this.pollingTaskCTS = new CancellationTokenSource();

            this.pollingInterval = DefaultPollingInterval;

            this.pollingTask = new Task(() =>
            {
                while (true)
                {
                    Thread.Sleep(this.pollingInterval);

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

        private FFXIVLIB ffivlib;
        private Chatlog chatLog;

        private void PollingTaskCore()
        {
            if (Process.GetProcessesByName("ffxiv").Length > 0)
            {
                if (this.ffivlib == null)
                {
                    this.ffivlib = new FFXIVLIB();
                }
            }
            else
            {
                this.pollingInterval = 3 * 1000;
                return;
            }

            this.pollingInterval = DefaultPollingInterval;

            if (this.chatLog == null)
            {
                this.chatLog = this.ffivlib.GetChatlog();
            }

            if (!this.chatLog.IsNewLine())
            {
                return;
            }

            foreach (var entry in this.chatLog.GetChatLogLines())
            {
                TraceUtility.WriteLog(entry.RawString);
            }

            var entitys = this.ffivlib.GetEntityByType(TYPE.Player);
            foreach (var entity in entitys)
            {
                var t = string.Empty;
                t += "{" + Environment.NewLine;
                t += "    Name = " + entity.Name + "," + Environment.NewLine;
                t += "    X = " + entity.X.ToString() + "," + Environment.NewLine;
                t += "    Y = " + entity.Y.ToString() + "," + Environment.NewLine;
                t += "    Z = " + entity.Z.ToString() + "," + Environment.NewLine;
                t += "}" + Environment.NewLine;
                TraceUtility.WriteLog(t);
            }
        }
    }
}
