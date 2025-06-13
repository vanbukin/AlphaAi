using System;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.Commands.ChatWithLlm;
using AlphaAi.Commands.DisplayHelp;
using AlphaAi.Commands.ResetLlmContext;
using AlphaAi.Services.Telegram.SelfProvider;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AlphaAi.Services.CommandDispatcher.Implementation;

public class DefaultCommandDispatcher : ICommandDispatcher
{
    private readonly DefaultCommandDispatcherOptions _options;
    private readonly ITelegramSelfProvider _selfProvider;
    private readonly ICommandVisitor _visitor;

    public DefaultCommandDispatcher(
        DefaultCommandDispatcherOptions options,
        ICommandVisitor visitor,
        ITelegramSelfProvider selfProvider)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(visitor);
        ArgumentNullException.ThrowIfNull(selfProvider);
        _options = options;
        _visitor = visitor;
        _selfProvider = selfProvider;
    }


    public async Task HandleMessageAsync(Message message, UpdateType type, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (message is null || string.IsNullOrEmpty(message.Text))
        {
            return;
        }

        switch (message.Text)
        {
            case "..help":
                {
                    var command = new DisplayHelpCommand(new(message, type));
                    var handler = _visitor.Visit(command);
                    await handler.HandleAsync(command, cancellationToken);
                    return;
                }
            case "..reset":
                {
                    var command = new ResetLlmContextCommand(new(message, type));
                    var handler = _visitor.Visit(command);
                    await handler.HandleAsync(command, cancellationToken);
                    return;
                }
            default:
                {
                    if (message.Text.StartsWith(_options.BotName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var command = new ChatWithLlmCommand(new(message, type), false);
                        var handler = _visitor.Visit(command);
                        await handler.HandleAsync(command, cancellationToken);
                    }

                    if (message.ReplyToMessage?.From is not null)
                    {
                        var self = _selfProvider.GetSelf();
                        if (message.ReplyToMessage.From.Id == self.Id)
                        {
                            var command = new ChatWithLlmCommand(new(message, type), true);
                            var handler = _visitor.Visit(command);
                            await handler.HandleAsync(command, cancellationToken);
                        }
                    }

                    return;
                }
        }
    }
}
