using System.Collections.Generic;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs
{
    public abstract class TwitchLibEventSubEventArgs<T> : System.EventArgs where T: new()
    {
        public Dictionary<string, string> Headers { get; set; } = new();
        public T Notification { get; set; } = new();
    }
}