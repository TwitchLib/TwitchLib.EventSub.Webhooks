using TwitchLib.EventSub.Webhooks.Core.Models;
using TwitchLib.EventSub.Webhooks.Core.SubscriptionTypes.User;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs.User
{
    public class UserUpdateArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<UserUpdate>>
    { }
}