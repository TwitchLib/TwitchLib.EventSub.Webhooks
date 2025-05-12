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

                return IsSignatureValid(providedSignature!, id!, timestamp!, await ReadRequestBodyContentAsync(request), _options.SecretBytes!);
            }
            catch (Exception ex)
            {
                _logger.LogSignatureVerificationException(ex.Message);
                return false;
            }
        }

        internal static bool IsSignatureValid(string messageSignature, string messageId, string messageTimestamp, string messageBody, byte[] secret)
        {
            var providedSignature = BytesFromHex(messageSignature.Split('=').ElementAtOrDefault(1)).ToArray();
            var computedSignature = CalculateSignature(Encoding.UTF8.GetBytes(messageId + messageTimestamp + messageBody), secret);

            return computedSignature.Zip(providedSignature, (a, b) => a == b).Aggregate(true, (a, r) => a && r);
        }

        private static Memory<byte> BytesFromHex(ReadOnlySpan<char> content)
        {
            if (content.IsEmpty || content.IsWhiteSpace())
            {
                return Memory<byte>.Empty;
            }

            try
            {
                var data = MemoryPool<byte>.Shared.Rent(content.Length / 2).Memory;
                var input = 0;
                for (var output = 0; output < data.Length; output++)
                {
                    data.Span[output] = Convert.ToByte(new string(new[] { content[input++], content[input++] }), 16);
                }

                return input != content.Length ? Memory<byte>.Empty : data;
            }
            catch (Exception exception) when (exception is ArgumentException or FormatException)
            {
                return Memory<byte>.Empty;
            }
        }

        private static byte[] CalculateSignature(byte[] payload, byte[] secret)
        {
            using var hmac = new HMACSHA256(secret);
            return hmac.ComputeHash(payload);
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

        private static async Task<string> ReadRequestBodyContentAsync(HttpRequest request)
        {
            await PrepareRequestBodyAsync(request);
            using var reader = new StreamReader(request.Body, Encoding.UTF8, false, leaveOpen: true);
            var requestBody = await reader.ReadToEndAsync();
            request.Body.Seek(0L, SeekOrigin.Begin);

            return requestBody;
        }

        private static async Task WriteResponseAsync(HttpContext context, int statusCode, string contentType, string responseBody)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = contentType;
            await context.Response.WriteAsync(responseBody);
        }
    }
}
#pragma warning restore 1591