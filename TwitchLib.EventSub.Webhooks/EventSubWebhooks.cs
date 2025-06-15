using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TwitchLib.EventSub.Core.SubscriptionTypes.Channel;
using TwitchLib.EventSub.Core.SubscriptionTypes.Drop;
using TwitchLib.EventSub.Core.SubscriptionTypes.Extension;
using TwitchLib.EventSub.Core.SubscriptionTypes.Stream;
using TwitchLib.EventSub.Core.SubscriptionTypes.User;
using TwitchLib.EventSub.Webhooks.Core;
using TwitchLib.EventSub.Webhooks.Core.EventArgs;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Channel;
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
        public event EventHandler<ChannelBanArgs>? OnChannelBan;
        /// <inheritdoc/>
        public event EventHandler<ChannelCheerArgs>? OnChannelCheer;

        /// <inheritdoc/>
        public event EventHandler<ChannelCharityCampaignStartArgs>? OnChannelCharityCampaignStart;
        /// <inheritdoc/>
        public event EventHandler<ChannelCharityCampaignDonateArgs>? OnChannelCharityCampaignDonate;
        /// <inheritdoc/>
        public event EventHandler<ChannelCharityCampaignProgressArgs>? OnChannelCharityCampaignProgress;
        /// <inheritdoc/>
        public event EventHandler<ChannelCharityCampaignStopArgs>? OnChannelCharityCampaignStop;

        /// <inheritdoc/>
        public event EventHandler<ChannelFollowArgs>? OnChannelFollow;
        /// <inheritdoc/>
        public event EventHandler<ChannelGoalBeginArgs>? OnChannelGoalBegin;
        /// <inheritdoc/>
        public event EventHandler<ChannelGoalEndArgs>? OnChannelGoalEnd;
        /// <inheritdoc/>
        public event EventHandler<ChannelGoalProgressArgs>? OnChannelGoalProgress;
        /// <inheritdoc/>
        public event EventHandler<ChannelHypeTrainBeginArgs>? OnChannelHypeTrainBegin;
        /// <inheritdoc/>
        public event EventHandler<ChannelHypeTrainEndArgs>? OnChannelHypeTrainEnd;
        /// <inheritdoc/>
        public event EventHandler<ChannelHypeTrainProgressArgs>? OnChannelHypeTrainProgress;
        /// <inheritdoc/>
        public event EventHandler<ChannelModeratorArgs>? OnChannelModeratorAdd;
        /// <inheritdoc/>
        public event EventHandler<ChannelModeratorArgs>? OnChannelModeratorRemove;
        /// <inheritdoc/>
        public event EventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardAdd;
        /// <inheritdoc/>
        public event EventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardUpdate;
        /// <inheritdoc/>
        public event EventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardRemove;
        /// <inheritdoc/>
        public event EventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionAdd;
        /// <inheritdoc/>
        public event EventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionUpdate;
        /// <inheritdoc/>
        public event EventHandler<ChannelPollBeginArgs>? OnChannelPollBegin;
        /// <inheritdoc/>
        public event EventHandler<ChannelPollEndArgs>? OnChannelPollEnd;
        /// <inheritdoc/>
        public event EventHandler<ChannelPollProgressArgs>? OnChannelPollProgress;
        /// <inheritdoc/>
        public event EventHandler<ChannelPredictionBeginArgs>? OnChannelPredictionBegin;
        /// <inheritdoc/>
        public event EventHandler<ChannelPredictionEndArgs>? OnChannelPredictionEnd;
        /// <inheritdoc/>
        public event EventHandler<ChannelPredictionLockArgs>? OnChannelPredictionLock;
        /// <inheritdoc/>
        public event EventHandler<ChannelPredictionProgressArgs>? OnChannelPredictionProgress;
        /// <inheritdoc/>
        public event EventHandler<ChannelRaidArgs>? OnChannelRaid;
        /// <inheritdoc/>
        public event EventHandler<ChannelShieldModeBeginArgs>? OnChannelShieldModeBegin;
        /// <inheritdoc/>
        public event EventHandler<ChannelShieldModeEndArgs>? OnChannelShieldModeEnd;
        /// <inheritdoc/>
        public event EventHandler<ChannelShoutoutCreateArgs>? OnChannelShoutoutCreate;
        /// <inheritdoc/>
        public event EventHandler<ChannelShoutoutReceiveArgs>? OnChannelShoutoutReceive;
        /// <inheritdoc/>
        public event EventHandler<ChannelSubscribeArgs>? OnChannelSubscribe;
        /// <inheritdoc/>
        public event EventHandler<ChannelSubscriptionEndArgs>? OnChannelSubscriptionEnd;
        /// <inheritdoc/>
        public event EventHandler<ChannelSubscriptionGiftArgs>? OnChannelSubscriptionGift;
        /// <inheritdoc/>
        public event EventHandler<ChannelSubscriptionMessageArgs>? OnChannelSubscriptionMessage;
        /// <inheritdoc/>
        public event EventHandler<ChannelUnbanArgs>? OnChannelUnban;
        /// <inheritdoc/>
        public event EventHandler<ChannelUpdateArgs>? OnChannelUpdate;
        /// <inheritdoc/>
        public event EventHandler<OnErrorArgs>? OnError;
        /// <inheritdoc/>
        public event EventHandler<DropEntitlementGrantArgs>? OnDropEntitlementGrant;
        /// <inheritdoc/>
        public event EventHandler<ExtensionBitsTransactionCreateArgs>? OnExtensionBitsTransactionCreate;
        /// <inheritdoc/>
        public event EventHandler<RevocationArgs>? OnRevocation;
        /// <inheritdoc/>
        public event EventHandler<StreamOfflineArgs>? OnStreamOffline;
        /// <inheritdoc/>
        public event EventHandler<StreamOnlineArgs>? OnStreamOnline;
        /// <inheritdoc/>
        public event EventHandler<UserAuthorizationGrantArgs>? OnUserAuthorizationGrant;
        /// <inheritdoc/>
        public event EventHandler<UserAuthorizationRevokeArgs>? OnUserAuthorizationRevoke;
        /// <inheritdoc/>
        public event EventHandler<UserUpdateArgs>? OnUserUpdate;
        /// <inheritdoc/>
        public event EventHandler<ChannelChatClearArgs>? OnChannelChatClear;
        /// <inheritdoc/>
        public event EventHandler<ChannelChatClearUserMessageArgs>? OnChannelChatClearUserMessage;
        /// <inheritdoc/>
        public event EventHandler<ChannelChatMessageArgs>? OnChannelChatMessage;
        /// <inheritdoc/>
        public event EventHandler<ChannelChatMessageDeleteArgs>? OnChannelChatMessageDelete;
        /// <inheritdoc/>
        public event EventHandler<ChannelChatNotificationArgs>? OnChannelChatNotification;
        
        
        public event EventHandler<UserWhisperMessageArgs>? OnUserWhisperMessage;

        /// <inheritdoc/>
        public async Task ProcessNotificationAsync(Dictionary<string, string> headers, Stream body)
        {
            try
            {
                if (!headers.TryGetValue("Twitch-Eventsub-Subscription-Type", out var subscriptionType))
                {
                    OnError?.Invoke(this, new OnErrorArgs { Reason = "Missing_Header", Message = "The Twitch-Eventsub-Subscription-Type header was not found" });
                    return;
                }
                if (!headers.TryGetValue("Twitch-Eventsub-Subscription-Version", out var subscriptionVersion))
                {
                    OnError?.Invoke(this, new OnErrorArgs { Reason = "Missing_Header", Message = "The Twitch-Eventsub-Subscription-Version header was not found" });
                    return;
                }

                switch ((subscriptionType, subscriptionVersion))
                {
                    case ("channel.ban", "1"):
                        var banNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelBan>>(body, _jsonSerializerOptions);
                        OnChannelBan?.Invoke(this, new ChannelBanArgs { Headers = headers, Notification = banNotification! });
                        break;
                    case ("channel.cheer", "1"):
                        var cheerNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelCheer>>(body, _jsonSerializerOptions);
                        OnChannelCheer?.Invoke(this, new ChannelCheerArgs { Headers = headers, Notification = cheerNotification! });
                        break;
                    case ("channel.charity_campaign.start", "1"):
                        var charityStartNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelCharityCampaignStart>>(body, _jsonSerializerOptions);
                        OnChannelCharityCampaignStart?.Invoke(this, new ChannelCharityCampaignStartArgs { Headers = headers, Notification = charityStartNotification! });
                        break;
                    case ("channel.charity_campaign.donate", "1"):
                        var charityDonationNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelCharityCampaignDonate>>(body, _jsonSerializerOptions);
                        OnChannelCharityCampaignDonate?.Invoke(this, new ChannelCharityCampaignDonateArgs { Headers = headers, Notification = charityDonationNotification! });
                        break;
                    case ("channel.charity_campaign.progress", "1"):
                        var charityProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelCharityCampaignProgress>>(body, _jsonSerializerOptions);
                        OnChannelCharityCampaignProgress?.Invoke(this, new ChannelCharityCampaignProgressArgs { Headers = headers, Notification = charityProgressNotification! });
                        break;
                    case ("channel.charity_campaign.stop", "1"):
                        var charityStopNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelCharityCampaignStop>>(body, _jsonSerializerOptions);
                        OnChannelCharityCampaignStop?.Invoke(this, new ChannelCharityCampaignStopArgs { Headers = headers, Notification = charityStopNotification! });
                        break;
                    case ("channel.follow", "2"):
                        var followNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelFollow>>(body, _jsonSerializerOptions);
                        OnChannelFollow?.Invoke(this, new ChannelFollowArgs { Headers = headers, Notification = followNotification! });
                        break;
                    case ("channel.goal.begin", "1"):
                        var goalBeginNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelGoalBegin>>(body, _jsonSerializerOptions);
                        OnChannelGoalBegin?.Invoke(this, new ChannelGoalBeginArgs { Headers = headers, Notification = goalBeginNotification! });
                        break;
                    case ("channel.goal.end", "1"):
                        var goalEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelGoalEnd>>(body, _jsonSerializerOptions);
                        OnChannelGoalEnd?.Invoke(this, new ChannelGoalEndArgs { Headers = headers, Notification = goalEndNotification! });
                        break;
                    case ("channel.goal.progress", "1"):
                        var goalProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelGoalProgress>>(body, _jsonSerializerOptions);
                        OnChannelGoalProgress?.Invoke(this, new ChannelGoalProgressArgs { Headers = headers, Notification = goalProgressNotification! });
                        break;
                    case ("channel.hype_train.begin", "1"):
                        var hypeTrainBeginNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<HypeTrainBegin>>(body, _jsonSerializerOptions);
                        OnChannelHypeTrainBegin?.Invoke(this, new ChannelHypeTrainBeginArgs { Headers = headers, Notification = hypeTrainBeginNotification! });
                        break;
                    case ("channel.hype_train.end", "1"):
                        var hypeTrainEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<HypeTrainEnd>>(body, _jsonSerializerOptions);
                        OnChannelHypeTrainEnd?.Invoke(this, new ChannelHypeTrainEndArgs { Headers = headers, Notification = hypeTrainEndNotification! });
                        break;
                    case ("channel.hype_train.progress", "1"):
                        var hypeTrainProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<HypeTrainProgress>>(body, _jsonSerializerOptions);
                        OnChannelHypeTrainProgress?.Invoke(this, new ChannelHypeTrainProgressArgs { Headers = headers, Notification = hypeTrainProgressNotification! });
                        break;
                    case ("channel.moderator.add", "1"):
                        var moderatorAddNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelModerator>>(body, _jsonSerializerOptions);
                        OnChannelModeratorAdd?.Invoke(this, new ChannelModeratorArgs { Headers = headers, Notification = moderatorAddNotification! });
                        break;
                    case ("channel.moderator.remove", "1"):
                        var moderatorRemoveNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelModerator>>(body, _jsonSerializerOptions);
                        OnChannelModeratorRemove?.Invoke(this, new ChannelModeratorArgs { Headers = headers, Notification = moderatorRemoveNotification! });
                        break;
                    case ("channel.channel_points_custom_reward.add", "1"):
                        var customRewardAddNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomReward>>(body, _jsonSerializerOptions);
                        OnChannelPointsCustomRewardAdd?.Invoke(this, new ChannelPointsCustomRewardArgs { Headers = headers, Notification = customRewardAddNotification! });
                        break;
                    case ("channel.channel_points_custom_reward.remove", "1"):
                        var customRewardRemoveNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomReward>>(body, _jsonSerializerOptions);
                        OnChannelPointsCustomRewardRemove?.Invoke(this, new ChannelPointsCustomRewardArgs { Headers = headers, Notification = customRewardRemoveNotification! });
                        break;
                    case ("channel.channel_points_custom_reward.update", "1"):
                        var customRewardUpdateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomReward>>(body, _jsonSerializerOptions);
                        OnChannelPointsCustomRewardUpdate?.Invoke(this, new ChannelPointsCustomRewardArgs { Headers = headers, Notification = customRewardUpdateNotification! });
                        break;
                    case ("channel.channel_points_custom_reward_redemption.add", "1"):
                        var customRewardRedemptionAddNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomRewardRedemption>>(body, _jsonSerializerOptions);
                        OnChannelPointsCustomRewardRedemptionAdd?.Invoke(this, new ChannelPointsCustomRewardRedemptionArgs { Headers = headers, Notification = customRewardRedemptionAddNotification! });
                        break;
                    case ("channel.channel_points_custom_reward_redemption.update", "1"):
                        var customRewardRedemptionUpdateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPointsCustomRewardRedemption>>(body, _jsonSerializerOptions);
                        OnChannelPointsCustomRewardRedemptionUpdate?.Invoke(this, new ChannelPointsCustomRewardRedemptionArgs { Headers = headers, Notification = customRewardRedemptionUpdateNotification! });
                        break;
                    case ("channel.poll.begin", "1"):
                        var pollBeginNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPollBegin>>(body, _jsonSerializerOptions);
                        OnChannelPollBegin?.Invoke(this, new ChannelPollBeginArgs { Headers = headers, Notification = pollBeginNotification! });
                        break;
                    case ("channel.poll.end", "1"):
                        var pollEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPollEnd>>(body, _jsonSerializerOptions);
                        OnChannelPollEnd?.Invoke(this, new ChannelPollEndArgs { Headers = headers, Notification = pollEndNotification! });
                        break;
                    case ("channel.poll.progress", "1"):
                        var pollProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPollProgress>>(body, _jsonSerializerOptions);
                        OnChannelPollProgress?.Invoke(this, new ChannelPollProgressArgs { Headers = headers, Notification = pollProgressNotification! });
                        break;
                    case ("channel.prediction.begin", "1"):
                        var predictionBeginNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPredictionBegin>>(body, _jsonSerializerOptions);
                        OnChannelPredictionBegin?.Invoke(this, new ChannelPredictionBeginArgs { Headers = headers, Notification = predictionBeginNotification! });
                        break;
                    case ("channel.prediction.end", "1"):
                        var predictionEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPredictionEnd>>(body, _jsonSerializerOptions);
                        OnChannelPredictionEnd?.Invoke(this, new ChannelPredictionEndArgs { Headers = headers, Notification = predictionEndNotification! });
                        break;
                    case ("channel.prediction.lock", "1"):
                        var predictionLockNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPredictionLock>>(body, _jsonSerializerOptions);
                        OnChannelPredictionLock?.Invoke(this, new ChannelPredictionLockArgs { Headers = headers, Notification = predictionLockNotification! });
                        break;
                    case ("channel.prediction.progress", "1"):
                        var predictionProgressNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelPredictionProgress>>(body, _jsonSerializerOptions);
                        OnChannelPredictionProgress?.Invoke(this, new ChannelPredictionProgressArgs { Headers = headers, Notification = predictionProgressNotification! });
                        break;
                    case ("channel.raid", "1"):
                        var raidNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelRaid>>(body, _jsonSerializerOptions);
                        OnChannelRaid?.Invoke(this, new ChannelRaidArgs { Headers = headers, Notification = raidNotification! });
                        break;
                    case ("channel.shield_mode.begin", "1"):
                        var shieldBeginNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelShieldModeBegin>>(body, _jsonSerializerOptions);
                        OnChannelShieldModeBegin?.Invoke(this, new ChannelShieldModeBeginArgs { Headers = headers, Notification = shieldBeginNotification! });
                        break;
                    case ("channel.shield_mode.end", "1"):
                        var shieldEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelShieldModeEnd>>(body, _jsonSerializerOptions);
                        OnChannelShieldModeEnd?.Invoke(this, new ChannelShieldModeEndArgs { Headers = headers, Notification = shieldEndNotification! });
                        break;
                    case  ("channel.shoutout.create", "1"):
                        var shoutoutCreateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelShoutoutCreate>>(body, _jsonSerializerOptions);
                        OnChannelShoutoutCreate?.Invoke(this, new ChannelShoutoutCreateArgs { Headers = headers, Notification = shoutoutCreateNotification! });
                        break;
                    case ("channel.shoutout.receive", "1"):
                        var shoutoutReceiveNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelShoutoutReceive>>(body, _jsonSerializerOptions);
                        OnChannelShoutoutReceive?.Invoke(this, new ChannelShoutoutReceiveArgs { Headers = headers, Notification = shoutoutReceiveNotification! });
                        break;
                    case ("channel.subscribe", "1"):
                        var subscribeNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelSubscribe>>(body, _jsonSerializerOptions);
                        OnChannelSubscribe?.Invoke(this, new ChannelSubscribeArgs { Headers = headers, Notification = subscribeNotification! });
                        break;
                    case ("channel.subscription.end", "1"):
                        var subscriptionEndNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelSubscriptionEnd>>(body, _jsonSerializerOptions);
                        OnChannelSubscriptionEnd?.Invoke(this, new ChannelSubscriptionEndArgs { Headers = headers, Notification = subscriptionEndNotification! });
                        break;
                    case ("channel.subscription.gift", "1"):
                        var subscriptionGiftNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelSubscriptionGift>>(body, _jsonSerializerOptions);
                        OnChannelSubscriptionGift?.Invoke(this, new ChannelSubscriptionGiftArgs { Headers = headers, Notification = subscriptionGiftNotification! });
                        break;
                    case ("channel.subscription.message", "1"):
                        var subscriptionMessageNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelSubscriptionMessage>>(body, _jsonSerializerOptions);
                        OnChannelSubscriptionMessage?.Invoke(this, new ChannelSubscriptionMessageArgs { Headers = headers, Notification = subscriptionMessageNotification! });
                        break;
                    case ("channel.unban", "1"):
                        var unbanNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelUnban>>(body, _jsonSerializerOptions);
                        OnChannelUnban?.Invoke(this, new ChannelUnbanArgs { Headers = headers, Notification = unbanNotification! });
                        break;
                    case ("channel.update", "2"):
                        var channelUpdateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelUpdate>>(body, _jsonSerializerOptions);
                        OnChannelUpdate?.Invoke(this, new ChannelUpdateArgs { Headers = headers, Notification = channelUpdateNotification! });
                        break;
                    case ("drop.entitlement.grant", "1"):
                        var dropGrantNotification = await JsonSerializer.DeserializeAsync<BatchedNotificationPayload<DropEntitlementGrant>>(body, _jsonSerializerOptions);
                        OnDropEntitlementGrant?.Invoke(this, new DropEntitlementGrantArgs { Headers = headers, Notification = dropGrantNotification! });
                        break;
                    case ("extension.bits_transaction.create", "1"):
                        var extBitsTransactionCreateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ExtensionBitsTransactionCreate>>(body, _jsonSerializerOptions);
                        OnExtensionBitsTransactionCreate?.Invoke(this, new ExtensionBitsTransactionCreateArgs { Headers = headers, Notification = extBitsTransactionCreateNotification! });
                        break;
                    case ("stream.offline", "1"):
                        var streamOfflineNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<StreamOffline>>(body, _jsonSerializerOptions);
                        OnStreamOffline?.Invoke(this, new StreamOfflineArgs { Headers = headers, Notification = streamOfflineNotification! });
                        break;
                    case ("stream.online", "1"):
                        var streamOnlineNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<StreamOnline>>(body, _jsonSerializerOptions);
                        OnStreamOnline?.Invoke(this, new StreamOnlineArgs { Headers = headers, Notification = streamOnlineNotification! });
                        break;
                    case ("user.authorization.grant", "1"):
                        var userAuthGrantNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<UserAuthorizationGrant>>(body, _jsonSerializerOptions);
                        OnUserAuthorizationGrant?.Invoke(this, new UserAuthorizationGrantArgs { Headers = headers, Notification = userAuthGrantNotification! });
                        break;
                    case ("user.authorization.revoke", "1"):
                        var userAuthRevokeNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<UserAuthorizationRevoke>>(body, _jsonSerializerOptions);
                        OnUserAuthorizationRevoke?.Invoke(this, new UserAuthorizationRevokeArgs { Headers = headers, Notification = userAuthRevokeNotification! });
                        break;
                    case ("user.update", "1"):
                        var userUpdateNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<UserUpdate>>(body, _jsonSerializerOptions);
                        OnUserUpdate?.Invoke(this, new UserUpdateArgs { Headers = headers, Notification = userUpdateNotification! });
                        break;
                    case ("channel.chat.clear", "1"):
                        var channelChatClearNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelChatClear>>(body, _jsonSerializerOptions);
                        OnChannelChatClear?.Invoke(this, new ChannelChatClearArgs { Headers = headers, Notification = channelChatClearNotification! });
                        break;
                    case ("channel.chat.clear_user_messages", "1"):
                        var channelChatClearUserMessageNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelChatClearUserMessage>>(body, _jsonSerializerOptions);
                        OnChannelChatClearUserMessage?.Invoke(this, new ChannelChatClearUserMessageArgs() { Headers = headers, Notification = channelChatClearUserMessageNotification! });
                        break;
                    case ("channel.chat.message", "1"):
                        var channelChatMessageNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelChatMessage>>(body, _jsonSerializerOptions);
                        OnChannelChatMessage?.Invoke(this, new ChannelChatMessageArgs { Headers = headers, Notification = channelChatMessageNotification! });
                        break;
                    case ("channel.chat.message_delete", "1"):
                        var channelChatMessageDeleteNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelChatMessageDelete>>(body, _jsonSerializerOptions);
                        OnChannelChatMessageDelete?.Invoke(this, new ChannelChatMessageDeleteArgs { Headers = headers, Notification = channelChatMessageDeleteNotification! });
                        break;
                    case ("channel.chat.notification", "1"):
                        var channelChatNotificationNotification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<ChannelChatNotification>>(body, _jsonSerializerOptions);
                        OnChannelChatNotification?.Invoke(this, new ChannelChatNotificationArgs { Headers = headers, Notification = channelChatNotificationNotification! });
                        break;
                    case ("user.whisper.message", "1"):
                        var userWhisperMessage = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<UserWhisperMessage>>(body, _jsonSerializerOptions);
                        OnUserWhisperMessage?.Invoke(this, new UserWhisperMessageArgs { Headers = headers, Notification = userWhisperMessage! });
                        break;
                    default:
                        OnError?.Invoke(this, new OnErrorArgs { Reason = "Unknown_Subscription_Type", Message = $"Cannot parse unknown subscription type {subscriptionType}" });
                        break;
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }
        }

        /// <inheritdoc/>
        public async Task ProcessRevocationAsync(Dictionary<string, string> headers, Stream body)
        {
            try
            {
                var notification = await JsonSerializer.DeserializeAsync<EventSubNotificationPayload<object>>(body, _jsonSerializerOptions);
                OnRevocation?.Invoke(this, new RevocationArgs { Headers = headers, Notification = notification! });
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new OnErrorArgs { Reason = "Application_Error", Message = ex.Message });
            }
        }
    }
}