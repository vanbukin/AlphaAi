using System;
using AlphaAi.Services.CommandDispatcher;
using AlphaAi.Services.CommandDispatcher.Models;
using AlphaAi.Services.CommandDispatcher.Models.Abstractions;

namespace AlphaAi.Commands.ChatWithLlm;

public class ChatWithLlmCommand : AbstractCommand<ChatWithLlmCommand>
{
    public ChatWithLlmCommand(
        TelegramMessage telegramMessage,
        bool isReplyToPreviousLlmAnswer)
    {
        ArgumentNullException.ThrowIfNull(telegramMessage);
        TelegramMessage = telegramMessage;
        IsReplyToPreviousLlmAnswer = isReplyToPreviousLlmAnswer;
    }

    public override TelegramMessage TelegramMessage { get; }
    public bool IsReplyToPreviousLlmAnswer { get; }


    public override AbstractCommandHandler<ChatWithLlmCommand> Accept(ICommandVisitor visitor)
    {
        ArgumentNullException.ThrowIfNull(visitor);
        return visitor.Visit(this);
    }
}
