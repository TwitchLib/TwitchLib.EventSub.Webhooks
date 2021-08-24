using TwitchLib.EventSub.Webhooks.Core.Models;
using TwitchLib.EventSub.Webhooks.Core.SubscriptionTypes.Stream;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs.Stream
{
    public class StreamOnlineArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<StreamOnline>>
    { }
}