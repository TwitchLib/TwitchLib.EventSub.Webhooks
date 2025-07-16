using System;
using System.Collections.Generic;
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
        public event AsyncEventHandler<ChannelBanArgs>? OnChannelBan;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCheerArgs>? OnChannelCheer;

        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCharityCampaignStartArgs>? OnChannelCharityCampaignStart;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCharityCampaignDonateArgs>? OnChannelCharityCampaignDonate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCharityCampaignProgressArgs>? OnChannelCharityCampaignProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelCharityCampaignStopArgs>? OnChannelCharityCampaignStop;

        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelFollowArgs>? OnChannelFollow;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalBeginArgs>? OnChannelGoalBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalEndArgs>? OnChannelGoalEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelGoalProgressArgs>? OnChannelGoalProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainBeginArgs>? OnChannelHypeTrainBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainBeginV2Args>? OnChannelHypeTrainBeginV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainEndArgs>? OnChannelHypeTrainEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainEndV2Args>? OnChannelHypeTrainEndV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainProgressArgs>? OnChannelHypeTrainProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelHypeTrainProgressV2Args>? OnChannelHypeTrainProgressV2;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModeratorArgs>? OnChannelModeratorAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelModeratorArgs>? OnChannelModeratorRemove;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardRemove;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionAdd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollBeginArgs>? OnChannelPollBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollEndArgs>? OnChannelPollEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPollProgressArgs>? OnChannelPollProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionBeginArgs>? OnChannelPredictionBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionEndArgs>? OnChannelPredictionEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionLockArgs>? OnChannelPredictionLock;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelPredictionProgressArgs>? OnChannelPredictionProgress;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelRaidArgs>? OnChannelRaid;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelShieldModeBeginArgs>? OnChannelShieldModeBegin;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelShieldModeEndArgs>? OnChannelShieldModeEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelShoutoutCreateArgs>? OnChannelShoutoutCreate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelShoutoutReceiveArgs>? OnChannelShoutoutReceive;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscribeArgs>? OnChannelSubscribe;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionEndArgs>? OnChannelSubscriptionEnd;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionGiftArgs>? OnChannelSubscriptionGift;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelSubscriptionMessageArgs>? OnChannelSubscriptionMessage;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelUnbanArgs>? OnChannelUnban;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelUpdateArgs>? OnChannelUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<OnErrorArgs>? OnError;
        /// <inheritdoc/>
        public event AsyncEventHandler<ConduitShardDisabledArgs>? ConduitShardDisabled;
        /// <inheritdoc/>
        public event AsyncEventHandler<DropEntitlementGrantArgs>? OnDropEntitlementGrant;
        /// <inheritdoc/>
        public event AsyncEventHandler<ExtensionBitsTransactionCreateArgs>? OnExtensionBitsTransactionCreate;
        /// <inheritdoc/>
        public event AsyncEventHandler<RevocationArgs>? OnRevocation;
        /// <inheritdoc/>
        public event AsyncEventHandler<StreamOfflineArgs>? OnStreamOffline;
        /// <inheritdoc/>
        public event AsyncEventHandler<StreamOnlineArgs>? OnStreamOnline;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserAuthorizationGrantArgs>? OnUserAuthorizationGrant;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserAuthorizationRevokeArgs>? OnUserAuthorizationRevoke;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserUpdateArgs>? OnUserUpdate;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatClearArgs>? OnChannelChatClear;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatClearUserMessageArgs>? OnChannelChatClearUserMessage;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatMessageArgs>? OnChannelChatMessage;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatMessageDeleteArgs>? OnChannelChatMessageDelete;
        /// <inheritdoc/>
        public event AsyncEventHandler<ChannelChatNotificationArgs>? OnChannelChatNotification;
        /// <inheritdoc/>
        public event AsyncEventHandler<UserWhisperMessageArgs>? OnUserWhisperMessage;

        /// <inheritdoc/>
        public async Task ProcessNotificationAsync(Dictionary<string, string> headers, Stream body)
        {
            try
            {
                if (!headers.TryGetValue("Twitch-Eventsub-Subscription-Type", out var subscriptionType))
                {
                    await OnError.InvokeAsync(this, new OnErrorArgs { Reason = "Missing_Header", Message = "The Twitch-Eventsub-Subscription-Type header was not found" });
                    return;
                }
                if (!headers.TryGetValue("Twitch-Eventsub-Subscription-Version", out var subscriptionVersion))
                {
                    await OnError.InvokeAsync(this, new OnErrorArgs { Reason = "Missing_Header", Message = "The Twitch-Eventsub-Subscription-Version header was not found" });
                    return;
                }

                switch ((subscriptionType, subscriptionVersion))
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
                    case ("channel.ban", "1"):
                        await InvokeEventSubEvent<ChannelBanArgs, EventSubNotificationPayload<ChannelBan>>(OnChannelBan);
                        break;
                    case ("channel.cheer", "1"):
                        await InvokeEventSubEvent<ChannelCheerArgs, EventSubNotificationPayload<ChannelCheer>>(OnChannelCheer);
                        break;
                    case ("channel.charity_campaign.start", "1"):
                        await InvokeEventSubEvent<ChannelCharityCampaignStartArgs, EventSubNotificationPayload<ChannelCharityCampaignStart>>(OnChannelCharityCampaignStart);
                        break;
                    case ("channel.charity_campaign.donate", "1"):
                        await InvokeEventSubEvent<ChannelCharityCampaignDonateArgs, EventSubNotificationPayload<ChannelCharityCampaignDonate>>(OnChannelCharityCampaignDonate);
                        break;
                    case ("channel.charity_campaign.progress", "1"):
                        await InvokeEventSubEvent<ChannelCharityCampaignProgressArgs, EventSubNotificationPayload<ChannelCharityCampaignProgress>>(OnChannelCharityCampaignProgress);
                        break;
                    case ("channel.charity_campaign.stop", "1"):
                        await InvokeEventSubEvent<ChannelCharityCampaignStopArgs, EventSubNotificationPayload<ChannelCharityCampaignStop>>(OnChannelCharityCampaignStop);
                        break;
                    case ("channel.follow", "2"):
                        await InvokeEventSubEvent<ChannelFollowArgs, EventSubNotificationPayload<ChannelFollow>>(OnChannelFollow);
                        break;
                    case ("channel.goal.begin", "1"):
                        await InvokeEventSubEvent<ChannelGoalBeginArgs, EventSubNotificationPayload<ChannelGoalBegin>>(OnChannelGoalBegin);
                        break;
                    case ("channel.goal.end", "1"):
                        await InvokeEventSubEvent<ChannelGoalEndArgs, EventSubNotificationPayload<ChannelGoalEnd>>(OnChannelGoalEnd);
                        break;
                    case ("channel.goal.progress", "1"):
                        await InvokeEventSubEvent<ChannelGoalProgressArgs, EventSubNotificationPayload<ChannelGoalProgress>>(OnChannelGoalProgress);
                        break;
                    case ("channel.hype_train.begin", "1"):
                        await InvokeEventSubEvent<ChannelHypeTrainBeginArgs, EventSubNotificationPayload<HypeTrainBegin>>(OnChannelHypeTrainBegin);
                        break;
                    case ("channel.hype_train.begin", "2"):
                        await InvokeEventSubEvent<ChannelHypeTrainBeginV2Args, EventSubNotificationPayload<HypeTrainBeginV2>>(OnChannelHypeTrainBeginV2);
                        break;
                    case ("channel.hype_train.end", "1"):
                        await InvokeEventSubEvent<ChannelHypeTrainEndArgs, EventSubNotificationPayload<HypeTrainEnd>>(OnChannelHypeTrainEnd);
                        break;
                    case ("channel.hype_train.end", "2"):
                        await InvokeEventSubEvent<ChannelHypeTrainEndV2Args, EventSubNotificationPayload<HypeTrainEndV2>>(OnChannelHypeTrainEndV2);
                        break;
                    case ("channel.hype_train.progress", "1"):
                        await InvokeEventSubEvent<ChannelHypeTrainProgressArgs, EventSubNotificationPayload<HypeTrainProgress>>(OnChannelHypeTrainProgress);
                        break;
                    case ("channel.hype_train.progress", "2"):
                        await InvokeEventSubEvent<ChannelHypeTrainProgressV2Args, EventSubNotificationPayload<HypeTrainProgressV2>>(OnChannelHypeTrainProgressV2);
                        break;
                    case ("channel.moderator.add", "1"):
                        await InvokeEventSubEvent<ChannelModeratorArgs, EventSubNotificationPayload<ChannelModerator>>(OnChannelModeratorAdd);
                        break;
                    case ("channel.moderator.remove", "1"):
                        await InvokeEventSubEvent<ChannelModeratorArgs, EventSubNotificationPayload<ChannelModerator>>(OnChannelModeratorRemove);
                        break;
                    case ("channel.channel_points_custom_reward.add", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardArgs, EventSubNotificationPayload<ChannelPointsCustomReward>>(OnChannelPointsCustomRewardAdd);
                        break;
                    case ("channel.channel_points_custom_reward.remove", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardArgs, EventSubNotificationPayload<ChannelPointsCustomReward>>(OnChannelPointsCustomRewardRemove);
                        break;
                    case ("channel.channel_points_custom_reward.update", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardArgs, EventSubNotificationPayload<ChannelPointsCustomReward>>(OnChannelPointsCustomRewardUpdate);
                        break;
                    case ("channel.channel_points_custom_reward_redemption.add", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardRedemptionArgs, EventSubNotificationPayload<ChannelPointsCustomRewardRedemption>>(OnChannelPointsCustomRewardRedemptionAdd);
                        break;
                    case ("channel.channel_points_custom_reward_redemption.update", "1"):
                        await InvokeEventSubEvent<ChannelPointsCustomRewardRedemptionArgs, EventSubNotificationPayload<ChannelPointsCustomRewardRedemption>>(OnChannelPointsCustomRewardRedemptionUpdate);
                        break;
                    case ("channel.poll.begin", "1"):
                        await InvokeEventSubEvent<ChannelPollBeginArgs, EventSubNotificationPayload<ChannelPollBegin>>(OnChannelPollBegin);
                        break;
                    case ("channel.poll.end", "1"):
                        await InvokeEventSubEvent<ChannelPollEndArgs, EventSubNotificationPayload<ChannelPollEnd>>(OnChannelPollEnd);
                        break;
                    case ("channel.poll.progress", "1"):
                        await InvokeEventSubEvent<ChannelPollProgressArgs, EventSubNotificationPayload<ChannelPollProgress>>(OnChannelPollProgress);
                        break;
                    case ("channel.prediction.begin", "1"):
                        await InvokeEventSubEvent<ChannelPredictionBeginArgs, EventSubNotificationPayload<ChannelPredictionBegin>>(OnChannelPredictionBegin);
                        break;
                    case ("channel.prediction.end", "1"):
                        await InvokeEventSubEvent<ChannelPredictionEndArgs, EventSubNotificationPayload<ChannelPredictionEnd>>(OnChannelPredictionEnd);
                        break;
                    case ("channel.prediction.lock", "1"):
                        await InvokeEventSubEvent<ChannelPredictionLockArgs, EventSubNotificationPayload<ChannelPredictionLock>>(OnChannelPredictionLock);
                        break;
                    case ("channel.prediction.progress", "1"):
                        await InvokeEventSubEvent<ChannelPredictionProgressArgs, EventSubNotificationPayload<ChannelPredictionProgress>>(OnChannelPredictionProgress);
                        break;
                    case ("channel.raid", "1"):
                        await InvokeEventSubEvent<ChannelRaidArgs, EventSubNotificationPayload<ChannelRaid>>(OnChannelRaid);
                        break;
                    case ("channel.shield_mode.begin", "1"):
                        await InvokeEventSubEvent<ChannelShieldModeBeginArgs, EventSubNotificationPayload<ChannelShieldModeBegin>>(OnChannelShieldModeBegin);
                        break;
                    case ("channel.shield_mode.end", "1"):
                        await InvokeEventSubEvent<ChannelShieldModeEndArgs, EventSubNotificationPayload<ChannelShieldModeEnd>>(OnChannelShieldModeEnd);
                        break;
                    case ("channel.shoutout.create", "1"):
                        await InvokeEventSubEvent<ChannelShoutoutCreateArgs, EventSubNotificationPayload<ChannelShoutoutCreate>>(OnChannelShoutoutCreate);
                        break;
                    case ("channel.shoutout.receive", "1"):
                        await InvokeEventSubEvent<ChannelShoutoutReceiveArgs, EventSubNotificationPayload<ChannelShoutoutReceive>>(OnChannelShoutoutReceive);
                        break;
                    case ("channel.subscribe", "1"):
                        await InvokeEventSubEvent<ChannelSubscribeArgs, EventSubNotificationPayload<ChannelSubscribe>>(OnChannelSubscribe);
                        break;
                    case ("channel.subscription.end", "1"):
                        await InvokeEventSubEvent<ChannelSubscriptionEndArgs, EventSubNotificationPayload<ChannelSubscriptionEnd>>(OnChannelSubscriptionEnd);
                        break;
                    case ("channel.subscription.gift", "1"):
                        await InvokeEventSubEvent<ChannelSubscriptionGiftArgs, EventSubNotificationPayload<ChannelSubscriptionGift>>(OnChannelSubscriptionGift);
                        break;
                    case ("channel.subscription.message", "1"):
                        await InvokeEventSubEvent<ChannelSubscriptionMessageArgs, EventSubNotificationPayload<ChannelSubscriptionMessage>>(OnChannelSubscriptionMessage);
                        break;
                    case ("channel.unban", "1"):
                        await InvokeEventSubEvent<ChannelUnbanArgs, EventSubNotificationPayload<ChannelUnban>>(OnChannelUnban);
                        break;
                    case ("channel.update", "2"):
                        await InvokeEventSubEvent<ChannelUpdateArgs, EventSubNotificationPayload<ChannelUpdate>>(OnChannelUpdate);
                        break;
                    case ("drop.entitlement.grant", "1"):
                        await InvokeEventSubEvent<DropEntitlementGrantArgs, BatchedNotificationPayload<DropEntitlementGrant>>(OnDropEntitlementGrant);
                        break;
                    case ("conduit.shard.disabled", "1"):
                        await InvokeEventSubEvent<ConduitShardDisabledArgs, EventSubNotificationPayload<ConduitShardDisabled>>(ConduitShardDisabled);
                        break;
                    case ("extension.bits_transaction.create", "1"):
                        await InvokeEventSubEvent<ExtensionBitsTransactionCreateArgs, EventSubNotificationPayload<ExtensionBitsTransactionCreate>>(OnExtensionBitsTransactionCreate);
                        break;
                    case ("stream.offline", "1"):
                        await InvokeEventSubEvent<StreamOfflineArgs, EventSubNotificationPayload<StreamOffline>>(OnStreamOffline);
                        break;
                    case ("stream.online", "1"):
                        await InvokeEventSubEvent<StreamOnlineArgs, EventSubNotificationPayload<StreamOnline>>(OnStreamOnline);
                        break;
                    case ("user.authorization.grant", "1"):
                        await InvokeEventSubEvent<UserAuthorizationGrantArgs, EventSubNotificationPayload<UserAuthorizationGrant>>(OnUserAuthorizationGrant);
                        break;
                    case ("user.authorization.revoke", "1"):
                        await InvokeEventSubEvent<UserAuthorizationRevokeArgs, EventSubNotificationPayload<UserAuthorizationRevoke>>(OnUserAuthorizationRevoke);
                        break;
                    case ("user.update", "1"):
                        await InvokeEventSubEvent<UserUpdateArgs, EventSubNotificationPayload<UserUpdate>>(OnUserUpdate);
                        break;
                    case ("channel.chat.clear", "1"):
                        await InvokeEventSubEvent<ChannelChatClearArgs, EventSubNotificationPayload<ChannelChatClear>>(OnChannelChatClear);
                        break;
                    case ("channel.chat.clear_user_messages", "1"):
                        await InvokeEventSubEvent<ChannelChatClearUserMessageArgs, EventSubNotificationPayload<ChannelChatClearUserMessage>>(OnChannelChatClearUserMessage);
                        break;
                    case ("channel.chat.message", "1"):
                        await InvokeEventSubEvent<ChannelChatMessageArgs, EventSubNotificationPayload<ChannelChatMessage>>(OnChannelChatMessage);
                        break;
                    case ("channel.chat.message_delete", "1"):
                        await InvokeEventSubEvent<ChannelChatMessageDeleteArgs, EventSubNotificationPayload<ChannelChatMessageDelete>>(OnChannelChatMessageDelete);
                        break;
                    case ("channel.chat.notification", "1"):
                        await InvokeEventSubEvent<ChannelChatNotificationArgs, EventSubNotificationPayload<ChannelChatNotification>>(OnChannelChatNotification);
                        break;
                    case ("user.whisper.message", "1"):
                        await InvokeEventSubEvent<UserWhisperMessageArgs, EventSubNotificationPayload<UserWhisperMessage>>(OnUserWhisperMessage);
                        break;
                    default:
                        await InvokeEventSubEvent<UnknownEventSubNotificationArgs, EventSubNotificationPayload<JsonElement>>(UnknownEventSubNotification);
                        break;
                }
            }
            catch (Exception ex)
            {
                await OnError.InvokeAsync(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }

            async Task InvokeEventSubEvent<TEvent, TModel>(AsyncEventHandler<TEvent>? asyncEventHandler)
                where TEvent : TwitchLibEventSubEventArgs<TModel>, new()
                where TModel : new()
            {
                var notification = await JsonSerializer.DeserializeAsync<TModel>(body, _jsonSerializerOptions);
                await asyncEventHandler.InvokeAsync(this, new TEvent { Headers = headers, Notification = notification! });
            }
        }

        /// <inheritdoc/>
        public async Task ProcessRevocationAsync(Dictionary<string, string> headers, Stream body)
        {
            try
            {
                var notification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<object>>(body, _jsonSerializerOptions);
                await OnRevocation.InvokeAsync(this, new RevocationArgs { Headers = headers, Notification = notification! });
            }
            catch (Exception ex)
            {
                await OnError.InvokeAsync(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }
        }
    }
}