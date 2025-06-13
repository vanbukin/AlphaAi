using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums;

/// <summary>
///     Search results format.
/// </summary>
public enum Format
{
    /// <summary>
    ///     XML format (default value)
    /// </summary>
    [JsonStringEnumMemberName("FORMAT_XML")]
    FORMAT_XML = 0,

    /// <summary>
    ///     HTML format
    /// </summary>
    [JsonStringEnumMemberName("HTML format")]
    FORMAT_HTML = 1
}
