using System;
using AlphaAi.Services.CommandDispatcher;
using AlphaAi.Services.CommandDispatcher.Models;
using AlphaAi.Services.CommandDispatcher.Models.Abstractions;

namespace AlphaAi.Commands.DisplayHelp;

public class DisplayHelpCommand : AbstractCommand<DisplayHelpCommand>
{
    public DisplayHelpCommand(TelegramMessage telegramMessage)
    {
        ArgumentNullException.ThrowIfNull(telegramMessage);
        TelegramMessage = telegramMessage;
    }

    public override TelegramMessage TelegramMessage { get; }

    public override AbstractCommandHandler<DisplayHelpCommand> Accept(ICommandVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
