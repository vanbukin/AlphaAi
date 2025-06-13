using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AlphaAi.McpServer.Configuration.Options.YandexSearch;

[SuppressMessage("ReSharper", "PreferConcreteValueOverDefault")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class YandexSearchOptions
{
    [Required]
    [MaxLength(10000)]
    public string ApiKey { get; set; } = default!;

    [Required]
    [MaxLength(10000)]
    public string FolderId { get; set; } = default!;
}
