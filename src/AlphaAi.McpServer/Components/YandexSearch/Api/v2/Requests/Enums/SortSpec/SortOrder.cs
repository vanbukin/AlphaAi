using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.SortSpec;

/// <summary>
///     Documents sorting order.
/// </summary>
public enum SortOrder
{
    /// <summary>
    ///     Reverse order from oldest to most recent.
    /// </summary>
    [JsonStringEnumMemberName("SORT_ORDER_ASC")]
    SORT_ORDER_ASC = 1,

    /// <summary>
    ///     Direct order from most recent to oldest (default).
    /// </summary>
    [JsonStringEnumMemberName("SORT_ORDER_DESC")]
    SORT_ORDER_DESC = 0
}
