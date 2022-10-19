using TwitchLib.EventSub.Webhooks.Core.Models;
using TwitchLib.EventSub.Core.SubscriptionTypes.Drop;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs.Drop
{
    public class DropEntitlementGrantArgs : TwitchLibEventSubEventArgs<BatchedNotificationPayload<DropEntitlementGrant>>
    { }
}