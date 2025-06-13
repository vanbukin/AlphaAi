using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.SearchQuery;

/// <summary>
///     Rule for filtering search results and determines whether any documents should be excluded.
/// </summary>
public enum FamilyMode
{
    /// <summary>
    ///     Filtering is disabled. Search results include any documents regardless of their contents.
    /// </summary>
    [JsonStringEnumMemberName("FAMILY_MODE_NONE")]
    FAMILY_MODE_NONE = 0,

    /// <summary>
    ///     Moderate filter (default value). Documents of the Adult category are excluded from search results unless a query is explicitly made for searching resources of this category.
    /// </summary>
    [JsonStringEnumMemberName("FAMILY_MODE_MODERATE")]
    FAMILY_MODE_MODERATE = 1,

    /// <summary>
    ///     Regardless of a search query, documents of the Adult category and those with profanity are excluded from search results.
    /// </summary>
    [JsonStringEnumMemberName("FAMILY_MODE_STRICT")]
    FAMILY_MODE_STRICT = 2
}
