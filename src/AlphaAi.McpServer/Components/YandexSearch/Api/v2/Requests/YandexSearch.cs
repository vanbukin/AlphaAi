using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests;

/// <summary>
///     Web Search request
/// </summary>
public class YandexSearch
{
    public YandexSearch(
        YandexSearchQuery query,
        YandexSortSpec? sortSpec,
        YandexGroupSpec? groupSpec,
        long? maxPassages,
        string? region,
        Localization? l10N,
        string? folderId,
        string? userAgent)
    {
        Query = query;
        SortSpec = sortSpec;
        GroupSpec = groupSpec;
        MaxPassages = maxPassages;
        Region = region;
        L10n = l10N;
        FolderId = folderId;
        UserAgent = userAgent;
    }

    /// <summary>
    ///     Required field. Search query.
    /// </summary>
    [JsonPropertyName("query")]
    [Required]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public YandexSearchQuery Query { get; }

    /// <summary>
    ///     The rules for sorting search results that define the sequence of the returned search results.
    /// </summary>
    [JsonPropertyName("sortSpec")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public YandexSortSpec? SortSpec { get; }

    /// <summary>
    ///     Grouping settings that are used to group documents from a single domain into a container.
    /// </summary>
    [JsonPropertyName("groupSpec")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public YandexGroupSpec? GroupSpec { get; }

    /// <summary>
    ///     The maximum number of passages that can be used when generating a document snippet.
    /// </summary>
    [JsonPropertyName("maxPassages")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? MaxPassages { get; }

    /// <summary>
    ///     ID of the search country or region that impacts the document ranking rules.
    /// </summary>
    [JsonPropertyName("region")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Region { get; }

    /// <summary>
    ///     The notification language for a search response.
    /// </summary>
    [JsonPropertyName("l10n")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Localization? L10n { get; }

    /// <summary>
    ///     ID of the folder.
    /// </summary>
    [JsonPropertyName("folderId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FolderId { get; }

    /// <summary>
    ///     User-Agent request header value.
    /// </summary>
    [JsonPropertyName("userAgent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? UserAgent { get; }
}
