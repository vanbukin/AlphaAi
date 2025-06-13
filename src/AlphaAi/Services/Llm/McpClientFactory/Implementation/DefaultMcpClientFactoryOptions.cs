using System;

namespace AlphaAi.Services.Llm.McpClientFactory.Implementation;

public class DefaultMcpClientFactoryOptions
{
    public DefaultMcpClientFactoryOptions(Uri endpoint)
    {
        ArgumentNullException.ThrowIfNull(endpoint);
        Endpoint = endpoint;
    }

    public Uri Endpoint { get; }
}
