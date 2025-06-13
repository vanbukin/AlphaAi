using System.Threading;
using System.Threading.Tasks;
using ModelContextProtocol.Client;

namespace AlphaAi.Services.Llm.McpClientFactory;

public interface IMcpClientFactory
{
    Task<IMcpClient> CreateAsync(CancellationToken cancellationToken);
}
