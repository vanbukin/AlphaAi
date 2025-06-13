using System;
using AlphaAi.McpServer.Configuration.Options.YandexSearch;

namespace AlphaAi.McpServer.Configuration.TypedConfigurations.YandexSearch;

public class YandexSearchConfiguration
{
    private YandexSearchConfiguration(string apiKey, string folderId)
    {
        ArgumentNullException.ThrowIfNull(apiKey);
        ArgumentNullException.ThrowIfNull(folderId);
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(apiKey));
        }

        if (string.IsNullOrEmpty(folderId))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(folderId));
        }

        ApiKey = apiKey;
        FolderId = folderId;
    }

    public string ApiKey { get; }
    public string FolderId { get; }

    public static YandexSearchConfiguration Convert(YandexSearchOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return new(options.ApiKey, options.FolderId);
    }
}
