using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Webhooks.Core.EventArgs;
using TwitchLib.EventSub.Webhooks.Core.Models;

namespace TwitchLib.EventSub.Core.EventArgs.Channel
{
    public class ChannelCharityCampaignDonateArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<ChannelCharityCampaignDonate>>
    { }
}