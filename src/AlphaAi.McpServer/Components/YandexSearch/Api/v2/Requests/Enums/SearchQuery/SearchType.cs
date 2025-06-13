using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.SearchQuery;

/// <summary>
///     Search type that determines the domain name that will be used for the search queries.
/// </summary>
public enum SearchType
{
    /// <summary>
    ///     Russian search type (default), yandex.ru search domain name will be used.
    /// </summary>
    [JsonStringEnumMemberName("SEARCH_TYPE_RU")]
    SEARCH_TYPE_RU = 0,

    /// <summary>
    ///     Turkish search type, yandex.tr search domain name will be used.
    /// </summary>
    [JsonStringEnumMemberName("SEARCH_TYPE_TR")]
    SEARCH_TYPE_TR = 1,

    /// <summary>
    ///     International search type, yandex.com search domain name will be used.
    /// </summary>
    [JsonStringEnumMemberName("SEARCH_TYPE_COM")]
    SEARCH_TYPE_COM = 2,

    /// <summary>
    ///     Kazakh search type, yandex.kz search domain name will be used.
    /// </summary>
    [JsonStringEnumMemberName("SEARCH_TYPE_KK")]
    SEARCH_TYPE_KK = 3,

    /// <summary>
    ///     Belarusian search type, yandex.by search domain name will be used.
    /// </summary>
    [JsonStringEnumMemberName("SEARCH_TYPE_BE")]
    SEARCH_TYPE_BE = 4,

    /// <summary>
    ///     Uzbek search type, yandex.uz search domain name will be used.
    /// </summary>
    [JsonStringEnumMemberName("SEARCH_TYPE_UZ")]
    SEARCH_TYPE_UZ = 5
}
