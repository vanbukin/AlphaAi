using System;

namespace AlphaAi.Commands.DisplayHelp;

public class DisplayHelpCommandHandlerOptions
{
    public DisplayHelpCommandHandlerOptions(string botName)
    {
        ArgumentNullException.ThrowIfNull(botName);
        if (string.IsNullOrEmpty(botName))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(botName));
        }

        BotName = botName;
    }

    public string BotName { get; }
}
