using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TwitchLib.EventSub.Core;
using TwitchLib.EventSub.Webhooks.Core.EventArgs;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Automod;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Channel;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Conduit;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Drop;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Extension;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Stream;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.User;

namespace TwitchLib.EventSub.Webhooks.Core
{
    /// <summary>
    /// Class where everything runs together
    /// <para>Listen to events from EventSub from this class</para>
    /// </summary>
    public interface IEventSubWebhooks
    {
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
        /// Event that triggers on "channel.ban" notifications
        /// </summary>
        event AsyncEventHandler<ChannelBanArgs>? OnChannelBan;
        /// <summary>
        /// Event that triggers on "channel.cheer" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCheerArgs>? OnChannelCheer;
        /// <summary>
        /// Event that triggers on "channel.charity_campaign.start" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCharityCampaignStartArgs>? OnChannelCharityCampaignStart;
        /// <summary>
        /// Event that triggers on "channel.charity_campaign.donate" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCharityCampaignDonateArgs>? OnChannelCharityCampaignDonate;
        /// <summary>
        /// Event that triggers on "channel.charity_campaign.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCharityCampaignProgressArgs>? OnChannelCharityCampaignProgress;
        /// <summary>
        /// Event that triggers on "channel.charity_campaign.stop" notifications
        /// </summary>
        event AsyncEventHandler<ChannelCharityCampaignStopArgs>? OnChannelCharityCampaignStop;
        /// <summary>
        /// Event that triggers on "channel.follow" notifications
        /// </summary>
        event AsyncEventHandler<ChannelFollowArgs>? OnChannelFollow;
        /// <summary>
        /// Event that triggers on "channel.goal.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalBeginArgs>? OnChannelGoalBegin;
        /// <summary>
        /// Event that triggers on "channel.goal.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalEndArgs>? OnChannelGoalEnd;
        /// <summary>
        /// Event that triggers on "channel.goal.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelGoalProgressArgs>? OnChannelGoalProgress;
        /// <summary>
        /// Event that triggers on "channel.hype_train.begin" notifications
        /// </summary>
        [Obsolete("This event is deprecated, please use: OnChannelHypeTrainBeginV2")]
        event AsyncEventHandler<ChannelHypeTrainBeginArgs>? OnChannelHypeTrainBegin;
        /// <summary>
        /// Event that triggers on "channel.hype_train.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainBeginV2Args>? OnChannelHypeTrainBeginV2;
        /// <summary>
        /// Event that triggers on "channel.hype_train.end" notifications
        /// </summary>
        [Obsolete("This event is deprecated, please use: OnChannelHypeTrainEndV2")]
        event AsyncEventHandler<ChannelHypeTrainEndArgs>? OnChannelHypeTrainEnd;
        /// <summary>
        /// Event that triggers on "channel.hype_train.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainEndV2Args>? OnChannelHypeTrainEndV2;
        /// <summary>
        /// Event that triggers on "channel.hype_train.progress" notifications
        /// </summary>
        [Obsolete("This event is deprecated, please use: OnChannelHypeTrainProgressV2")]
        event AsyncEventHandler<ChannelHypeTrainProgressArgs>? OnChannelHypeTrainProgress;
        /// <summary>
        /// Event that triggers on "channel.hype_train.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelHypeTrainProgressV2Args>? OnChannelHypeTrainProgressV2;
        /// <summary>
        /// Event that triggers on "channel.moderator.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelModeratorArgs>? OnChannelModeratorAdd;
        /// <summary>
        /// Event that triggers on "channel.moderator.remove" notifications
        /// </summary>
        event AsyncEventHandler<ChannelModeratorArgs>? OnChannelModeratorRemove;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardAdd;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardUpdate;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward.remove" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardArgs>? OnChannelPointsCustomRewardRemove;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward_redemption.add" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionAdd;
        /// <summary>
        /// Event that triggers on "channel.channel_points_custom_reward_redemption.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPointsCustomRewardRedemptionArgs>? OnChannelPointsCustomRewardRedemptionUpdate;
        /// <summary>
        /// Event that triggers on "channel.poll.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollBeginArgs>? OnChannelPollBegin;
        /// <summary>
        /// Event that triggers on "channel.poll.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollEndArgs>? OnChannelPollEnd;
        /// <summary>
        /// Event that triggers on "channel.poll.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPollProgressArgs>? OnChannelPollProgress;
        /// <summary>
        /// Event that triggers on "channel.prediction.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionBeginArgs>? OnChannelPredictionBegin;
        /// <summary>
        /// Event that triggers on "channel.prediction.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionEndArgs>? OnChannelPredictionEnd;
        /// <summary>
        /// Event that triggers on "channel.prediction.lock" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionLockArgs>? OnChannelPredictionLock;
        /// <summary>
        /// Event that triggers on "channel.prediction.progress" notifications
        /// </summary>
        event AsyncEventHandler<ChannelPredictionProgressArgs>? OnChannelPredictionProgress;
        /// <summary>
        /// Event that triggers on "channel.raid" notifications
        /// </summary>
        event AsyncEventHandler<ChannelRaidArgs>? OnChannelRaid;
        /// <summary>
        /// Event that triggers on "channel.shield_mode.begin" notifications
        /// </summary>
        event AsyncEventHandler<ChannelShieldModeBeginArgs>? OnChannelShieldModeBegin;
        /// <summary>
        /// Event that triggers on "channel.shield_mode.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelShieldModeEndArgs>? OnChannelShieldModeEnd;
        /// <summary>
        /// Event that triggers on "channel.shoutout.create" notifications
        /// </summary>
        event AsyncEventHandler<ChannelShoutoutCreateArgs>? OnChannelShoutoutCreate;
        /// <summary>
        /// Event that triggers on "channel.shoutout.receive" notifications
        /// </summary>
        event AsyncEventHandler<ChannelShoutoutReceiveArgs>? OnChannelShoutoutReceive;
        /// <summary>
        /// Event that triggers on "channel.subscribe" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscribeArgs>? OnChannelSubscribe;
        /// <summary>
        /// Event that triggers on "channel.subscription.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionEndArgs>? OnChannelSubscriptionEnd;
        /// <summary>
        /// Event that triggers on "channel.subscription.gift" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionGiftArgs>? OnChannelSubscriptionGift;
        /// <summary>
        /// Event that triggers on "channel.subscription.end" notifications
        /// </summary>
        event AsyncEventHandler<ChannelSubscriptionMessageArgs>? OnChannelSubscriptionMessage;
        /// <summary>
        /// Event that triggers on "channel.unban" notifications
        /// </summary>
        event AsyncEventHandler<ChannelUnbanArgs>? OnChannelUnban;
        /// <summary>
        /// Event that triggers on "channel.update" notifications
        /// </summary>
        event AsyncEventHandler<ChannelUpdateArgs>? OnChannelUpdate;
        /// <summary>
        /// Event that triggers if an error parsing a notification or revocation was encountered
        /// </summary>
        event AsyncEventHandler<OnErrorArgs>? OnError;
        /// <summary>
        /// Event that triggers on "conduit.shard.disabled" notifications
        /// </summary>
        event AsyncEventHandler<ConduitShardDisabledArgs>? ConduitShardDisabled;
        /// <summary>
        /// Event that triggers on "drop.entitlement.grant" notifications
        /// </summary>
        event AsyncEventHandler<DropEntitlementGrantArgs>? OnDropEntitlementGrant;
        /// <summary>
        /// Event that triggers on "extension.bits_transaction.create" notifications
        /// </summary>
        event AsyncEventHandler<ExtensionBitsTransactionCreateArgs>? OnExtensionBitsTransactionCreate;
        /// <summary>
        /// Event that triggers on if a revocation notification was received
        /// </summary>
        event AsyncEventHandler<RevocationArgs>? OnRevocation;
        /// <summary>
        /// Event that triggers on "stream.offline" notifications
        /// </summary>
        event AsyncEventHandler<StreamOfflineArgs>? OnStreamOffline;
        /// <summary>
        /// Event that triggers on "stream.online" notifications
        /// </summary>
        event AsyncEventHandler<StreamOnlineArgs>? OnStreamOnline;
        /// <summary>
        /// Event that triggers on "user.authorization.grant" notifications
        /// </summary>
        event AsyncEventHandler<UserAuthorizationGrantArgs>? OnUserAuthorizationGrant;
        /// <summary>
        /// Event that triggers on "user.authorization.revoke" notifications
        /// </summary>
        event AsyncEventHandler<UserAuthorizationRevokeArgs>? OnUserAuthorizationRevoke;
        /// <summary>
        /// Event that triggers on "user.update" notifications
        /// </summary>
        event AsyncEventHandler<UserUpdateArgs>? OnUserUpdate;
        /// <summary>
        /// Event that triggers on "channel.chat.clear" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatClearArgs>? OnChannelChatClear;
        /// <summary>
        /// Event that triggers on "channel.chat.clear_user_messages" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatClearUserMessageArgs>? OnChannelChatClearUserMessage;
        /// <summary>
        /// Event that triggers on "channel.chat.message" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatMessageArgs>? OnChannelChatMessage;
        /// <summary>
        /// Event that triggers on "channel.chat.message_delete" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatMessageDeleteArgs>? OnChannelChatMessageDelete;
        /// <summary>
        /// Event that triggers on "channel.chat.notification" notifications
        /// </summary>
        event AsyncEventHandler<ChannelChatNotificationArgs>? OnChannelChatNotification;
        

        /// <summary>
        /// Processes "notification" type messages. You should not use this in your code, its for internal use only!
        /// </summary>
        /// <param name="headers">Dictionary of the request headers</param>
        /// <param name="body">Stream of the request body</param>
        Task ProcessNotificationAsync(Dictionary<string, string> headers, Stream body);
        /// <summary>
        /// Processes "revocation" type messages. You should not use this in your code, its for internal use only!
        /// </summary>
        /// <param name="headers">Dictionary of the request headers</param>
        /// <param name="body">Stream of the request body</param>
        Task ProcessRevocationAsync(Dictionary<string, string> headers, Stream body);
    }
}
