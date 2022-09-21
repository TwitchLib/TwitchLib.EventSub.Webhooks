using TwitchLib.EventSub.Webhooks.Core.Models.Charity;

namespace TwitchLib.EventSub.Webhooks.Core.SubscriptionTypes.Channel
{
    /// <summary>
    /// Charity Campaign Donate subscription type model
    /// <para>Description:</para>
    /// <para>Sends an event notification when a user donates to the broadcaster’s charity campaign.</para>
    /// </summary>
    public class ChannelCharityCampaignDonate
    {
        /// <summary>
        /// An ID that uniquely identifies the charity campaign.
        /// </summary>
        public string CampaignId { get; set; } = string.Empty;
        /// <summary>
        /// An ID that uniquely identifies the broadcaster that’s running the campaign.
        /// </summary>
        public string BroadcasterId { get; set; } = string.Empty;
        /// <summary>
        /// The broadcaster’s login name.
        /// </summary>
        public string BroadcasterLogin { get; set; } = string.Empty;
        /// <summary>
        /// The broadcaster’s display name.
        /// </summary>
        public string BroadcasterName { get; set; } = string.Empty;
        /// <summary>
        /// An ID that uniquely identifies the user that donated to the campaign.
        /// </summary>
        public string UserId { get; set; } = string.Empty;
        /// <summary>
        /// The users’s login name.
        /// </summary>
        public string UserLogin { get; set; } = string.Empty;
        /// <summary>
        /// The users’s display name.
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// An object that contains the amount of the user’s donation.
        /// </summary>
        public CharityAmount Amount { get; set; } = new();

        /// <summary>
        /// The ISO-4217 three-letter currency code that identifies the type of currency in value.
        /// </summary>
        public string Currency { get; set; } = string.Empty;
    }
}