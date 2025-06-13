using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.Services.CommandDispatcher.Models.Abstractions;
using Telegram.Bot;

namespace AlphaAi.Commands.DisplayHelp;

public class DisplayHelpCommandHandler : AbstractCommandHandler<DisplayHelpCommand>
{
    private readonly TelegramBotClient _bot;
    private readonly string _helpTemplate;

    public DisplayHelpCommandHandler(DisplayHelpCommandHandlerOptions options, TelegramBotClient bot)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(bot);
        _helpTemplate = BuildHelpTemplate(options.BotName);
        _bot = bot;
    }

    public override async Task HandleAsync(DisplayHelpCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        cancellationToken.ThrowIfCancellationRequested();


        var (message, _) = command.TelegramMessage;
        await _bot.SendMessage(
            message.Chat,
            _helpTemplate,
            replyParameters: new()
            {
                MessageId = message.MessageId
            },
            cancellationToken: cancellationToken);
    }

    private static string BuildHelpTemplate(string botName)
    {
        var builder = new StringBuilder();
        builder.AppendLine("..help - display help");
        builder.AppendLine("..reset - reset LLM context");
        var botNameTemplate = $"{botName} - prefix to talk with LLM";
        builder.AppendLine(botNameTemplate);
        return builder.ToString();
    }
}
