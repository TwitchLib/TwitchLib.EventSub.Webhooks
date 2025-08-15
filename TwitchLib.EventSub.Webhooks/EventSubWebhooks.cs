using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TwitchLib.EventSub.Core;
using TwitchLib.EventSub.Core.EventArgs;
using TwitchLib.EventSub.Core.EventArgs.Automod;
using TwitchLib.EventSub.Core.EventArgs.Channel;
using TwitchLib.EventSub.Core.EventArgs.Conduit;
using TwitchLib.EventSub.Core.EventArgs.Drop;
using TwitchLib.EventSub.Core.EventArgs.Extension;
using TwitchLib.EventSub.Core.EventArgs.Stream;
using TwitchLib.EventSub.Core.EventArgs.User;
using TwitchLib.EventSub.Core.Extensions;
using TwitchLib.EventSub.Core.Models;
using TwitchLib.EventSub.Core.SubscriptionTypes.Automod;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Core.SubscriptionTypes.Conduit;
using TwitchLib.EventSub.Core.SubscriptionTypes.Drop;
using TwitchLib.EventSub.Core.SubscriptionTypes.Extension;
using TwitchLib.EventSub.Core.SubscriptionTypes.Stream;
using TwitchLib.EventSub.Core.SubscriptionTypes.User;
using TwitchLib.EventSub.Webhooks.Core;
using TwitchLib.EventSub.Webhooks.Core.EventArgs;
using TwitchLib.EventSub.Webhooks.Core.Models;

namespace TwitchLib.EventSub.Webhooks
{
    /// <inheritdoc/>
    /// <summary>
    /// <para>Implements <see cref="IEventSubWebhooks"/></para>
    /// </summary>
    public class EventSubWebhooks : IEventSubWebhooks
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        /// <inheritdoc/>
        public event AsyncEventHandler<OnErrorArgs>? Error;
        /// <inheritdoc/>
        public event AsyncEventHandler<RevocationArgs>? Revocation;
        /// <inheritdoc/>
        public event AsyncEventHandler<UnknownEventSubNotificationArgs>? UnknownEventSubNotification;
        /// <inheritdoc/>
        public event AsyncEventHandler<AutomodMessageHoldArgs>? AutomodMessageHold;
        /// <inheritdoc/>
        public event AsyncEventHandler<AutomodMessageHoldV2Args>? AutomodMessageHoldV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<AutomodMessageUpdateArgs>? AutomodMessageUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<AutomodMessageUpdateV2Args>? AutomodMessageUpdateV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<AutomodSettingsUpdateArgs>? AutomodSettingsUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<AutomodTermsUpdateArgs>? AutomodTermsUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelBitsUseArgs>? ChannelBitsUse;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelAdBreakBeginArgs>? ChannelAdBreakBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelBanArgs>? ChannelBan;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCheerArgs>? ChannelCheer;

        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCharityCampaignStartArgs>? ChannelCharityCampaignStart;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCharityCampaignDonateArgs>? ChannelCharityCampaignDonate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCharityCampaignProgressArgs>? ChannelCharityCampaignProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCharityCampaignStopArgs>? ChannelCharityCampaignStop;

        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelFollowArgs>? ChannelFollow;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalBeginArgs>? ChannelGoalBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalEndArgs>? ChannelGoalEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalProgressArgs>? ChannelGoalProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainBeginV2Args>? ChannelHypeTrainBeginV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainEndV2Args>? ChannelHypeTrainEndV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainProgressV2Args>? ChannelHypeTrainProgressV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelUnbanRequestCreateArgs>? ChannelUnbanRequestCreate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelUnbanRequestResolveArgs>? ChannelUnbanRequestResolve;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModerateArgs>? ChannelModerate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModerateV2Args>? ChannelModerateV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModeratorArgs>? ChannelModeratorAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModeratorArgs>? ChannelModeratorRemove;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGuestStarSessionBeginArgs>? ChannelGuestStarSessionBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGuestStarSessionEndArgs>? ChannelGuestStarSessionEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGuestStarGuestUpdateArgs>? ChannelGuestStarGuestUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGuestStarSettingsUpdateArgs>? ChannelGuestStarSettingsUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsAutomaticRewardRedemptionAddV2Args>? ChannelPointsAutomaticRewardRedemptionAddV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? ChannelPointsCustomRewardAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? ChannelPointsCustomRewardUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? ChannelPointsCustomRewardRemove;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? ChannelPointsCustomRewardRedemptionAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? ChannelPointsCustomRewardRedemptionUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollBeginArgs>? ChannelPollBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollEndArgs>? ChannelPollEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollProgressArgs>? ChannelPollProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionBeginArgs>? ChannelPredictionBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionEndArgs>? ChannelPredictionEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionLockArgs>? ChannelPredictionLock;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionProgressArgs>? ChannelPredictionProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSuspiciousUserMessageArgs>? ChannelSuspiciousUserMessage;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSuspiciousUserUpdateArgs>? ChannelSuspiciousUserUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelVipArgs>? ChannelVipAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelVipArgs>? ChannelVipRemove;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelWarningAcknowledgeArgs>? ChannelWarningAcknowledge;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelWarningSendArgs>? ChannelWarningSend;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelRaidArgs>? ChannelRaid;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelShieldModeBeginArgs>? ChannelShieldModeBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelShieldModeEndArgs>? ChannelShieldModeEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelShoutoutCreateArgs>? ChannelShoutoutCreate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelShoutoutReceiveArgs>? ChannelShoutoutReceive;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSharedChatSessionBeginArgs>? ChannelSharedChatSessionBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSharedChatSessionUpdateArgs>? ChannelSharedChatSessionUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSharedChatSessionEndArgs>? ChannelSharedChatSessionEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscribeArgs>? ChannelSubscribe;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionEndArgs>? ChannelSubscriptionEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionGiftArgs>? ChannelSubscriptionGift;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionMessageArgs>? ChannelSubscriptionMessage;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelUnbanArgs>? ChannelUnban;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelUpdateArgs>? ChannelUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ConduitShardDisabledArgs>? ConduitShardDisabled;
        /// <inheritdoc/>
        public event AsyncEventHandler<DropEntitlementGrantArgs>? DropEntitlementGrant;
        /// <inheritdoc/>
        public event AsyncEventHandler<ExtensionBitsTransactionCreateArgs>? ExtensionBitsTransactionCreate;
        /// <inheritdoc/>
        public event AsyncEventHandler<StreamOfflineArgs>? StreamOffline;
        /// <inheritdoc/>
        public event AsyncEventHandler<StreamOnlineArgs>? StreamOnline;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserAuthorizationGrantArgs>? UserAuthorizationGrant;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserAuthorizationRevokeArgs>? UserAuthorizationRevoke;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserUpdateArgs>? UserUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatClearArgs>? ChannelChatClear;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatClearUserMessagesArgs>? ChannelChatClearUserMessages;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatMessageArgs>? ChannelChatMessage;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatMessageDeleteArgs>? ChannelChatMessageDelete;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatNotificationArgs>? ChannelChatNotification;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatSettingsUpdateArgs>? ChannelChatSettingsUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatUserMessageHoldArgs>? ChannelChatUserMessageHold;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatUserMessageUpdateArgs>? ChannelChatUserMessageUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserWhisperMessageArgs>? UserWhisperMessage;

        /// <inheritdoc/>
        public async Task ProcessNotificationAsync(WebhookEventSubMetadata metadata, Stream body)
        {
            if (string.IsNullOrEmpty(metadata.SubscriptionType) || string.IsNullOrEmpty(metadata.SubscriptionVersion))
            {
                await Error.InvokeAsync(this, new OnErrorArgs { Reason = "Missing_Header", Message = "The Twitch-Eventsub-Subscription-Type or Twitch-Eventsub-Subscription-Version header was not found" });
                return;
            }

            try
            {
                var task = (metadata.SubscriptionType, metadata.SubscriptionVersion) switch
                {
                    ("automod.message.hold", "1") => InvokeEventSubEvent<AutomodMessageHoldArgs, AutomodMessageHold>(AutomodMessageHold),
                    ("automod.message.hold", "2") => InvokeEventSubEvent<AutomodMessageHoldV2Args, AutomodMessageHoldV2>(AutomodMessageHoldV2),
                    ("automod.message.update", "1") => InvokeEventSubEvent<AutomodMessageUpdateArgs, AutomodMessageUpdate>(AutomodMessageUpdate),
                    ("automod.message.update", "2") => InvokeEventSubEvent<AutomodMessageUpdateV2Args, AutomodMessageUpdateV2>(AutomodMessageUpdateV2),
                    ("automod.settings.update", "1") => InvokeEventSubEvent<AutomodSettingsUpdateArgs, AutomodSettingsUpdate>(AutomodSettingsUpdate),
                    ("automod.terms.update", "1") => InvokeEventSubEvent<AutomodTermsUpdateArgs, AutomodTermsUpdate>(AutomodTermsUpdate),
                    ("channel.bits.use", "1") => InvokeEventSubEvent<ChannelBitsUseArgs, ChannelBitsUse>(ChannelBitsUse),
                    ("channel.update", "2") => InvokeEventSubEvent<ChannelUpdateArgs, ChannelUpdate>(ChannelUpdate),
                    ("channel.follow", "2") => InvokeEventSubEvent<ChannelFollowArgs, ChannelFollow>(ChannelFollow),
                    ("channel.ad_break.begin", "1") => InvokeEventSubEvent<ChannelAdBreakBeginArgs, ChannelAdBreakBegin>(ChannelAdBreakBegin),
                    ("channel.chat.clear", "1") => InvokeEventSubEvent<ChannelChatClearArgs, ChannelChatClear>(ChannelChatClear),
                    ("channel.chat.clear_user_messages", "1") => InvokeEventSubEvent<ChannelChatClearUserMessagesArgs, ChannelChatClearUserMessages>(ChannelChatClearUserMessages),
                    ("channel.chat.message", "1") => InvokeEventSubEvent<ChannelChatMessageArgs, ChannelChatMessage>(ChannelChatMessage),
                    ("channel.chat.message_delete", "1") => InvokeEventSubEvent<ChannelChatMessageDeleteArgs, ChannelChatMessageDelete>(ChannelChatMessageDelete),
                    ("channel.chat.notification", "1") => InvokeEventSubEvent<ChannelChatNotificationArgs, ChannelChatNotification>(ChannelChatNotification),
                    ("channel.chat_settings.update", "1") => InvokeEventSubEvent<ChannelChatSettingsUpdateArgs, ChannelChatSettingsUpdate>(ChannelChatSettingsUpdate),
                    ("channel.chat.user_message_hold", "1") => InvokeEventSubEvent<ChannelChatUserMessageHoldArgs, ChannelChatUserMessageHold>(ChannelChatUserMessageHold),
                    ("channel.chat.user_message_update", "1") => InvokeEventSubEvent<ChannelChatUserMessageUpdateArgs, ChannelChatUserMessageUpdate>(ChannelChatUserMessageUpdate),
                    ("channel.shared_chat.begin", "1") => InvokeEventSubEvent<ChannelSharedChatSessionBeginArgs, ChannelSharedChatSessionBegin>(ChannelSharedChatSessionBegin),
                    ("channel.shared_chat.update", "1") => InvokeEventSubEvent<ChannelSharedChatSessionUpdateArgs, ChannelSharedChatSessionUpdate>(ChannelSharedChatSessionUpdate),
                    ("channel.shared_chat.end", "1") => InvokeEventSubEvent<ChannelSharedChatSessionEndArgs, ChannelSharedChatSessionEnd>(ChannelSharedChatSessionEnd),
                    ("channel.subscribe", "1") => InvokeEventSubEvent<ChannelSubscribeArgs, ChannelSubscribe>(ChannelSubscribe),
                    ("channel.subscription.end", "1") => InvokeEventSubEvent<ChannelSubscriptionEndArgs, ChannelSubscriptionEnd>(ChannelSubscriptionEnd),
                    ("channel.subscription.gift", "1") => InvokeEventSubEvent<ChannelSubscriptionGiftArgs, ChannelSubscriptionGift>(ChannelSubscriptionGift),
                    ("channel.subscription.message", "1") => InvokeEventSubEvent<ChannelSubscriptionMessageArgs, ChannelSubscriptionMessage>(ChannelSubscriptionMessage),
                    ("channel.cheer", "1") => InvokeEventSubEvent<ChannelCheerArgs, ChannelCheer>(ChannelCheer),
                    ("channel.raid", "1") => InvokeEventSubEvent<ChannelRaidArgs, ChannelRaid>(ChannelRaid),
                    ("channel.ban", "1") => InvokeEventSubEvent<ChannelBanArgs, ChannelBan>(ChannelBan),
                    ("channel.unban", "1") => InvokeEventSubEvent<ChannelUnbanArgs, ChannelUnban>(ChannelUnban),
                    ("channel.unban_request.create", "1") => InvokeEventSubEvent<ChannelUnbanRequestCreateArgs, ChannelUnbanRequestCreate>(ChannelUnbanRequestCreate),
                    ("channel.unban_request.resolve", "1") => InvokeEventSubEvent<ChannelUnbanRequestResolveArgs, ChannelUnbanRequestResolve>(ChannelUnbanRequestResolve),
                    ("channel.moderate", "1") => InvokeEventSubEvent<ChannelModerateArgs, ChannelModerate>(ChannelModerate),
                    ("channel.moderate", "2") => InvokeEventSubEvent<ChannelModerateV2Args, ChannelModerateV2>(ChannelModerateV2),
                    ("channel.moderator.add", "1") => InvokeEventSubEvent<ChannelModeratorArgs, ChannelModerator>(ChannelModeratorAdd),
                    ("channel.moderator.remove", "1") => InvokeEventSubEvent<ChannelModeratorArgs, ChannelModerator>(ChannelModeratorRemove),
                    ("channel.guest_star_session.begin", "beta") => InvokeEventSubEvent<ChannelGuestStarSessionBeginArgs, ChannelGuestStarSessionBegin>(ChannelGuestStarSessionBegin),
                    ("channel.guest_star_session.end", "beta") => InvokeEventSubEvent<ChannelGuestStarSessionEndArgs, ChannelGuestStarSessionEnd>(ChannelGuestStarSessionEnd),
                    ("channel.guest_star_guest.update", "beta") => InvokeEventSubEvent<ChannelGuestStarGuestUpdateArgs, ChannelGuestStarGuestUpdate>(ChannelGuestStarGuestUpdate),
                    ("channel.guest_star_settings.update", "beta") => InvokeEventSubEvent<ChannelGuestStarSettingsUpdateArgs, ChannelGuestStarSettingsUpdate>(ChannelGuestStarSettingsUpdate),
                    ("channel.channel_points_automatic_reward_redemption.add", "2") => InvokeEventSubEvent<ChannelPointsAutomaticRewardRedemptionAddV2Args, ChannelPointsAutomaticRewardRedemptionV2>(ChannelPointsAutomaticRewardRedemptionAddV2),
                    ("channel.channel_points_custom_reward.add", "1") => InvokeEventSubEvent<ChannelPointsCustomRewardArgs, ChannelPointsCustomReward>(ChannelPointsCustomRewardAdd),
                    ("channel.channel_points_custom_reward.update", "1") => InvokeEventSubEvent<ChannelPointsCustomRewardArgs, ChannelPointsCustomReward>(ChannelPointsCustomRewardUpdate),
                    ("channel.channel_points_custom_reward.remove", "1") => InvokeEventSubEvent<ChannelPointsCustomRewardArgs, ChannelPointsCustomReward>(ChannelPointsCustomRewardRemove),
                    ("channel.channel_points_custom_reward_redemption.add", "1") => InvokeEventSubEvent<ChannelPointsCustomRewardRedemptionArgs, ChannelPointsCustomRewardRedemption>(ChannelPointsCustomRewardRedemptionAdd),
                    ("channel.channel_points_custom_reward_redemption.update", "1") => InvokeEventSubEvent<ChannelPointsCustomRewardRedemptionArgs, ChannelPointsCustomRewardRedemption>(ChannelPointsCustomRewardRedemptionUpdate),
                    ("channel.poll.begin", "1") => InvokeEventSubEvent<ChannelPollBeginArgs, ChannelPollBegin>(ChannelPollBegin),
                    ("channel.poll.progress", "1") => InvokeEventSubEvent<ChannelPollProgressArgs, ChannelPollProgress>(ChannelPollProgress),
                    ("channel.poll.end", "1") => InvokeEventSubEvent<ChannelPollEndArgs, ChannelPollEnd>(ChannelPollEnd),
                    ("channel.prediction.begin", "1") => InvokeEventSubEvent<ChannelPredictionBeginArgs, ChannelPredictionBegin>(ChannelPredictionBegin),
                    ("channel.prediction.progress", "1") => InvokeEventSubEvent<ChannelPredictionProgressArgs, ChannelPredictionProgress>(ChannelPredictionProgress),
                    ("channel.prediction.lock", "1") => InvokeEventSubEvent<ChannelPredictionLockArgs, ChannelPredictionLock>(ChannelPredictionLock),
                    ("channel.prediction.end", "1") => InvokeEventSubEvent<ChannelPredictionEndArgs, ChannelPredictionEnd>(ChannelPredictionEnd),
                    ("channel.suspicious_user.message", "1") => InvokeEventSubEvent<ChannelSuspiciousUserMessageArgs, ChannelSuspiciousUserMessage>(ChannelSuspiciousUserMessage),
                    ("channel.suspicious_user.update", "1") => InvokeEventSubEvent<ChannelSuspiciousUserUpdateArgs, ChannelSuspiciousUserUpdate>(ChannelSuspiciousUserUpdate),
                    ("channel.vip.add", "1") => InvokeEventSubEvent<ChannelVipArgs, ChannelVip>(ChannelVipAdd),
                    ("channel.vip.remove", "1") => InvokeEventSubEvent<ChannelVipArgs, ChannelVip>(ChannelVipRemove),
                    ("channel.warning.acknowledge", "1") => InvokeEventSubEvent<ChannelWarningAcknowledgeArgs, ChannelWarningAcknowledge>(ChannelWarningAcknowledge),
                    ("channel.warning.send", "1") => InvokeEventSubEvent<ChannelWarningSendArgs, ChannelWarningSend>(ChannelWarningSend),
                    ("channel.charity_campaign.donate", "1") => InvokeEventSubEvent<ChannelCharityCampaignDonateArgs, ChannelCharityCampaignDonate>(ChannelCharityCampaignDonate),
                    ("channel.charity_campaign.start", "1") => InvokeEventSubEvent<ChannelCharityCampaignStartArgs, ChannelCharityCampaignStart>(ChannelCharityCampaignStart),
                    ("channel.charity_campaign.progress", "1") => InvokeEventSubEvent<ChannelCharityCampaignProgressArgs, ChannelCharityCampaignProgress>(ChannelCharityCampaignProgress),
                    ("channel.charity_campaign.stop", "1") => InvokeEventSubEvent<ChannelCharityCampaignStopArgs, ChannelCharityCampaignStop>(ChannelCharityCampaignStop),
                    ("conduit.shard.disabled", "1") => InvokeEventSubEvent<ConduitShardDisabledArgs, ConduitShardDisabled>(ConduitShardDisabled),
                    ("drop.entitlement.grant", "1") => InvokeEventSubEvent<DropEntitlementGrantArgs, EventSubBatchedEvent<DropEntitlementGrant>[]>(DropEntitlementGrant),
                    ("extension.bits_transaction.create", "1") => InvokeEventSubEvent<ExtensionBitsTransactionCreateArgs, ExtensionBitsTransactionCreate>(ExtensionBitsTransactionCreate),
                    ("channel.goal.begin", "1") => InvokeEventSubEvent<ChannelGoalBeginArgs, ChannelGoalBegin>(ChannelGoalBegin),
                    ("channel.goal.progress", "1") => InvokeEventSubEvent<ChannelGoalProgressArgs, ChannelGoalProgress>(ChannelGoalProgress),
                    ("channel.goal.end", "1") => InvokeEventSubEvent<ChannelGoalEndArgs, ChannelGoalEnd>(ChannelGoalEnd),
                    ("channel.hype_train.begin", "2") => InvokeEventSubEvent<ChannelHypeTrainBeginV2Args, HypeTrainBeginV2>(ChannelHypeTrainBeginV2),
                    ("channel.hype_train.progress", "2") => InvokeEventSubEvent<ChannelHypeTrainProgressV2Args, HypeTrainProgressV2>(ChannelHypeTrainProgressV2),
                    ("channel.hype_train.end", "2") => InvokeEventSubEvent<ChannelHypeTrainEndV2Args, HypeTrainEndV2>(ChannelHypeTrainEndV2),
                    ("channel.shield_mode.begin", "1") => InvokeEventSubEvent<ChannelShieldModeBeginArgs, ChannelShieldModeBegin>(ChannelShieldModeBegin),
                    ("channel.shield_mode.end", "1") => InvokeEventSubEvent<ChannelShieldModeEndArgs, ChannelShieldModeEnd>(ChannelShieldModeEnd),
                    ("channel.shoutout.create", "1") => InvokeEventSubEvent<ChannelShoutoutCreateArgs, ChannelShoutoutCreate>(ChannelShoutoutCreate),
                    ("channel.shoutout.receive", "1") => InvokeEventSubEvent<ChannelShoutoutReceiveArgs, ChannelShoutoutReceive>(ChannelShoutoutReceive),
                    ("stream.online", "1") => InvokeEventSubEvent<StreamOnlineArgs, StreamOnline>(StreamOnline),
                    ("stream.offline", "1") => InvokeEventSubEvent<StreamOfflineArgs, StreamOffline>(StreamOffline),
                    ("user.authorization.grant", "1") => InvokeEventSubEvent<UserAuthorizationGrantArgs, UserAuthorizationGrant>(UserAuthorizationGrant),
                    ("user.authorization.revoke", "1") => InvokeEventSubEvent<UserAuthorizationRevokeArgs, UserAuthorizationRevoke>(UserAuthorizationRevoke),
                    ("user.update", "1") => InvokeEventSubEvent<UserUpdateArgs, UserUpdate>(UserUpdate),
                    ("user.whisper.message", "1") => InvokeEventSubEvent<UserWhisperMessageArgs, UserWhisperMessage>(UserWhisperMessage),
                    _ => InvokeEventSubEvent<UnknownEventSubNotificationArgs, JsonElement>(UnknownEventSubNotification),
                };
                await task;
            }
            catch (Exception ex)
            {
                await Error.InvokeAsync(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }

            async Task InvokeEventSubEvent<TEventArgs , TModel>(AsyncEventHandler<TEventArgs>? asyncEventHandler)
                where TEventArgs : TwitchLibEventSubNotificationArgs<TModel>, new()
            {

                var notification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<TModel>>(body, _jsonSerializerOptions);
                await asyncEventHandler.InvokeAsync(this, new TEventArgs { Metadata = metadata, Payload = notification! });
            }
        }

        /// <inheritdoc/>
        public async Task ProcessRevocationAsync(WebhookEventSubMetadata metadata, Stream body)
        {
            try
            {
                var notification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<object>>(body, _jsonSerializerOptions);
                await Revocation.InvokeAsync(this, new RevocationArgs { Metadata = metadata, Payload = notification! });
            }
            catch (Exception ex)
            {
                await Error.InvokeAsync(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }
        }
    }
}