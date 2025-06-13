using System;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.Services.CommandDispatcher.Models.Abstractions;
using AlphaAi.Services.Llm.Chat;
using Telegram.Bot;

namespace AlphaAi.Commands.ResetLlmContext;

public class ResetLlmContextCommandHandler : AbstractCommandHandler<ResetLlmContextCommand>
{
    private readonly TelegramBotClient _bot;
    private readonly ILlmChatService _llmChat;

    public ResetLlmContextCommandHandler(ILlmChatService llmChat, TelegramBotClient bot)
    {
        ArgumentNullException.ThrowIfNull(llmChat);
        ArgumentNullException.ThrowIfNull(bot);
        _llmChat = llmChat;
        _bot = bot;
    }

    public override async Task HandleAsync(ResetLlmContextCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        cancellationToken.ThrowIfCancellationRequested();
        await _llmChat.ResetContextAsync(cancellationToken);
        var (message, _) = command.TelegramMessage;
        await _bot.SendMessage(
            message.Chat,
            "Контекст очищен",
            cancellationToken: cancellationToken);
    }
}
