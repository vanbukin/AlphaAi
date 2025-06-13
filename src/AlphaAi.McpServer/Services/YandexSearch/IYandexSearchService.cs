using System.Threading;
using System.Threading.Tasks;

namespace AlphaAi.McpServer.Services.YandexSearch;

public interface IYandexSearchService
{
    Task<string?> SearchAsync(string query, CancellationToken cancellationToken);
}
