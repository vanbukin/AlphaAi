using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.GroupSpec;

/// <summary>
///     Grouping method.
/// </summary>
public enum GroupMode
{
    /// <summary>
    ///     Flat grouping. Each group contains a single document.
    /// </summary>
    [JsonStringEnumMemberName("GROUP_MODE_FLAT")]
    GROUP_MODE_FLAT = 0,

    /// <summary>
    ///     Grouping by domain. Each group contains documents from one domain.
    /// </summary>
    [JsonStringEnumMemberName("GROUP_MODE_DEEP")]
    GROUP_MODE_DEEP = 1
}
