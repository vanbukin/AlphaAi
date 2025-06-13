using System.Text.Json.Serialization;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests.Enums.GroupSpec;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Requests;

/// <summary>
///     Grouping settings that are used to group documents from a single domain into a container.
/// </summary>
public class YandexGroupSpec
{
    /// <summary>
    ///     Grouping settings that are used to group documents from a single domain into a container.
    /// </summary>
    /// <param name="groupMode">Grouping method.</param>
    /// <param name="groupsOnPage">Maximum number of groups that can be returned per page with search results.</param>
    /// <param name="docsInGroup">Maximum number of documents that can be returned per group.</param>
    public YandexGroupSpec(GroupMode? groupMode, long? groupsOnPage, long? docsInGroup)
    {
        GroupMode = groupMode;
        GroupsOnPage = groupsOnPage;
        DocsInGroup = docsInGroup;
    }

    /// <summary>
    ///     Grouping method.
    /// </summary>
    [JsonPropertyName("groupMode")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public GroupMode? GroupMode { get; }

    /// <summary>
    ///     Maximum number of groups that can be returned per page with search results.
    /// </summary>
    [JsonPropertyName("groupsOnPage")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? GroupsOnPage { get; }

    /// <summary>
    ///     Maximum number of documents that can be returned per group.
    /// </summary>
    [JsonPropertyName("docsInGroup")]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? DocsInGroup { get; }
}
