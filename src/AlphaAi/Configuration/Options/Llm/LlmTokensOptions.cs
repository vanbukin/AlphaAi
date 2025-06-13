using System.ComponentModel.DataAnnotations;

namespace AlphaAi.Configuration.Options.Llm;

public class LlmTokensOptions
{
    [Range(1, int.MaxValue, MinimumIsExclusive = false, MaximumIsExclusive = true)]
    public int SummarizeAfter { get; set; }

    [Range(1, int.MaxValue, MinimumIsExclusive = false, MaximumIsExclusive = true)]
    public int RecreateAfter { get; set; }
}
