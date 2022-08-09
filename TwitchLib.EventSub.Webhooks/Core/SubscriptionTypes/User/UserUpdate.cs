namespace TwitchLib.EventSub.Webhooks.Core.SubscriptionTypes.User
{
    /// <summary>
    /// User Update subscription type model
    /// <para>Description:</para>
    /// <para>A user has updated their account.</para>
    /// </summary>
    public class UserUpdate
    {
        /// <summary>
        /// The user’s user id.
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// The user's user display name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// The user's user login.
        /// </summary>
        public string UserLogin { get; set; } = string.Empty;
        /// <summary>
        /// The user’s email address. The event includes the user’s email address only if the app used to request this event type includes the user:read:email scope for the user; otherwise, the field is set to an empty string. 
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// <para>A Boolean value that determines whether Twitch has verified the user’s email address. Is true if Twitch has verified the email address; otherwise, false.</para>
        /// <para>NOTE: Ignore this field if the email field contains an empty string.</para>
        /// </summary>
        public bool EmailVerified { get; set; }
        /// <summary>
        /// The user's description 
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}