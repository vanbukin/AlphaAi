using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.SortSpec;

/// <summary>
///     Documents sorting mode.
/// </summary>
public enum SortMode
{
    /// <summary>
    ///     Sort documents by relevance (default value).
    /// </summary>
    [JsonStringEnumMemberName("SORT_MODE_BY_RELEVANCE")]
    SORT_MODE_BY_RELEVANCE = 0,

    /// <summary>
    ///     Sort documents by update time.
    /// </summary>
    [JsonStringEnumMemberName("SORT_MODE_BY_TIME")]
    SORT_MODE_BY_TIME = 1
}
