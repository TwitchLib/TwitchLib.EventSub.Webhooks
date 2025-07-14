using System.Text.Json;
using TwitchLib.EventSub.Webhooks.Core.Models;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs;

public class UnknownEventSubEventArgs : TwitchLibEventSubEventArgs<EventSubNotificationPayload<JsonElement>>
{ }
