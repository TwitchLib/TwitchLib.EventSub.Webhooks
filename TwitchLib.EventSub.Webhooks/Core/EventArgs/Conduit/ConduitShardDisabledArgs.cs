using TwitchLib.EventSub.Core.SubscriptionTypes.Conduit;
using TwitchLib.EventSub.Webhooks.Core.Models;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs.Conduit;

public class ConduitShardDisabledArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<ConduitShardDisabled>>
{ }
