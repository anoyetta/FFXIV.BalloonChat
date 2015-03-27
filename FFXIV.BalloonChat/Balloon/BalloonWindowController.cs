using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using FFXIVAPP.Client.Helper;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.IPluginInterface.Events;

namespace FFXIV.BalloonChat.Balloon
{
    public class BalloonWindowController
    {
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

        public ActorEntity CurrentUser { get; private set; }
        public List<ActorEntity> CurrentPCs { get; private set; }

        public void Initialize()
        {
            HelperViewModel.Instance.PluginHost.NewPCEntries += this.PluginHost_NewPCEntries;
            HelperViewModel.Instance.PluginHost.NewChatLogEntry += this.PluginHost_NewChatLogEntry;
        }

        private void PluginHost_NewPCEntries(object sender, ActorEntitiesEvent e)
        {
            if (e.ActorEntities == null)
            {
                return;
            }

            if (e.ActorEntities.Any())
            {
                this.CurrentUser = e.ActorEntities.FirstOrDefault();
                this.CurrentPCs = new List<ActorEntity>(e.ActorEntities);
            }
        }

        private void PluginHost_NewChatLogEntry(object sender, ChatLogEntryEvent e)
        {
            var entry = e.ChatLogEntry;
            if (entry == null)
            {
                return;
            }

            Trace.WriteLine(entry.Raw);
            Trace.WriteLine(entry.Line);
        }
    }
}
