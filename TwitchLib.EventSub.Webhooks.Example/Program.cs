using TwitchLib.EventSub.Webhooks.Example;
using TwitchLib.EventSub.Webhooks.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTwitchLibEventSubWebhooks(config =>
{
    config.CallbackPath = "/eventsub/";
    config.Secret = "supersecuresecret";
    config.EnableLogging = true;
});
builder.Services.AddHostedService<EventSubHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseTwitchLibEventSubWebhooks();

app.MapGet("/", () => "Hello World!");

app.Run();
