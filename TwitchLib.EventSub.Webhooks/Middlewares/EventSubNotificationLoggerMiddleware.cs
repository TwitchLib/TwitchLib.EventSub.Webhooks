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
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();
            _logger.LogEventSubHttpNotification(context.Request.Path, context.Response.StatusCode, stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
#pragma warning restore 1591