using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlphaAi.McpServer.Components.YandexSearch.Api.v2.Responses;

public class YandexSearchResponse
{
    public YandexSearchResponse(string rawData)
    {
        ArgumentNullException.ThrowIfNull(rawData);
        RawData = rawData;
    }

    [JsonPropertyName("rawData")]
    [Required]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public string RawData { get; }
}
