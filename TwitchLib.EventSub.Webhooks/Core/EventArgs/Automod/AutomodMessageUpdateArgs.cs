using TwitchLib.EventSub.Core.SubscriptionTypes.Automod;
using TwitchLib.EventSub.Webhooks.Core.Models;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs.Automod;

public class AutomodMessageUpdateArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<AutomodMessageUpdate>>
{ }
