using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.SearchQuery;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests;

/// <summary>
///     Search query.
/// </summary>
public class YandexSearchQuery
{
    /// <summary>
    ///     Search query.
    /// </summary>
    /// <param name="searchType">Required field. Search type that determines the domain name that will be used for the search queries.</param>
    /// <param name="queryText">Required field. Search query text</param>
    /// <param name="familyMode">Rule for filtering search results and determines whether any documents should be excluded.</param>
    /// <param name="page">The number of a requested page with search results</param>
    /// <param name="fixTypoMode">Typos autocorrections mode</param>
    public YandexSearchQuery(
        SearchType searchType,
        string queryText,
        FamilyMode? familyMode,
        long? page,
        FixTypoMode? fixTypoMode)
    {
        SearchType = searchType;
        QueryText = queryText;
        FamilyMode = familyMode;
        Page = page;
        FixTypoMode = fixTypoMode;
    }

    /// <summary>
    ///     Required field. Search type that determines the domain name that will be used for the search queries.
    /// </summary>
    [JsonPropertyName("searchType")]
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public SearchType SearchType { get; }

    /// <summary>
    ///     Required field. Search query text
    /// </summary>
    [JsonPropertyName("queryText")]
    [Required]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string QueryText { get; }

    /// <summary>
    ///     Rule for filtering search results and determines whether any documents should be excluded.
    /// </summary>
    [JsonPropertyName("familyMode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FamilyMode? FamilyMode { get; }

    /// <summary>
    ///     The number of a requested page with search results
    /// </summary>
    [JsonPropertyName("page")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? Page { get; }

    /// <summary>
    ///     Typos autocorrections mode
    /// </summary>
    [JsonPropertyName("fixTypoMode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FixTypoMode? FixTypoMode { get; }
}
