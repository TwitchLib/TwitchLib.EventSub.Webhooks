using System.Text.Json;
using TwitchLib.EventSub.Core.EventArgs;

namespace TwitchLib.EventSub.Webhooks.Core.EventArgs;

public class UnknownEventSubNotificationArgs : TwitchLibEventSubNotificationArgs<JsonElement>
{ }
