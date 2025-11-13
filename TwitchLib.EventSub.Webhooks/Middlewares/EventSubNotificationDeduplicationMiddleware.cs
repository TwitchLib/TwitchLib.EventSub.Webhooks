using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net.Mime;
using System.Threading.Tasks;
using TwitchLib.EventSub.Webhooks.Core;
using TwitchLib.EventSub.Webhooks.Extensions;

namespace TwitchLib.EventSub.Webhooks.Middlewares;

public class EventSubNotificationDeduplicationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IEventSubDeduplicationService _deduplicationService;
    private readonly ILogger<EventSubNotificationDeduplicationMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSubNotificationDeduplicationMiddleware"/> class.
    /// </summary>
    public EventSubNotificationDeduplicationMiddleware(RequestDelegate next, IEventSubDeduplicationService deduplicationService, ILogger<EventSubNotificationDeduplicationMiddleware> logger)
    {
        _next = next;
        _deduplicationService = deduplicationService;
        _logger = logger;
    }

    /// <summary>
    /// Request handling method.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
    /// <returns>A <see cref="Task"/> that represents the execution of this middleware.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var messageId = context.Request.Headers["Twitch-Eventsub-Message-Id"].ToString();

        if (_deduplicationService.IsDuplicateMessage(messageId))
        {
            _logger.LogDuplicateMessage(messageId);
            await WriteResponseAsync(context, 200, MediaTypeNames.Text.Plain, "Thanks for the heads up Jordan").ConfigureAwait(false);
            return;
        }
        await _next(context);
    }

    private static Task WriteResponseAsync(HttpContext context, int statusCode, string contentType, string responseBody)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = contentType;
        return context.Response.WriteAsync(responseBody);
    }
}
