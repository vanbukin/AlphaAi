using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AlphaAi.Configuration.Options.Llm;

[SuppressMessage("ReSharper", "PreferConcreteValueOverDefault")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class LlmOptions
{
    [Required]
    [MaxLength(10000)]
    [Url]
    public string Endpoint { get; set; } = default!;

    [Required]
    [MaxLength(10000)]
    public string ApiKey { get; set; } = default!;

    [Required]
    [MaxLength(10000)]
    public string Model { get; set; } = default!;

    [Required]
    [MaxLength(200)]
    public string BotName { get; set; } = default!;

    [Required]
    public LlmTokensOptions Tokens { get; set; } = default!;

    [Required]
    public LlmMcpOptions Mcp { get; set; } = default!;
}
