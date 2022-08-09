using System;

namespace TwitchLib.EventSub.Webhooks.Core.SubscriptionTypes.Channel
{
    /// <summary>
    /// Channel Ban subscription type model
    /// <para>Description:</para>
    /// <para>A viewer is banned/timed out from the specified channel.</para>
    /// </summary>
    public class ChannelBan
    {
        /// <summary>
        /// The user ID for the user who was banned/timed out on the specified channel.
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// The user display name for the user who was banned/timed out on the specified channel.
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// The user login for the user who was banned/timed out on the specified channel.
        /// </summary>
        public string UserLogin { get; set; } = string.Empty;
        /// <summary>
        /// The requested broadcaster ID.
        /// </summary>
        public string BroadcasterUserId { get; set; } = string.Empty;
        /// <summary>
        /// The requested broadcaster display name.
        /// </summary>
        public string BroadcasterUserName { get; set; } = string.Empty;
        /// <summary>
        /// The requested broadcaster login.
        /// </summary>
        public string BroadcasterUserLogin { get; set; } = string.Empty;
        /// <summary>
        /// The user ID of the issuer of the ban/timeout.
        /// </summary>
        public string ModeratorUserId { get; set; } = string.Empty;
        /// <summary>
        /// The user name of the issuer of the ban/timeout.
        /// </summary>
        public string ModeratorUserName { get; set; } = string.Empty;
        /// <summary>
        /// The user login of the issuer of the ban/timeout.
        /// </summary>
        public string ModeratorUserLogin { get; set; } = string.Empty;
        /// <summary>
        /// The reason behind the ban.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// The UTC date and time (in RFC3339 format) of when the user was banned or put in a timeout.
        /// </summary>
        public DateTime BannedAt { get; set; } = DateTime.MinValue;
        /// <summary>
        /// The UTC date and time (in RFC3339 format) of when the timeout ends. Is null if the user was banned instead of put in a timeout.
        /// </summary>
        public DateTime? EndsAt { get; set; }
        /// <summary>
        /// Indicates whether the ban is permanent (true) or a timeout (false). If true, EndsAt will be null.
        /// </summary>
        public bool IsPermanent { get; set; }
    }
}