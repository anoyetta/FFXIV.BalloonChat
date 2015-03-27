using System;

namespace FFXIV.BalloonChat
{
    public static class Constants
    {
        public static class ChatModes
        {
            public static readonly ChatMode Public =
                new ChatMode("Public", "000A", "000C", "000D", "000E");

            public static readonly ChatMode Say =
                new ChatMode("Say", "000A");

            public static readonly ChatMode Tell =
                new ChatMode("Tell", "000C", "000D");

            public static readonly ChatMode Party =
                new ChatMode("Party", "000E");

            public static readonly ChatMode Shout =
                new ChatMode("Shout", "000B");

            public static readonly ChatMode Yell =
                new ChatMode("Yell", "001E");

            public static readonly ChatMode LS1 =
                new ChatMode("LS1", "0010");

            public static readonly ChatMode LS2 =
                new ChatMode("LS2", "0011");

            public static readonly ChatMode LS3 =
                new ChatMode("LS3", "0012");

            public static readonly ChatMode LS4 =
                new ChatMode("LS4", "0013");

            public static readonly ChatMode LS5 =
                new ChatMode("LS5", "0014");

            public static readonly ChatMode LS6 =
                new ChatMode("LS6", "0015");

            public static readonly ChatMode LS7 =
                new ChatMode("LS7", "0016");

            public static readonly ChatMode LS8 =
                new ChatMode("LS8", "0017");

            public static readonly ChatMode Alliance =
                new ChatMode("Alliance", "000F");

            public static readonly ChatMode FC =
                new ChatMode("FC", "0018");

            public static readonly ChatMode[] Array = new ChatMode[]
            {
                Public,
                Say,
                Tell,
                Shout,
                Yell,
                Party,
                Alliance,
                FC,
                LS1,
                LS2,
                LS3,
                LS4,
                LS5,
                LS6,
                LS7,
                LS8,
            };
        }
    }

    [Serializable]
    public class ChatMode
    {
        public ChatMode()
        {
            this.Name = string.Empty;
            this.Codes = new string[0];
        }

        public ChatMode(
            string name,
            params string[] codes)
        {
            this.Name = name;
            this.Codes = codes;
        }

        public string Name { get; set; }
        public string[] Codes { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
