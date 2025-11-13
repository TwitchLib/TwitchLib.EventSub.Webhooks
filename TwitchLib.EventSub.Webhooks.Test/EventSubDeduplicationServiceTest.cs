using Microsoft.Extensions.Time.Testing;
using TwitchLib.EventSub.Webhooks.Services;

namespace TwitchLib.EventSub.Webhooks.Test;

public class EventSubDeduplicationServiceTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(20)]
    public void AddDifferentMessages(int count)
    {
        var service = new EventSubDeduplicationService(null);
        for (int i = 0; i < count; i++)
        {
            var messageId = Guid.NewGuid().ToString();
            service.IsDuplicateMessage(messageId);
        }

        Assert.Equal(count, service.Messages.Count);
    }

    [Fact]
    public void AddDuplicateMessageFail()
    {
        var service = new EventSubDeduplicationService(null);
        var messageId = Guid.NewGuid().ToString();

        Assert.False(service.IsDuplicateMessage(messageId));
        Assert.True(service.IsDuplicateMessage(messageId));
    }

    [Fact]
    public void AddDuplicateMessageAfterPreviousExpires()
    {
        var timeProvider = new FakeTimeProvider();
        var service = new EventSubDeduplicationService(timeProvider);
        var messageId = Guid.NewGuid().ToString();
        var messageTtl = EventSubDeduplicationService.MessageTtl;

        service.IsDuplicateMessage(messageId);
        service.IsDuplicateMessage(messageId);
        timeProvider.Advance(messageTtl);

        Assert.Equal(0, service.Messages.Count);

        service.IsDuplicateMessage(messageId);

        Assert.Equal(1, service.Messages.Count);
    }
}
