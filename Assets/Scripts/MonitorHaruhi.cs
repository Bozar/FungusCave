using System;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class MonitorHaruhi : MonoBehaviour
    {
        public EventHandler<SendMessageEventArgs> HaruhiSays;

        public void Trigger(SendMessageEventArgs e)
        {
            OnHaruhiSays(e);
        }

        protected virtual void OnHaruhiSays(SendMessageEventArgs e)
        {
            HaruhiSays?.Invoke(this, e);
        }
    }

    public class SendMessageEventArgs : EventArgs
    {
        public readonly string Message;

        public SendMessageEventArgs(string message)
        {
            Message = message;
        }
    }
}
