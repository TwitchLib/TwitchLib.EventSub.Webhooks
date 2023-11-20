using System;
using Microsoft.Extensions.Logging;
using TwitchLib.EventSub.Webhooks.Middlewares;

namespace TwitchLib.EventSub.Webhooks.Extensions;

internal static partial class LogExtensions
{
    [LoggerMessage(LogLevel.Information, "EventSub notification request to {callbackPath} responded status code {statusCode} in {responseTime} ms")]
    public static partial void LogEventSubHttpNotification(this ILogger<EventSubNotificationLoggerMiddleware> logger, string callbackPath, int statusCode, double responseTime);
    
    [LoggerMessage(LogLevel.Error, "Exception occurred while calculating signature! {exceptionMessage}")]
    public static partial void LogSignatureVerificationException(this ILogger<EventSubSignatureVerificationMiddleware> logger, string exceptionMessage);
}