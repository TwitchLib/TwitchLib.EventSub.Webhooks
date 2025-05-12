using TwitchLib.EventSub.Webhooks.Middlewares;

namespace TwitchLib.EventSub.Webhooks.Test;

public class UnitTest1
{
    [Fact]
    public void SignatureVerification_Succeeded()
    {
        var messageId = "65e98bd4-a057-11cd-187a-75e8f5b3fb89";
        var messageTimestamp = "2025-05-11T20:13:00.4746258Z";
        var messageSignature = "sha256=d821508901727be752420bbd78366545d6da62e68b101f38d37574bfbe2b627c";
        var messageBody = """
                    {"subscription":{"id":"d5b930eb-5333-a535-0e38-603bc2a22ce1","status":"enabled","type":"stream.offline","version":"1","condition":{"broadcaster_user_id":"88922321"},"transport":{"method":"webhook","callback":"null"},"created_at":"2025-05-11T20:13:00.4746258Z","cost":0},"event":{"broadcaster_user_id":"88922321","broadcaster_user_login":"testBroadcaster","broadcaster_user_name":"testBroadcaster"}}
                    """;
        var secret = "supersecuresecret"u8.ToArray();

        Assert.True(EventSubSignatureVerificationMiddleware.IsSignatureValid(messageSignature, messageId, messageTimestamp, messageBody, secret));
    }
}
