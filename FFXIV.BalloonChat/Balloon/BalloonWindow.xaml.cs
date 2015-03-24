using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

using FFXIV.BalloonChat.Config;

namespace FFXIV.BalloonChat.Balloon
{
    public partial class BalloonWindow : Window
    {
        private const int DisplayTimeByChar = 240;
        private const int DisplayTimeMin = 800;
        private const double DefaultWidth = 400d;

        private static List<BalloonWindow> balloonList = new List<BalloonWindow>();

        private DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Normal);
        private DateTime timeOfClosing;

        public BalloonWindow()
        {
            this.InitializeComponent();
        }

        public string Speaker
        {
            get { return this.BalloonControl.Speaker; }
            private set { this.BalloonControl.Speaker = value; }
        }

        public string Message
        {
            get { return this.BalloonControl.Message; }
            private set { this.BalloonControl.Message = value; }
        }

        public BalloonChatMode ChatMode
        {
            get;
            private set;
        }

        public BalloonChatTheme Theme
        {
            get { return this.BalloonControl.Theme; }
            private set { this.BalloonControl.Theme = value; }
        }

        public static void ShowBalloon(
            BalloonChatMode chatModeSetting,
            string speaker,
            string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            lock (balloonList)
            {
                var balloon = (
                    from x in balloonList
                    where
                    x.Speaker == speaker &&
                    x.ChatMode.Mode == chatModeSetting.Mode
                    select
                    x).FirstOrDefault();

                // 既に同じ人の発言がある場合はメッセージを追加する
                if (balloon != null)
                {
                    message = message.Trim();

                    // 表示時間を加算する
                    var displayTime = chatModeSetting.DisplayTime;
                    if (chatModeSetting.AutoDisplayTime)
                    {
                        displayTime = DisplayTimeByChar * message.Length;

                        if (displayTime < DisplayTimeMin)
                        {
                            displayTime = DisplayTimeMin;
                        }
                    }

                    balloon.timeOfClosing = balloon.timeOfClosing.AddMilliseconds(displayTime);

                    // メッセージを追加する
                    balloon.Message += Environment.NewLine + message;

                    return;
                }
            }

            // 新しいバルーンを表示する
            new BalloonWindow().ShowNewBalloon(
                chatModeSetting,
                speaker,
                message);
        }

        private void ShowNewBalloon(
            BalloonChatMode chatModeSetting,
            string speaker,
            string message)
        {
            this.Theme = (
                from x in BalloonChatConfig.Default.Themes
                where
                x.ID == chatModeSetting.Theme
                select
                x).FirstOrDefault();

            if (this.Theme == null)
            {
                return;
            }

            this.Title = "Balloon - " + chatModeSetting.Mode.ToString();
            this.Opacity = (100d - (double)BalloonChatConfig.Default.Opacity) / 100d;

            this.Speaker = speaker.Trim();
            this.Message = message.Trim();
            this.ChatMode = chatModeSetting;
            this.BalloonControl.ChatMode = this.ChatMode.Mode.ToString();

            // Clickを透過させる
            this.SourceInitialized += (s1, e1) =>
            {
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
            };

            // Windowの横幅を決定する
            this.Width = DefaultWidth * (this.Theme.Font.Size / 13.0d);

            lock (balloonList)
            {
                // バルーンリストに追加する
                balloonList.Add(this);

                // 表示時間を決定する
                var displayTime = this.ChatMode.DisplayTime;
                if (this.ChatMode.AutoDisplayTime)
                {
                    displayTime = DisplayTimeByChar * this.Message.Length;

                    if (displayTime < DisplayTimeMin)
                    {
                        displayTime = DisplayTimeMin;
                    }
                }

                this.timeOfClosing = DateTime.Now.AddMilliseconds(displayTime);
            }

            // 表示する
            this.Show();

            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            this.timer.Tick += (s, e) =>
            {
                lock (balloonList)
                {
                    if (DateTime.Now >= this.timeOfClosing)
                    {
                        this.timer.Stop();
                        this.Close();
                        balloonList.Remove(this);
                    }
                }
            };

            this.timer.Start();
        }

        #region NativeMethods

        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        #endregion
    }
}
