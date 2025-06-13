using System;

namespace AlphaAi.Services.Llm.Chat.Implementation;

public class DefaultLlmChatServiceTokensOptions
{
    public DefaultLlmChatServiceTokensOptions(int summarizeAfter, int recreateAfter)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(summarizeAfter);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(recreateAfter);
        SummarizeAfter = summarizeAfter;
        RecreateAfter = recreateAfter;
    }

    public int SummarizeAfter { get; }

    public int RecreateAfter { get; }
}
