using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums;

/// <summary>
///     The notification language for a search response.
/// </summary>
public enum Localization
{
    /// <summary>
    ///     Russian (default value)
    /// </summary>
    [JsonStringEnumMemberName("LOCALIZATION_RU")]
    LOCALIZATION_RU = 0,

    /// <summary>
    ///     Ukrainian
    /// </summary>
    [JsonStringEnumMemberName("LOCALIZATION_UK")]
    LOCALIZATION_UK = 1,

    /// <summary>
    ///     Belarusian
    /// </summary>
    [JsonStringEnumMemberName("LOCALIZATION_BE")]
    LOCALIZATION_BE = 2,

    /// <summary>
    ///     Kazakh
    /// </summary>
    [JsonStringEnumMemberName("LOCALIZATION_KK")]
    LOCALIZATION_KK = 3,

    /// <summary>
    ///     Turkish
    /// </summary>
    [JsonStringEnumMemberName("LOCALIZATION_TR")]
    LOCALIZATION_TR = 4,

    /// <summary>
    ///     English
    /// </summary>
    [JsonStringEnumMemberName("LOCALIZATION_EN")]
    LOCALIZATION_EN = 5
}
