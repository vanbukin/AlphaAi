using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AlphaAi.Configuration.Options;

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
[SuppressMessage("ReSharper", "PreferConcreteValueOverDefault")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class ApplicationOptions
{
    [Required]
    [MaxLength(200)]
    public string TelegramBotToken { get; set; } = default!;

    [Required]
    [MinLength(1)]
    public long[] AllowedChatIds { get; set; } = default!;
}
