using System;
using AlphaAi.Configuration.Options.Llm;

namespace AlphaAi.Configuration.TypedConfigurations.Llm;

public class LlmConfiguration
{
    private LlmConfiguration(
        Uri endpoint,
        string apiKey,
        string model,
        string botName,
        LlmTokensConfiguration tokens)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(apiKey);
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(botName);
        ArgumentNullException.ThrowIfNull(tokens);
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(apiKey));
        }

        if (string.IsNullOrEmpty(model))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(model));
        }

        if (string.IsNullOrEmpty(botName))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(botName));
        }

        Endpoint = endpoint;
        ApiKey = apiKey;
        Model = model;
        BotName = botName;
        Tokens = tokens;
    }

    public Uri Endpoint { get; }
    public string ApiKey { get; }
    public string Model { get; }
    public string BotName { get; }
    public LlmTokensConfiguration Tokens { get; }

    public static LlmConfiguration Convert(LlmOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (!Uri.TryCreate(options.Endpoint, UriKind.Absolute, out var typedEndpoint))
        {
            throw new ArgumentException("Invalid endpoint.", nameof(options));
        }

        var tokens = LlmTokensConfiguration.Convert(options.Tokens);

        return new(typedEndpoint, options.ApiKey, options.Model, options.BotName, tokens);
    }
}
