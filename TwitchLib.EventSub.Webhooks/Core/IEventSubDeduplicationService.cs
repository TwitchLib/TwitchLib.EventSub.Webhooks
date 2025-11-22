namespace TwitchLib.EventSub.Webhooks.Core;

public interface IEventSubDeduplicationService
{
    bool IsDuplicateMessage(string messageId);
}
