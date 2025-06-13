using System;

namespace AlphaAi.Services.CommandDispatcher.Implementation;

public class DefaultCommandDispatcherOptions
{
    public DefaultCommandDispatcherOptions(string botName)
    {
        ArgumentNullException.ThrowIfNull(botName);
        BotName = botName;
    }

    public string BotName { get; }
}
