using System.Threading;
using System.Threading.Tasks;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Responses;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2;

public interface IYandexSearchApiClient
{
    Task<YandexSearchResponse?> SearchAsync(Requests.YandexSearch search, CancellationToken cancellationToken);
}
