using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.EventSub.Webhooks.Core;

namespace TwitchLib.EventSub.Webhooks.Services;

/// <inheritdoc />
public sealed class EventSubDeduplicationService : IEventSubDeduplicationService, IDisposable
{
    static internal readonly TimeSpan MessageTtl = TimeSpan.FromMinutes(2);
    internal readonly ConcurrentDictionary<string, DateTimeOffset> Messages = new();
    private readonly PeriodicTimer _timer;
    private readonly TimeProvider _timeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSubDeduplicationService"/> class.
    /// </summary>
    public EventSubDeduplicationService(TimeProvider? timeProvider = null)
    {
        _timeProvider = timeProvider ?? TimeProvider.System;
        _timer = new PeriodicTimer(TimeSpan.FromMinutes(1), _timeProvider);
        _ = RemoveOldMessagesAsyncLoop();
    }

    /// <inheritdoc />
    public bool IsDuplicateMessage(string messageId)
    {
        return !Messages.TryAdd(messageId, _timeProvider.GetUtcNow());
    }

    private async Task RemoveOldMessagesAsyncLoop()
    {
        while (await _timer.WaitForNextTickAsync())
        {
            var time = _timeProvider.GetUtcNow() - MessageTtl;
            foreach (var item in Messages)
            {
                if (item.Value <= time)
                {
                    Messages.TryRemove(item);
                }
            }
        }
    }

    void IDisposable.Dispose()
    {
        _timer.Dispose();
    }
}
