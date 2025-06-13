using System.Text.Json.Serialization;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.SortSpec;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests;

/// <summary>
///     The rules for sorting search results that define the sequence of the returned search results.
/// </summary>
public class YandexSortSpec
{
    /// <summary>
    ///     The rules for sorting search results that define the sequence of the returned search results.
    /// </summary>
    /// <param name="sortMode">Documents sorting mode.</param>
    /// <param name="sortOrder">Documents sorting order.</param>
    public YandexSortSpec(SortMode? sortMode, SortOrder? sortOrder)
    {
        SortMode = sortMode;
        SortOrder = sortOrder;
    }

    /// <summary>
    ///     Documents sorting mode.
    /// </summary>
    [JsonPropertyName("sortMode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SortMode? SortMode { get; }

    /// <summary>
    ///     Documents sorting order.
    /// </summary>
    [JsonPropertyName("sortOrder")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SortOrder? SortOrder { get; }
}
