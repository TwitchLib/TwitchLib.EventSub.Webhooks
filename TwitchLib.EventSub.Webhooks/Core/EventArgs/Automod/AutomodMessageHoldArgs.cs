using TwitchLib.EventSub.Core.SubscriptionTypes.Automod;
using TwitchLib.EventSub.Webhooks.Core.Models;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs.Automod;

public class AutomodMessageHoldArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<AutomodMessageHold>>
{ }
