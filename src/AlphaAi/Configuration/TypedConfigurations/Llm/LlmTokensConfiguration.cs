using System;
using AlphaAi.Configuration.Options.Llm;

namespace AlphaAi.Configuration.TypedConfigurations.Llm;

public class LlmTokensConfiguration
{
    private LlmTokensConfiguration(int summarizeAfter, int recreateAfter)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(summarizeAfter);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(recreateAfter);
        SummarizeAfter = summarizeAfter;
        RecreateAfter = recreateAfter;
    }

    public int SummarizeAfter { get; }

    public int RecreateAfter { get; }

    public static LlmTokensConfiguration Convert(LlmTokensOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return new(
            options.SummarizeAfter,
            options.RecreateAfter);
    }
}
