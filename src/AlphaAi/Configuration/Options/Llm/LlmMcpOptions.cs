using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AlphaAi.Configuration.Options.Llm;

public class LlmMcpOptions
{
    [MemberNotNullWhen(true, nameof(Endpoint))]
    public bool Enabled { get; set; }

    [MaxLength(10000)]
    [Url]
    public string? Endpoint { get; set; }
}
