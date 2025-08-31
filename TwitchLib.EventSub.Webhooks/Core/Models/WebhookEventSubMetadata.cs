using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.EventSub.Core.Models;

namespace TwitchLib.EventSub.Webhooks.Core.Models;

public class WebhookEventSubMetadata : EventSubMetadata
{
    /// <summary>
    /// An ID that uniquely identifies this message.
    /// </summary>
    public string MessageId { get; set; }

    public string MessageRetry { get; set; }

    /// <summary>
    /// The type of notification.
    /// </summary>
    /// <remarks>Possible values are:
    /// <para>notification — Contains the event's data.</para>
    /// <para>webhook_callback_verification — Contains the challenge used to verify that you own the event handler.</para>
    /// <para>revocation — Contains the reason why Twitch revoked your subscription.</para>
    /// </remarks>
    public string MessageType { get; set; }

    /// <summary>
    /// The HMAC signature that you use to verify that Twitch sent the message.
    /// </summary>
    public string MessageSignature { get; set; }

    /// <summary>
    /// The UTC date and time (in RFC3339 format) that Twitch sent the notification.
    /// </summary>
    public string MessageTimestamp { get; set; }

    /// <summary>
    /// The subscription type.
    /// </summary>
    public string SubscriptionType { get; set; }

    /// <summary>
    /// The subscription version.
    /// </summary>
    public string SubscriptionVersion { get; set; }

    /// <summary>
    /// Contains all headers that start with "twitch-eventsub-"
    /// </summary>
    public Dictionary<string, string> TwitchEventsubHeaders { get; set; }

    internal static bool TryCreateMetadata(IHeaderDictionary headers, out WebhookEventSubMetadata metadata)
    {
        var twitchHeaders = headers.Where(h => h.Key.StartsWith("Twitch-Eventsub-", StringComparison.InvariantCultureIgnoreCase))
            .ToDictionary(h => h.Key, h => h.Value.ToString(), StringComparer.InvariantCultureIgnoreCase);
        var containsRequiredHeaders = true;

        metadata = new WebhookEventSubMetadata()
        {
            MessageId = GetHeaderValue("Twitch-Eventsub-Message-Id"),
            MessageRetry = GetHeaderValue("Twitch-Eventsub-Message-Retry"),
            MessageType = GetHeaderValue("Twitch-Eventsub-Message-Type"),
            MessageSignature = GetHeaderValue("Twitch-Eventsub-Message-Signature"),
            MessageTimestamp = GetHeaderValue("Twitch-Eventsub-Message-Timestamp"),
            SubscriptionType = GetHeaderValue("Twitch-Eventsub-Subscription-Type"),
            SubscriptionVersion = GetHeaderValue("Twitch-Eventsub-Subscription-Version"),
            TwitchEventsubHeaders = twitchHeaders
        };

        return containsRequiredHeaders;

        string GetHeaderValue(string key)
        {
            containsRequiredHeaders &= twitchHeaders.TryGetValue(key, out var value);
            return value!;
        }
    }
}
