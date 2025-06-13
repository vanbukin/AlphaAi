using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;

namespace AlphaAi.Services.Llm.McpClientFactory.Implementation;

public class DefaultMcpClientFactory : IMcpClientFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly DefaultMcpClientFactoryOptions _options;

    public DefaultMcpClientFactory(DefaultMcpClientFactoryOptions options, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(loggerFactory);
        _options = options;
        _loggerFactory = loggerFactory;
    }

    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope")]
    public async Task<IMcpClient> CreateAsync(CancellationToken cancellationToken)
    {
        return await ModelContextProtocol.Client.McpClientFactory.CreateAsync(
            new SseClientTransport(
                new()
                {
                    Endpoint = _options.Endpoint
                }),
            null,
            _loggerFactory,
            cancellationToken);
    }
}
