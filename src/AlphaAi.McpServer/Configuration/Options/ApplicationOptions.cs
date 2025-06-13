using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AlphaAi.McpServer.Configuration.Options.YandexSearch;

namespace AlphaAi.McpServer.Configuration.Options;

[SuppressMessage("ReSharper", "PreferConcreteValueOverDefault")]
public class ApplicationOptions
{
    [Required]
    public YandexSearchOptions YandexSearch { get; set; } = default!;
}
