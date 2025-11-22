using System.IO;
using System.Threading.Tasks;
using TwitchLib.EventSub.Core;
using TwitchLib.EventSub.Core.EventArgs.Automod;
using TwitchLib.EventSub.Core.EventArgs.Channel;
using TwitchLib.EventSub.Core.EventArgs.Conduit;
using TwitchLib.EventSub.Core.EventArgs.Drop;
using TwitchLib.EventSub.Core.EventArgs.Extension;
using TwitchLib.EventSub.Core.EventArgs.Stream;
using TwitchLib.EventSub.Core.EventArgs.User;
using TwitchLib.EventSub.Webhooks.Core.EventArgs;
using TwitchLib.EventSub.Webhooks.Core.Models;

namespace TwitchLib.EventSub.Webhooks.Core
{
    /// <summary>
    /// Class where everything runs together
    /// <para>Listen to events from EventSub from this class</para>
    /// </summary>
    public interface IEventSubWebhooks
    {
        /// <summary>
        /// Event that triggers if an error parsing a notification or revocation was encountered
        /// </summary>
        event AsyncEventHandler<OnErrorArgs>? Error;
        /// <summary>
        /// Event that triggers on if a revocation notification was received
        /// </summary>
        event AsyncEventHandler<RevocationArgs>? Revocation;
        /// <summary>
        /// Event that triggers when EventSub send notification, that's unknown. (ie.: not implementet ... yet!)
        /// </summary>
        event AsyncEventHandler<UnknownEventSubNotificationArgs>? UnknownEventSubNotification;
        /// <summary>
        /// Event that triggers on "automod.message.hold" notifications
        /// </summary>
        event AsyncEventHandler<AutomodMessageHoldArgs>? AutomodMessageHold;
        /// <summary>
        /// Event that triggers on "automod.message.hold" notifications
        /// </summary>
        event AsyncEventHandler<AutomodMessageHoldV2Args>? AutomodMessageHoldV2;
        /// <summary>
        /// Event that triggers on "automod.message.update" notifications
        /// </summary>
        event AsyncEventHandler<AutomodMessageUpdateArgs>? AutomodMessageUpdate;
        /// <summary>
        /// Event that triggers on "automod.message.update" notifications
        /// </summary>
        event AsyncEventHandler<AutomodMessageUpdateV2Args>? AutomodMessageUpdateV2;
        /// <summary>
        /// Event that triggers on "automod.settings.update" notifications
        /// </summary>
        event AsyncEventHandler<AutomodSettingsUpdateArgs>? AutomodSettingsUpdate;
        /// <summary>
        /// Event that triggers on "automod.terms.update" notifications
        /// </summary>
        event AsyncEventHandler<AutomodTermsUpdateArgs>? AutomodTermsUpdate;
        /// <summary>
        /// Event that triggers on "channel.bits.use" notifications
        /// </summary>
        event AsyncEventHandler<ChannelBitsUseArgs>? ChannelBitsUse;
        /// <summary>
        /// Event that triggers on "channel.ad_break.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelAdBreakBeginArgs>? ChannelAdBreakBegin;
        /// <summary>
        /// Event that triggers on "channel.ban" notifications
        /// </summary>
        event AsyncEventHandler<ChannelBanArgs>? ChannelBan;
        /// <summary>
        /// Event that triggers on "channel.cheer" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCheerArgs>? ChannelCheer;
        /// <summary>
        /// Event that triggers on "channel.charity_campaign.start" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCharityCampaignStartArgs>? ChannelCharityCampaignStart;
        /// <summary>
        /// Event that triggers on "channel.charity_campaign.donate" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCharityCampaignDonateArgs>? ChannelCharityCampaignDonate;
        /// <summary>
        /// Event that triggers on "channel.charity_campaign.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCharityCampaignProgressArgs>? ChannelCharityCampaignProgress;
        /// <summary>
        /// Event that triggers on "channel.charity_campaign.stop" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCharityCampaignStopArgs>? ChannelCharityCampaignStop;
        /// <summary>
        /// Event that triggers on "channel.follow" notifications
        /// </summary>
        event AsyncEventHandler<ChannelFollowArgs>? ChannelFollow;
        /// <summary>
        /// Event that triggers on "channel.goal.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalBeginArgs>? ChannelGoalBegin;
        /// <summary>
        /// Event that triggers on "channel.goal.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalEndArgs>? ChannelGoalEnd;
        /// <summary>
        /// Event that triggers on "channel.goal.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalProgressArgs>? ChannelGoalProgress;
        /// <summary>
        /// Event that triggers on "channel.hype_train.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainBeginV2Args>? ChannelHypeTrainBeginV2;
        /// <summary>
        /// Event that triggers on "channel.hype_train.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainEndV2Args>? ChannelHypeTrainEndV2;
        /// <summary>
        /// Event that triggers on "channel.hype_train.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainProgressV2Args>? ChannelHypeTrainProgressV2;
        /// <summary>
        /// Event that triggers on "channel.unban_request.create" notifications
        /// </summary>
        event AsyncEventHandler<ChannelUnbanRequestCreateArgs>? ChannelUnbanRequestCreate;
        /// <summary>
        /// Event that triggers on "channel.unban_request.resolve" notifications
        /// </summary>
        event AsyncEventHandler<ChannelUnbanRequestResolveArgs>? ChannelUnbanRequestResolve;
        /// <summary>
        /// Event that triggers on "channel.moderate" notifications
        /// </summary>
        event AsyncEventHandler<ChannelModerateArgs>? ChannelModerate;
        /// <summary>
        /// Event that triggers on "channel.moderate" notifications
        /// </summary>
        event AsyncEventHandler<ChannelModerateV2Args>? ChannelModerateV2;
        /// <summary>
        /// Event that triggers on "channel.moderator.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelModeratorArgs>? ChannelModeratorAdd;
        /// <summary>
        /// Event that triggers on "channel.moderator.remove" notifications
        /// </summary>
        event AsyncEventHandler<ChannelModeratorArgs>? ChannelModeratorRemove;
        /// <summary>
        /// Event that triggers on "channel.guest_star_session.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGuestStarSessionBeginArgs>? ChannelGuestStarSessionBegin;
        /// <summary>
        /// Event that triggers on "channel.guest_star_guest.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGuestStarSessionEndArgs>? ChannelGuestStarSessionEnd;
        /// <summary>
        /// Event that triggers on "channel.guest_star_guest.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGuestStarGuestUpdateArgs>? ChannelGuestStarGuestUpdate;
        /// <summary>
        /// Event that triggers on "channel.guest_star_settings.update" notifications
        /// </summary>
        public event AsyncEventHandler<ChannelGuestStarSettingsUpdateArgs>? ChannelGuestStarSettingsUpdate;
        /// <summary>
        /// Event that triggers on "channel.channel_points_automatic_reward_redemption.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsAutomaticRewardRedemptionAddV2Args>? ChannelPointsAutomaticRewardRedemptionAddV2;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? ChannelPointsCustomRewardAdd;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? ChannelPointsCustomRewardUpdate;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.remove" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? ChannelPointsCustomRewardRemove;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward_redemption.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? ChannelPointsCustomRewardRedemptionAdd;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward_redemption.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? ChannelPointsCustomRewardRedemptionUpdate;
        /// <summary>
        /// Event that triggers on "channel.poll.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollBeginArgs>? ChannelPollBegin;
        /// <summary>
        /// Event that triggers on "channel.poll.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollEndArgs>? ChannelPollEnd;
        /// <summary>
        /// Event that triggers on "channel.poll.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollProgressArgs>? ChannelPollProgress;
        /// <summary>
        /// Event that triggers on "channel.prediction.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionBeginArgs>? ChannelPredictionBegin;
        /// <summary>
        /// Event that triggers on "channel.prediction.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionEndArgs>? ChannelPredictionEnd;
        /// <summary>
        /// Event that triggers on "channel.prediction.lock" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionLockArgs>? ChannelPredictionLock;
        /// <summary>
        /// Event that triggers on "channel.prediction.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionProgressArgs>? ChannelPredictionProgress;
        /// <summary>
        /// Event that triggers on "channel.suspicious_user.message" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSuspiciousUserMessageArgs>? ChannelSuspiciousUserMessage;
        /// <summary>
        /// Event that triggers on "channel.suspicious_user.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSuspiciousUserUpdateArgs>? ChannelSuspiciousUserUpdate;
        /// <summary>
        /// Event that triggers on "channel.vip.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelVipArgs>? ChannelVipAdd;
        /// <summary>
        /// Event that triggers on "channel.vip.remove" notifications
        /// </summary>
        event AsyncEventHandler<ChannelVipArgs>? ChannelVipRemove;
        /// <summary>
        /// Event that triggers on "channel.warning.acknowledge" notifications
        /// </summary>
        event AsyncEventHandler<ChannelWarningAcknowledgeArgs>? ChannelWarningAcknowledge;
        /// <summary>
        /// Event that triggers on "channel.warning.send" notifications
        /// </summary>
        event AsyncEventHandler<ChannelWarningSendArgs>? ChannelWarningSend;
        /// <summary>
        /// Event that triggers on "channel.raid" notifications
        /// </summary>
        event AsyncEventHandler<ChannelRaidArgs>? ChannelRaid;
        /// <summary>
        /// Event that triggers on "channel.shield_mode.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelShieldModeBeginArgs>? ChannelShieldModeBegin;
        /// <summary>
        /// Event that triggers on "channel.shield_mode.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelShieldModeEndArgs>? ChannelShieldModeEnd;
        /// <summary>
        /// Event that triggers on "channel.shoutout.create" notifications
        /// </summary>
        event AsyncEventHandler<ChannelShoutoutCreateArgs>? ChannelShoutoutCreate;
        /// <summary>
        /// Event that triggers on "channel.shoutout.receive" notifications
        /// </summary>
        event AsyncEventHandler<ChannelShoutoutReceiveArgs>? ChannelShoutoutReceive;
        /// <summary>
        /// Event that triggers on "channel.shared_chat.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSharedChatSessionBeginArgs>? ChannelSharedChatSessionBegin;
        /// <summary>
        /// Event that triggers on "channel.shared_chat.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSharedChatSessionUpdateArgs>? ChannelSharedChatSessionUpdate;
        /// <summary>
        /// Event that triggers on "channel.shared_chat.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSharedChatSessionEndArgs>? ChannelSharedChatSessionEnd;
        /// <summary>
        /// Event that triggers on "channel.subscribe" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscribeArgs>? ChannelSubscribe;
        /// <summary>
        /// Event that triggers on "channel.subscription.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionEndArgs>? ChannelSubscriptionEnd;
        /// <summary>
        /// Event that triggers on "channel.subscription.gift" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionGiftArgs>? ChannelSubscriptionGift;
        /// <summary>
        /// Event that triggers on "channel.subscription.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionMessageArgs>? ChannelSubscriptionMessage;
        /// <summary>
        /// Event that triggers on "channel.unban" notifications
        /// </summary>
        event AsyncEventHandler<ChannelUnbanArgs>? ChannelUnban;
        /// <summary>
        /// Event that triggers on "channel.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelUpdateArgs>? ChannelUpdate;
        /// <summary>
        /// Event that triggers on "conduit.shard.disabled" notifications
        /// </summary>
        event AsyncEventHandler<ConduitShardDisabledArgs>? ConduitShardDisabled;
        /// <summary>
        /// Event that triggers on "drop.entitlement.grant" notifications
        /// </summary>
        event AsyncEventHandler<DropEntitlementGrantArgs>? DropEntitlementGrant;
        /// <summary>
        /// Event that triggers on "extension.bits_transaction.create" notifications
        /// </summary>
        event AsyncEventHandler<ExtensionBitsTransactionCreateArgs>? ExtensionBitsTransactionCreate;
        /// <summary>
        /// Event that triggers on "stream.offline" notifications
        /// </summary>
        event AsyncEventHandler<StreamOfflineArgs>? StreamOffline;
        /// <summary>
        /// Event that triggers on "stream.online" notifications
        /// </summary>
        event AsyncEventHandler<StreamOnlineArgs>? StreamOnline;
        /// <summary>
        /// Event that triggers on "user.authorization.grant" notifications
        /// </summary>
        event AsyncEventHandler<UserAuthorizationGrantArgs>? UserAuthorizationGrant;
        /// <summary>
        /// Event that triggers on "user.authorization.revoke" notifications
        /// </summary>
        event AsyncEventHandler<UserAuthorizationRevokeArgs>? UserAuthorizationRevoke;
        /// <summary>
        /// Event that triggers on "user.update" notifications
        /// </summary>
        event AsyncEventHandler<UserUpdateArgs>? UserUpdate;
        /// <summary>
        /// Event that triggers on "channel.chat.clear" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatClearArgs>? ChannelChatClear;
        /// <summary>
        /// Event that triggers on "channel.chat.clear_user_messages" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatClearUserMessagesArgs>? ChannelChatClearUserMessages;
        /// <summary>
        /// Event that triggers on "channel.chat.message" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatMessageArgs>? ChannelChatMessage;
        /// <summary>
        /// Event that triggers on "channel.chat.message_delete" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatMessageDeleteArgs>? ChannelChatMessageDelete;
        /// <summary>
        /// Event that triggers on "channel.chat.notification" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatNotificationArgs>? ChannelChatNotification;
        /// <summary>
        /// Event that triggers on "channel.chat_settings.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatSettingsUpdateArgs>? ChannelChatSettingsUpdate;
        /// <summary>
        /// Event that triggers on "channel.chat.user_message_hold" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatUserMessageHoldArgs>? ChannelChatUserMessageHold;
        /// <summary>
        /// Event that triggers on "channel.chat.user_message_update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatUserMessageUpdateArgs>? ChannelChatUserMessageUpdate;


        /// <summary>
        /// Processes "notification" type messages. You should not use this in your code, its for internal use only!
        /// </summary>
        /// <param name="metadata">Metadata reveived from the request headers</param>
        /// <param name="body">Stream of the request body</param>
        Task ProcessNotificationAsync(WebhookEventSubMetadata metadata, Stream body);
        /// <summary>
        /// Processes "revocation" type messages. You should not use this in your code, its for internal use only!
        /// </summary>
        /// <param name="metadata">Metadata reveived from the request headers</param>
        /// <param name="body">Stream of the request body</param>
        Task ProcessRevocationAsync(WebhookEventSubMetadata metadata, Stream body);
    }
}
