using TwitchLib.EventSub.Webhooks.Core.Models;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs
{
    public abstract class TwitchLibEventSubEventArgs<T> : System.EventArgs where T: new()
    {
        public WebhookEventSubMetadata Metadata { get; set; } = new();
        public T Notification { get; set; } = new();
    }
}