using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using TwitchLib.EventSub.Webhooks.Extensions;

#pragma warning disable 1591
namespace TwitchLib.EventSub.Webhooks.Middlewares
{
    public class EventSubNotificationLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<EventSubNotificationLoggerMiddleware> _logger;

        public EventSubNotificationLoggerMiddleware(RequestDelegate next, ILogger<EventSubNotificationLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTimestamp = Stopwatch.GetTimestamp();
            await _next(context).ConfigureAwait(false);
            var elapsed = Stopwatch.GetElapsedTime(startTimestamp);
            _logger.LogEventSubHttpNotification(context.Request.Path, context.Response.StatusCode, elapsed.TotalMilliseconds);
        }
    }
}
#pragma warning restore 1591