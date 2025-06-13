using System;

namespace AlphaAi.Services.Llm.ChatClientFactory.Implementation;

public class DefaultChatClientFactoryOptions
{
    public DefaultChatClientFactoryOptions(Uri endpoint, string apiKey, string model)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(apiKey);
        ArgumentNullException.ThrowIfNull(model);
        Endpoint = endpoint;
        ApiKey = apiKey;
        Model = model;
    }

    public Uri Endpoint { get; }

    public string ApiKey { get; }

    public string Model { get; }
}
