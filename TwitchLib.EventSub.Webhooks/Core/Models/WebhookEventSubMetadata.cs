using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitchLib.EventSub.Webhooks.Core.Models;

public class WebhookEventSubMetadata
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
    /// Contains all headers that start with "Twitch-Eventsub-"
    /// </summary>
    public Dictionary<string, string> TwitchEventsubHeaders { get; set; }

    internal static WebhookEventSubMetadata CreateMetadata(IHeaderDictionary headers)
    {
        var twitchHeaders = headers.Where(h => h.Key.StartsWith("Twitch-Eventsub-", StringComparison.InvariantCultureIgnoreCase))
            .ToDictionary(h => h.Key, h => h.Value.ToString());

        return new WebhookEventSubMetadata()
        {
            MessageId = twitchHeaders["Twitch-Eventsub-Message-Id"],
            MessageRetry = twitchHeaders["Twitch-Eventsub-Message-Retry"],
            MessageType = twitchHeaders["Twitch-Eventsub-Message-Type"],
            MessageSignature = twitchHeaders["Twitch-Eventsub-Message-Signature"],
            MessageTimestamp = twitchHeaders["Twitch-Eventsub-Message-Timestamp"],
            SubscriptionType = twitchHeaders["Twitch-Eventsub-Subscription-Type"],
            SubscriptionVersion = twitchHeaders["Twitch-Eventsub-Subscription-Version"],
            TwitchEventsubHeaders = twitchHeaders
        };
    }
}
