using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TwitchLib.EventSub.Webhooks.Core;
using TwitchLib.EventSub.Webhooks.Core.Models;

#pragma warning disable 1591
namespace TwitchLib.EventSub.Webhooks.Middlewares
{
    public class EventSubNotificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEventSubWebhooks _eventSubWebhooks;

        public EventSubNotificationMiddleware(RequestDelegate next, IEventSubWebhooks eventSubWebhooks)
        {
            _next = next;
            _eventSubWebhooks = eventSubWebhooks;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // For now, we ignore the return value.
            WebhookEventSubMetadata.TryCreateMetadata(context.Request.Headers, out var metadata);

            switch (metadata.MessageType)
            {
                case "webhook_callback_verification":
                    var json = await JsonDocument.ParseAsync(context.Request.Body);
                    string challenge = json.RootElement.GetProperty("challenge"u8).GetString()!;
                    await WriteResponseAsync(context, 200, "text/plain", challenge!);
                    return;
                case "notification":
                    await _eventSubWebhooks.ProcessNotificationAsync(metadata, context.Request.Body);
                    await WriteResponseAsync(context, 200, "text/plain", "Thanks for the heads up Jordan");
                    return;
                case "revocation":
                    await _eventSubWebhooks.ProcessRevocationAsync(metadata, context.Request.Body);
                    await WriteResponseAsync(context, 200, "text/plain", "Thanks for the heads up Jordan");
                    return;
                default:
                    await WriteResponseAsync(context, 400, "text/plain", $"Unknown EventSub message type: {metadata.MessageType}");
                    return;
            }
        }

        private static Task WriteResponseAsync(HttpContext context, int statusCode, string contentType, string responseBody)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = contentType;
            return context.Response.WriteAsync(responseBody);
        }
    }
}
#pragma warning restore 1591
