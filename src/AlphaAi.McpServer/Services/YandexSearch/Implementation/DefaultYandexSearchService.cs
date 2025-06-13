using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.GroupSpec;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.SearchQuery;

namespace AlphaAi.McpServer.Services.YandexSearch.Implementation;

public class DefaultYandexSearchService : IYandexSearchService
{
    private readonly IYandexSearchApiClient _apiClient;
    private readonly DefaultYandexSearchServiceOptions _options;

    public DefaultYandexSearchService(DefaultYandexSearchServiceOptions options, IYandexSearchApiClient apiClient)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(apiClient);
        _options = options;
        _apiClient = apiClient;
    }

    public async Task<string?> SearchAsync(string query, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var search = new Components.YandexSearch.Api.v2.Requests.YandexSearch(new(
                SearchType.SEARCH_TYPE_RU,
                query,
                FamilyMode.FAMILY_MODE_NONE,
                null,
                FixTypoMode.FIX_TYPO_MODE_ON),
            null,
            new(
                GroupMode.GROUP_MODE_FLAT,
                3,
                1),
            null,
            null,
            Localization.LOCALIZATION_RU,
            _options.FolderId,
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36");
        var result = await _apiClient.SearchAsync(search, cancellationToken);
        if (result is null)
        {
            return null;
        }

        var binaryXml = Convert.FromBase64String(result.RawData);
        var stringXml = Encoding.UTF8.GetString(binaryXml);
        return stringXml;
    }
}
