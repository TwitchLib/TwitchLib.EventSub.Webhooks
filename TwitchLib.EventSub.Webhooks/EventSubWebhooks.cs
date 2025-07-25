using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TwitchLib.EventSub.Core;
using TwitchLib.EventSub.Core.Extensions;
using TwitchLib.EventSub.Core.SubscriptionTypes.Automod;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Core.SubscriptionTypes.Conduit;
using TwitchLib.EventSub.Core.SubscriptionTypes.Drop;
using TwitchLib.EventSub.Core.SubscriptionTypes.Extension;
using TwitchLib.EventSub.Core.SubscriptionTypes.Stream;
using TwitchLib.EventSub.Core.SubscriptionTypes.User;
using TwitchLib.EventSub.Webhooks.Core;
using TwitchLib.EventSub.Webhooks.Core.EventArgs;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Automod;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Channel;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Conduit;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Drop;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Extension;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Stream;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.User;
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
        public event AsyncEventHandler<ChannelModerateArgs>? ChannelModerate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModerateV2Args>? ChannelModerateV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModeratorArgs>? ChannelModeratorAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModeratorArgs>? ChannelModeratorRemove;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsAutomaticRewardRedemptionAddArgs>? ChannelPointsAutomaticRewardRedemptionAdd;
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
        public event AsyncEventHandler<ChannelChatClearUserMessageArgs>? ChannelChatClearUserMessage;
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
                switch ((metadata.SubscriptionType, metadata.SubscriptionVersion))
                {
                    case ("automod.message.hold", "1"):
                        await InvokeEventSubEvent<AutomodMessageHoldArgs, EventSubNotificationPayload<AutomodMessageHold>>(AutomodMessageHold);
                        break;
                    case ("automod.message.hold", "2"):
                        await InvokeEventSubEvent<AutomodMessageHoldV2Args, EventSubNotificationPayload<AutomodMessageHoldV2>>(AutomodMessageHoldV2);
                        break;
                    case ("automod.message.update", "1"):
                        await InvokeEventSubEvent<AutomodMessageUpdateArgs, EventSubNotificationPayload<AutomodMessageUpdate>>(AutomodMessageUpdate);
                        break;
                    case ("automod.message.update", "2"):
                        await InvokeEventSubEvent<AutomodMessageUpdateV2Args, EventSubNotificationPayload<AutomodMessageUpdateV2>>(AutomodMessageUpdateV2);
                        break;
                    case ("automod.settings.update", "1"):
                        await InvokeEventSubEvent<AutomodSettingsUpdateArgs, EventSubNotificationPayload<AutomodSettingsUpdate>>(AutomodSettingsUpdate);
                        break;
                    case ("automod.terms.update", "1"):
                        await InvokeEventSubEvent<AutomodTermsUpdateArgs, EventSubNotificationPayload<AutomodTermsUpdate>>(AutomodTermsUpdate);
                        break;
                    case ("channel.bits.use", "1"):
                        await InvokeEventSubEvent<ChannelBitsUseArgs, EventSubNotificationPayload<ChannelBitUse>>(ChannelBitsUse);
                        break;
                    case ("channel.ban", "1"):
                        await InvokeEventSubEvent<ChannelBanArgs, EventSubNotificationPayload<ChannelBan>>(ChannelBan);
                        break;
                    case ("channel.cheer", "1"):
                        await InvokeEventSubEvent<ChannelCheerArgs, EventSubNotificationPayload<ChannelCheer>>(ChannelCheer);
                        break;
                    case ("channel.charity_campaign.start", "1"):
                        await InvokeEventSubEvent<ChannelCharityCampaignStartArgs, EventSubNotificationPayload<ChannelCharityCampaignStart>>(ChannelCharityCampaignStart);
                        break;
                    case ("channel.charity_campaign.donate", "1"):
                        await InvokeEventSubEvent<ChannelCharityCampaignDonateArgs, EventSubNotificationPayload<ChannelCharityCampaignDonate>>(ChannelCharityCampaignDonate);
                        break;
                    case ("channel.charity_campaign.progress", "1"):
                        await InvokeEventSubEvent<ChannelCharityCampaignProgressArgs, EventSubNotificationPayload<ChannelCharityCampaignProgress>>(ChannelCharityCampaignProgress);
                        break;
                    case ("channel.charity_campaign.stop", "1"):
                        await InvokeEventSubEvent<ChannelCharityCampaignStopArgs, EventSubNotificationPayload<ChannelCharityCampaignStop>>(ChannelCharityCampaignStop);
                        break;
                    case ("channel.follow", "2"):
                        await InvokeEventSubEvent<ChannelFollowArgs, EventSubNotificationPayload<ChannelFollow>>(ChannelFollow);
                        break;
                    case ("channel.goal.begin", "1"):
                        await InvokeEventSubEvent<ChannelGoalBeginArgs, EventSubNotificationPayload<ChannelGoalBegin>>(ChannelGoalBegin);
                        break;
                    case ("channel.goal.end", "1"):
                        await InvokeEventSubEvent<ChannelGoalEndArgs, EventSubNotificationPayload<ChannelGoalEnd>>(ChannelGoalEnd);
                        break;
                    case ("channel.goal.progress", "1"):
                        await InvokeEventSubEvent<ChannelGoalProgressArgs, EventSubNotificationPayload<ChannelGoalProgress>>(ChannelGoalProgress);
                        break;
                    case ("channel.hype_train.begin", "2"):
                        await InvokeEventSubEvent<ChannelHypeTrainBeginV2Args, EventSubNotificationPayload<HypeTrainBeginV2>>(ChannelHypeTrainBeginV2);
                        break;
                    case ("channel.hype_train.end", "2"):
                        await InvokeEventSubEvent<ChannelHypeTrainEndV2Args, EventSubNotificationPayload<HypeTrainEndV2>>(ChannelHypeTrainEndV2);
                        break;
                    case ("channel.hype_train.progress", "2"):
                        await InvokeEventSubEvent<ChannelHypeTrainProgressV2Args, EventSubNotificationPayload<HypeTrainProgressV2>>(ChannelHypeTrainProgressV2);
                        break;
                    case ("channel.moderate", "1"):
                        await InvokeEventSubEvent<ChannelModerateArgs, EventSubNotificationPayload<ChannelModerate>>(ChannelModerate);
                        break;
                    case ("channel.moderate", "2"):
                        await InvokeEventSubEvent<ChannelModerateV2Args, EventSubNotificationPayload<ChannelModerateV2>>(ChannelModerateV2);
                        break;
                    case ("channel.moderator.add", "1"):
                        await InvokeEventSubEvent<ChannelModeratorArgs, EventSubNotificationPayload<ChannelModerator>>(ChannelModeratorAdd);
                        break;
                    case ("channel.moderator.remove", "1"):
                        await InvokeEventSubEvent<ChannelModeratorArgs, EventSubNotificationPayload<ChannelModerator>>(ChannelModeratorRemove);
                        break;
                    case ("channel.channel_points_automatic_reward_redemption.add", "1"):
                        await InvokeEventSubEvent<ChannelPointsAutomaticRewardRedemptionAddArgs, EventSubNotificationPayload<ChannelPointsAutomaticRewardRedemption>>(ChannelPointsAutomaticRewardRedemptionAdd);
                        break;
                    case ("channel.channel_points_automatic_reward_redemption.add", "2"):
                        await InvokeEventSubEvent<ChannelPointsAutomaticRewardRedemptionAddV2Args, EventSubNotificationPayload<ChannelPointsAutomaticRewardRedemptionV2>>(ChannelPointsAutomaticRewardRedemptionAddV2);
                        break;
                    case ("channel.channel_points_custom_reward.add", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardArgs, EventSubNotificationPayload<ChannelPointsCustomReward>>(ChannelPointsCustomRewardAdd);
                        break;
                    case ("channel.channel_points_custom_reward.remove", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardArgs, EventSubNotificationPayload<ChannelPointsCustomReward>>(ChannelPointsCustomRewardRemove);
                        break;
                    case ("channel.channel_points_custom_reward.update", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardArgs, EventSubNotificationPayload<ChannelPointsCustomReward>>(ChannelPointsCustomRewardUpdate);
                        break;
                    case ("channel.channel_points_custom_reward_redemption.add", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardRedemptionArgs, EventSubNotificationPayload<ChannelPointsCustomRewardRedemption>>(ChannelPointsCustomRewardRedemptionAdd);
                        break;
                    case ("channel.channel_points_custom_reward_redemption.update", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardRedemptionArgs, EventSubNotificationPayload<ChannelPointsCustomRewardRedemption>>(ChannelPointsCustomRewardRedemptionUpdate);
                        break;
                    case ("channel.poll.begin", "1"):
                        await InvokeEventSubEvent<ChannelPollBeginArgs, EventSubNotificationPayload<ChannelPollBegin>>(ChannelPollBegin);
                        break;
                    case ("channel.poll.end", "1"):
                        await InvokeEventSubEvent<ChannelPollEndArgs, EventSubNotificationPayload<ChannelPollEnd>>(ChannelPollEnd);
                        break;
                    case ("channel.poll.progress", "1"):
                        await InvokeEventSubEvent<ChannelPollProgressArgs, EventSubNotificationPayload<ChannelPollProgress>>(ChannelPollProgress);
                        break;
                    case ("channel.prediction.begin", "1"):
                        await InvokeEventSubEvent<ChannelPredictionBeginArgs, EventSubNotificationPayload<ChannelPredictionBegin>>(ChannelPredictionBegin);
                        break;
                    case ("channel.prediction.end", "1"):
                        await InvokeEventSubEvent<ChannelPredictionEndArgs, EventSubNotificationPayload<ChannelPredictionEnd>>(ChannelPredictionEnd);
                        break;
                    case ("channel.prediction.lock", "1"):
                        await InvokeEventSubEvent<ChannelPredictionLockArgs, EventSubNotificationPayload<ChannelPredictionLock>>(ChannelPredictionLock);
                        break;
                    case ("channel.prediction.progress", "1"):
                        await InvokeEventSubEvent<ChannelPredictionProgressArgs, EventSubNotificationPayload<ChannelPredictionProgress>>(ChannelPredictionProgress);
                        break;
                    case ("channel.raid", "1"):
                        await InvokeEventSubEvent<ChannelRaidArgs, EventSubNotificationPayload<ChannelRaid>>(ChannelRaid);
                        break;
                    case ("channel.shield_mode.begin", "1"):
                        await InvokeEventSubEvent<ChannelShieldModeBeginArgs, EventSubNotificationPayload<ChannelShieldModeBegin>>(ChannelShieldModeBegin);
                        break;
                    case ("channel.shield_mode.end", "1"):
                        await InvokeEventSubEvent<ChannelShieldModeEndArgs, EventSubNotificationPayload<ChannelShieldModeEnd>>(ChannelShieldModeEnd);
                        break;
                    case ("channel.shoutout.create", "1"):
                        await InvokeEventSubEvent<ChannelShoutoutCreateArgs, EventSubNotificationPayload<ChannelShoutoutCreate>>(ChannelShoutoutCreate);
                        break;
                    case ("channel.shoutout.receive", "1"):
                        await InvokeEventSubEvent<ChannelShoutoutReceiveArgs, EventSubNotificationPayload<ChannelShoutoutReceive>>(ChannelShoutoutReceive);
                        break;
                    case ("channel.subscribe", "1"):
                        await InvokeEventSubEvent<ChannelSubscribeArgs, EventSubNotificationPayload<ChannelSubscribe>>(ChannelSubscribe);
                        break;
                    case ("channel.subscription.end", "1"):
                        await InvokeEventSubEvent<ChannelSubscriptionEndArgs, EventSubNotificationPayload<ChannelSubscriptionEnd>>(ChannelSubscriptionEnd);
                        break;
                    case ("channel.subscription.gift", "1"):
                        await InvokeEventSubEvent<ChannelSubscriptionGiftArgs, EventSubNotificationPayload<ChannelSubscriptionGift>>(ChannelSubscriptionGift);
                        break;
                    case ("channel.subscription.message", "1"):
                        await InvokeEventSubEvent<ChannelSubscriptionMessageArgs, EventSubNotificationPayload<ChannelSubscriptionMessage>>(ChannelSubscriptionMessage);
                        break;
                    case ("channel.unban", "1"):
                        await InvokeEventSubEvent<ChannelUnbanArgs, EventSubNotificationPayload<ChannelUnban>>(ChannelUnban);
                        break;
                    case ("channel.update", "2"):
                        await InvokeEventSubEvent<ChannelUpdateArgs, EventSubNotificationPayload<ChannelUpdate>>(ChannelUpdate);
                        break;
                    case ("drop.entitlement.grant", "1"):
                        await InvokeEventSubEvent<DropEntitlementGrantArgs, BatchedNotificationPayload<DropEntitlementGrant>>(DropEntitlementGrant);
                        break;
                    case ("conduit.shard.disabled", "1"):
                        await InvokeEventSubEvent<ConduitShardDisabledArgs, EventSubNotificationPayload<ConduitShardDisabled>>(ConduitShardDisabled);
                        break;
                    case ("extension.bits_transaction.create", "1"):
                        await InvokeEventSubEvent<ExtensionBitsTransactionCreateArgs, EventSubNotificationPayload<ExtensionBitsTransactionCreate>>(ExtensionBitsTransactionCreate);
                        break;
                    case ("stream.offline", "1"):
                        await InvokeEventSubEvent<StreamOfflineArgs, EventSubNotificationPayload<StreamOffline>>(StreamOffline);
                        break;
                    case ("stream.online", "1"):
                        await InvokeEventSubEvent<StreamOnlineArgs, EventSubNotificationPayload<StreamOnline>>(StreamOnline);
                        break;
                    case ("user.authorization.grant", "1"):
                        await InvokeEventSubEvent<UserAuthorizationGrantArgs, EventSubNotificationPayload<UserAuthorizationGrant>>(UserAuthorizationGrant);
                        break;
                    case ("user.authorization.revoke", "1"):
                        await InvokeEventSubEvent<UserAuthorizationRevokeArgs, EventSubNotificationPayload<UserAuthorizationRevoke>>(UserAuthorizationRevoke);
                        break;
                    case ("user.update", "1"):
                        await InvokeEventSubEvent<UserUpdateArgs, EventSubNotificationPayload<UserUpdate>>(UserUpdate);
                        break;
                    case ("channel.chat.clear", "1"):
                        await InvokeEventSubEvent<ChannelChatClearArgs, EventSubNotificationPayload<ChannelChatClear>>(ChannelChatClear);
                        break;
                    case ("channel.chat.clear_user_messages", "1"):
                        await InvokeEventSubEvent<ChannelChatClearUserMessageArgs, EventSubNotificationPayload<ChannelChatClearUserMessage>>(ChannelChatClearUserMessage);
                        break;
                    case ("channel.chat.message", "1"):
                        await InvokeEventSubEvent<ChannelChatMessageArgs, EventSubNotificationPayload<ChannelChatMessage>>(ChannelChatMessage);
                        break;
                    case ("channel.chat.message_delete", "1"):
                        await InvokeEventSubEvent<ChannelChatMessageDeleteArgs, EventSubNotificationPayload<ChannelChatMessageDelete>>(ChannelChatMessageDelete);
                        break;
                    case ("channel.chat.notification", "1"):
                        await InvokeEventSubEvent<ChannelChatNotificationArgs, EventSubNotificationPayload<ChannelChatNotification>>(ChannelChatNotification);
                        break;
                    case ("channel.chat_settings.update", "1"):
                        await InvokeEventSubEvent<ChannelChatSettingsUpdateArgs, EventSubNotificationPayload<ChannelChatSettingsUpdate>>(ChannelChatSettingsUpdate);
                        break;
                    case ("channel.chat.user_message_hold", "1"):
                        await InvokeEventSubEvent<ChannelChatUserMessageHoldArgs, EventSubNotificationPayload<ChannelChatUserMessageHold>>(ChannelChatUserMessageHold);
                        break;
                    case ("channel.chat.user_message_update", "1"):
                        await InvokeEventSubEvent<ChannelChatUserMessageUpdateArgs, EventSubNotificationPayload<ChannelChatUserMessageUpdate>>(ChannelChatUserMessageUpdate);
                        break;
                    case ("user.whisper.message", "1"):
                        await InvokeEventSubEvent<UserWhisperMessageArgs, EventSubNotificationPayload<UserWhisperMessage>>(UserWhisperMessage);
                        break;
                    default:
                        await InvokeEventSubEvent<UnknownEventSubNotificationArgs, EventSubNotificationPayload<JsonElement>>(UnknownEventSubNotification);
                        break;
                }
            }
            catch (Exception ex)
            {
                await Error.InvokeAsync(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }

            async Task InvokeEventSubEvent<TEvent, TModel>(AsyncEventHandler<TEvent>? asyncEventHandler)
                where TEvent : TwitchLibEventSubEventArgs<TModel>, new()
                where TModel : new()
            {
                var notification = await JsonSerializer.DeserializeAsync<TModel>(body, _jsonSerializerOptions);
                await asyncEventHandler.InvokeAsync(this, new TEvent { Metadata = metadata, Notification = notification! });
            }
        }

        /// <inheritdoc/>
        public async Task ProcessRevocationAsync(WebhookEventSubMetadata metadata, Stream body)
        {
            try
            {
                var notification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<object>>(body, _jsonSerializerOptions);
                await Revocation.InvokeAsync(this, new RevocationArgs { Metadata = metadata, Notification = notification! });
            }
            catch (Exception ex)
            {
                await Error.InvokeAsync(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }
        }
    }
}