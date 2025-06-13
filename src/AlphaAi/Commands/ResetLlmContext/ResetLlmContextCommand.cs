using System;
using AlphaAi.Services.CommandDispatcher;
using AlphaAi.Services.CommandDispatcher.Models;
using AlphaAi.Services.CommandDispatcher.Models.Abstractions;

namespace AlphaAi.Commands.ResetLlmContext;

public class ResetLlmContextCommand : AbstractCommand<ResetLlmContextCommand>
{
    public ResetLlmContextCommand(TelegramMessage telegramMessage)
    {
        ArgumentNullException.ThrowIfNull(telegramMessage);
        TelegramMessage = telegramMessage;
    }

    public override TelegramMessage TelegramMessage { get; }

    public override AbstractCommandHandler<ResetLlmContextCommand> Accept(ICommandVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
