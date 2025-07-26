# TwitchLib.EventSub.Webhooks
Provides an easy way to setup a Twitch EventSub Webhooks Server

Setting up a Twitch EventSub server can be daunting and has some moving parts that you could get wrong.
TwitchLib.EventSub.Webhooks was build with that in mind and makes it as easy as it can get.
You only need a few lines of code to add and configure it.

## Installation

| NuGet            |       | [![TwitchLib.EventSub.Webhooks][1]][2]                                       |
| :--------------- | ----: | :--------------------------------------------------------------------------- |
| Package Manager  | `PM>` | `Install-Package TwitchLib.EventSub.Webhooks -Version 3.0.0`                 |
| .NET CLI         | `>`   | `dotnet add package TwitchLib.EventSub.Webhooks --version 3.0.0`             |
| PackageReference |       | `<PackageReference Include="TwitchLib.EventSub.Webhooks" Version="3.0.0" />` |
| Paket CLI        | `>`   | `paket add TwitchLib.EventSub.Webhooks --version 3.0.0`                      |

[1]: https://img.shields.io/nuget/v/TwitchLib.EventSub.Webhooks.svg?label=TwitchLib.EventSub.Webhooks
[2]: https://www.nuget.org/packages/TwitchLib.EventSub.Webhooks

## Breaking Changes in Version 3.0
- Removed deprecated versions of .NET.
- Events are now asynchronous (return value changed from `void` to `Task`)
- Events dropped the `On` prefix (`OnChannelChatMessage` => `ChannelChatMessage`)
- All EventSub events were moved to `TwitchLib.EventSub.Core` Nuget Package, for better management across future EventSub transport Client libraries.
  That means their namespace changed from `TwitchLib.EventSub.Webhooks.Core.EventArgs.*` to `TwitchLib.EventSub.Core.EventArgs.*`.
- Like Events, all EventSub Models were moved to the `TwitchLib.EventSub.Core` package, (namespace changed from `TwitchLib.EventSub.Webhooks.Core.Models` to `TwitchLib.EventSub.Core.Models`)
  but to ensure that the models can be used across projects some changes had to be made:
    - `Notification` in `TwitchLibEventSubEventArgs<T>` were renamed to `Payload`
    - `Headers`(`Dictionary<string,string>`) in `TwitchLibEventSubEventArgs<T>` were replaced with `Metadata`(`EventSubMetadata`) and before you can access the values you have to cast it to `WebhookEventSubMetadata`
    - `EventSubSubscriptionTransport` was renamed to `EventSubTransport`

## Breaking Changes in Version 2.0

Version 2.0 contains some breaking changes.
- Subscription Types and their models were moved to their own shared Nuget Package `TwitchLib.EventSub.Core` for better management across future EventSub transport Client libraries
  That means their namespace changed from `TwitchLib.EventSub.Webhooks.Core.SubscriptionTypes` / `TwitchLib.EventSub.Webhooks.Core.Models` to `TwitchLib.EventSub.Core.SubscriptionTypes` / `TwitchLib.EventSub.Core.Models`
- Every use of `DateTime` internally and in models was changed to use `DateTimeOffset` instead
- `ITwitchEventSubWebhooks` / `TwitchEventSubWebhooks` were renamed to `IEventSubWebhooks`/ `EventSubWebhooks`

## Disclaimer

The usual requirements that Twitch has for EventSub webhooks do still apply!
- Your callback url needs to be publicly accessible (localhost wont work!)
- You need to have SSL on port 443 for the domain used as a callback.

## Setup

Step 1: Create a new ASP.NET Core project (.NET 8.0 and up)

Step 2: Install the TwitchLib.EventSub.Webhooks nuget package. (See above on how to do that)

Step 3: Add necessary services and config to the DI Container

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTwitchLibEventSubWebhooks(config =>
{
    config.CallbackPath = "/eventsub/";
    config.Secret = "supersecuresecret";
});
builder.Services.AddHostedService<EventSubHostedService>();
```

!!! If you follow these steps your callback url will `https://{your_domain}/eventsub/` !!!

Step 4: Put the TwitchLib.EventSub.Webhooks middleware in the request pipeline

```csharp
var app = builder.Build();

app.UseTwitchLibEventSubWebhooks();

app.Run();
```

Step 5: Create the HostedService and listen for events

```csharp
using TwitchLib.EventSub.Webhooks.Core;
using TwitchLib.EventSub.Webhooks.Core.EventArgs;
using TwitchLib.EventSub.Webhooks.Core.EventArgs.Channel;

namespace TwitchLib.EventSub.Webhooks.Example
{
    public class EventSubHostedService : IHostedService
    {
        private readonly ILogger<EventSubHostedService> _logger;
        private readonly IEventSubWebhooks _eventSubWebhooks;

        public EventSubHostedService(ILogger<EventSubHostedService> logger, IEventSubWebhooks eventSubWebhooks)
        {
            _logger = logger;
            _eventSubWebhooks = eventSubWebhooks;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventSubWebhooks.OnError += OnError;
            _eventSubWebhooks.OnChannelFollow += OnChannelFollow;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _eventSubWebhooks.OnError -= OnError;
            _eventSubWebhooks.OnChannelFollow -= OnChannelFollow;
            return Task.CompletedTask;
        }

        private void OnChannelFollow(object? sender, ChannelFollowArgs e)
        {
            _logger.LogInformation($"{e.Notification.Event.UserName} followed {e.Notification.Event.BroadcasterUserName} at {e.Notification.Event.FollowedAt.ToUniversalTime()}");
        }

        private void OnError(object? sender, OnErrorArgs e)
        {
            _logger.LogError($"Reason: {e.Reason} - Message: {e.Message}");
        }
    }
}
```


That is all that you need to do to setup a Twitch EventSub Webhook Server with TwitchLib.EventSub.Webhooks.
Easy isn't it?

Alternatively you can also just clone the https://github.com/TwitchLib/TwitchLib.EventSub.Webhooks/tree/master/TwitchLib.EventSub.Webhooks.Example
