using TwitchLib.EventSub.Webhooks.Core.Models;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs.Channel
{
    public class ChannelPollProgressArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<ChannelPollProgress>>
    { }
}