using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TwitchLib.EventSub.Webhooks.Core.Models;
using TwitchLib.EventSub.Webhooks.Extensions;

#pragma warning disable 1591
namespace TwitchLib.EventSub.Webhooks.Middlewares
{

    public class EventSubSignatureVerificationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<EventSubSignatureVerificationMiddleware> _logger;
        private readonly TwitchLibEventSubOptions _options;

        public EventSubSignatureVerificationMiddleware(RequestDelegate next, ILogger<EventSubSignatureVerificationMiddleware> logger, IOptions<TwitchLibEventSubOptions> options)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (await IsValidEventSubRequest(context.Request))
            {
                await _next(context);
                return;
            }

            await WriteResponseAsync(context, 403, "text/plain", "Invalid Signature");
        }

        private async Task<bool> IsValidEventSubRequest(HttpRequest request)
        {
            try
            {
                if (!request.Headers.TryGetValue("Twitch-Eventsub-Message-Signature", out var providedSignatureHeader))
                    return false;
                
                var providedSignature = providedSignatureHeader.First();
                
                if (!request.Headers.TryGetValue("Twitch-Eventsub-Message-Id", out var idHeader))
                    return false;

                var id = idHeader.First();

                if (!request.Headers.TryGetValue("Twitch-Eventsub-Message-Timestamp", out var timestampHeader))
                    return false;

                var timestamp = timestampHeader.First();

                var body = await ReadRequestBodyContentAsync(request);

                return IsSignatureValid(providedSignature!, id!, timestamp!, body.Span, _options.SecretBytes!);
            }
            catch (Exception ex)
            {
                _logger.LogSignatureVerificationException(ex.Message);
                return false;
            }
        }

        internal static bool IsSignatureValid(ReadOnlySpan<char> messageSignature, ReadOnlySpan<char> messageId, ReadOnlySpan<char> messageTimestamp, ReadOnlySpan<byte> messageBody, ReadOnlySpan<byte> secret)
        {
            var messageCharSpan = GetSpanFromArrayPool<char>(messageId.Length + messageTimestamp.Length, out var messageCharArray);
            messageCharSpan.TryWrite($"{messageId}{messageTimestamp}", out _);

            var messageByteSpan = GetSpanFromArrayPool<byte>(Encoding.UTF8.GetByteCount(messageCharSpan) + messageBody.Length, out var messageByteArray);
            var written = Encoding.UTF8.GetBytes(messageCharSpan, messageByteSpan);
            messageBody.CopyTo(messageByteSpan.Slice(written));
            ArrayPool<char>.Shared.Return(messageCharArray);

            Span<byte> computedSignature = stackalloc byte[HMACSHA256.HashSizeInBytes];
            HMACSHA256.HashData(secret, messageByteSpan, computedSignature);
            ArrayPool<byte>.Shared.Return(messageByteArray);

            ReadOnlySpan<char> sha256Prefix = "sha256=";
            if (messageSignature.StartsWith(sha256Prefix))
                messageSignature = messageSignature.Slice(sha256Prefix.Length);
#if NET9_0_OR_GREATER
            Span<byte> providedSignature = stackalloc byte[HMACSHA256.HashSizeInBytes];
            Convert.FromHexString(messageSignature, providedSignature, out _, out _);
#else
            var providedSignature = Convert.FromHexString(messageSignature).AsSpan();
#endif

            return providedSignature.SequenceEqual(computedSignature);
        }

        static Span<T> GetSpanFromArrayPool<T>(int length, out T[] array)
        {
            array = ArrayPool<T>.Shared.Rent(length);
            return array.AsSpan(0, length);
        }

        private static async Task PrepareRequestBodyAsync(HttpRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (!request.Body.CanSeek)
            {
                request.EnableBuffering();
                await request.Body.DrainAsync(CancellationToken.None);
            }

            request.Body.Seek(0L, SeekOrigin.Begin);
        }

        private static async Task<ReadOnlyMemory<byte>> ReadRequestBodyContentAsync(HttpRequest request)
        {
            await PrepareRequestBodyAsync(request);
            using var memoryStream = new MemoryStream();
            await request.Body.CopyToAsync(memoryStream);
            request.Body.Seek(0L, SeekOrigin.Begin);

            return memoryStream.GetBuffer().AsMemory(0, (int)memoryStream.Position);
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