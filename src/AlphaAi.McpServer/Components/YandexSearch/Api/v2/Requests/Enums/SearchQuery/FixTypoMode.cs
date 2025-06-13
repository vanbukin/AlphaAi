using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.SearchQuery;

/// <summary>
///     Typos autocorrections mode
/// </summary>
public enum FixTypoMode
{
    /// <summary>
    ///     Automatically correct typos (default value).
    /// </summary>
    [JsonStringEnumMemberName("FIX_TYPO_MODE_ON")]
    FIX_TYPO_MODE_ON = 0,

    /// <summary>
    ///     Autocorrection is off.
    /// </summary>
    [JsonStringEnumMemberName("FIX_TYPO_MODE_OFF")]
    FIX_TYPO_MODE_OFF = 1
}
