using System;
using System.Diagnostics.CodeAnalysis;
using AlphaAi.McpServer.Configuration.Options;
using AlphaAi.McpServer.Configuration.TypedConfigurations.YandexSearch;

namespace AlphaAi.McpServer.Configuration.TypedConfigurations;

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
public class ApplicationConfiguration
{
    public ApplicationConfiguration(YandexSearchConfiguration yandexSearch)
    {
        ArgumentNullException.ThrowIfNull(yandexSearch);
        YandexSearch = yandexSearch;
    }

    public YandexSearchConfiguration YandexSearch { get; }

    public static ApplicationConfiguration Convert(ApplicationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var yandexSearch = YandexSearchConfiguration.Convert(options.YandexSearch);
        return new(yandexSearch);
    }
}
