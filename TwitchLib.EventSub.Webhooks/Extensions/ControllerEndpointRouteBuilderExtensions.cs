using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using TwitchLib.EventSub.Webhooks.Core;
using TwitchLib.EventSub.Webhooks.Core.Models;
using TwitchLib.EventSub.Webhooks.Middlewares;

namespace TwitchLib.EventSub.Webhooks.Extensions;

public static class TwitchLibEventSubEndpointRouteBuilderExtensions
{
    public static void MapTwitchLibEventSub(this IEndpointRouteBuilder endpoints)
    {
        var options = endpoints.ServiceProvider.GetRequiredService<IOptions<TwitchLibEventSubOptions>>();
        endpoints.MapPost(options.Value.CallbackPath, WebhookDelegate);
    }

    public static async Task WebhookDelegate(HttpContext context) // better method name
    {
        var startTimestamp = Stopwatch.GetTimestamp();
        var loggerFactory = context.RequestServices.GetRequiredService<ILoggerFactory>();
        var options = context.RequestServices.GetRequiredService<IOptions<TwitchLibEventSubOptions>>();

        using var ms = new MemoryStream(); // RecyclableMemoryStream?
        await context.Request.Body.CopyToAsync(ms);
        var body = ms.GetBuffer().AsMemory(0, (int)ms.Position);
        _ = WebhookEventSubMetadata.TryCreateMetadata(context.Request.Headers, out var metadata);

        if (!EventSubSignatureVerificationMiddleware.IsSignatureValid(metadata.MessageSignature, metadata.MessageId, metadata.MessageTimestamp, body.Span, options.Value.SecretBytes))
        {
            await WriteResponseAsync(context, 403, "Invalid Signature");
            return;
        }

        var deduplicationService = context.RequestServices.GetRequiredService<IEventSubDeduplicationService>();
        if (deduplicationService.IsDuplicateMessage(metadata.MessageId))
        {
            loggerFactory.CreateLogger<EventSubNotificationDeduplicationMiddleware>().LogDuplicateMessage(metadata.MessageId);
            await WriteResponseAsync(context, 200, "Thanks for the heads up Jordan");
            return;
        }

        var eventSubWebhooks = context.RequestServices.GetRequiredService<IEventSubWebhooks>();
        switch (metadata.MessageType)
        {
            case "webhook_callback_verification":
                var json = JsonDocument.Parse(body);
                string challenge = json.RootElement.GetProperty("challenge"u8).GetString()!;
                await WriteResponseAsync(context, 200, challenge!);
                break;
            case "notification":
                await eventSubWebhooks.ProcessNotificationAsync(metadata, context.Request.Body);
                await WriteResponseAsync(context, 200, "Thanks for the heads up Jordan");
                break;
            case "revocation":
                await eventSubWebhooks.ProcessRevocationAsync(metadata, context.Request.Body);
                await WriteResponseAsync(context, 200, "Thanks for the heads up Jordan");
                break;
            default:
                await WriteResponseAsync(context, 400, $"Unknown EventSub message type: {metadata.MessageType}");
                break;
        }
        var elapsed = Stopwatch.GetElapsedTime(startTimestamp);
        loggerFactory.CreateLogger<EventSubNotificationLoggerMiddleware>().LogEventSubHttpNotification(context.Request.Path, context.Response.StatusCode, elapsed.TotalMilliseconds);
    }

    private static Task WriteResponseAsync(HttpContext context, int statusCode, string responseBody)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = MediaTypeNames.Text.Plain;
        return context.Response.WriteAsync(responseBody);
    }
}
