using System;
using System.Diagnostics.CodeAnalysis;
using AlphaAi.Configuration.Options.Llm;

namespace AlphaAi.Configuration.TypedConfigurations.Llm;

public class LlmMcpConfiguration
{
    private LlmMcpConfiguration(bool enabled, Uri? endpoint)
    {
        Enabled = enabled;
        Endpoint = endpoint;
    }

    [MemberNotNullWhen(true, nameof(Endpoint))]
    public bool Enabled { get; }

    public Uri? Endpoint { get; }

    public static LlmMcpConfiguration Convert(LlmMcpOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (!options.Enabled)
        {
            return new(false, null);
        }

        if (!Uri.TryCreate(options.Endpoint, UriKind.Absolute, out var typedEndpoint))
        {
            throw new ArgumentException("Invalid endpoint.", nameof(options));
        }

        return new(true, typedEndpoint);
    }
}
