using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AlphaAi.Configuration.Options.Llm;
using AlphaAi.Configuration.Options.Telegram;

namespace AlphaAi.Configuration.Options;

[SuppressMessage("ReSharper", "PreferConcreteValueOverDefault")]
public class ApplicationOptions
{
    [Required]
    public TelegramOptions Telegram { get; set; } = default!;

    [Required]
    public LlmOptions Llm { get; set; } = default!;
}
