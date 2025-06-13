using System;

namespace AlphaAi.Services.Llm.Chat.Implementation;

public class DefaultLlmChatServiceOptions
{
    public DefaultLlmChatServiceOptions(string botName, DefaultLlmChatServiceTokensOptions tokens)
    {
        ArgumentNullException.ThrowIfNull(botName);
        ArgumentNullException.ThrowIfNull(tokens);
        BotName = botName;
        Tokens = tokens;
    }

    public string BotName { get; }

    public DefaultLlmChatServiceTokensOptions Tokens { get; }
}
