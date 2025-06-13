using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Responses;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2;

public class DefaultYandexSearchApiClient : IYandexSearchApiClient
{
    private readonly HttpClient _httpClient;

    public DefaultYandexSearchApiClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    public async Task<YandexSearchResponse?> SearchAsync(Requests.YandexSearch search, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        YandexSearchResponse? result;
        using (var response = await _httpClient.PostAsJsonAsync(
                   "https://searchapi.api.cloud.yandex.net/v2/web/search",
                   search,
                   cancellationToken))
        {
            response.EnsureSuccessStatusCode();
            result = await response.Content.ReadFromJsonAsync<YandexSearchResponse>(cancellationToken);
        }

        return result;
    }
}
