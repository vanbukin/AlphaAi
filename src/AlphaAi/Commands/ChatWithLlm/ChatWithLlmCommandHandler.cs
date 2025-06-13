using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using AlphaAi.BackgroundServices.Llm.Models;
using AlphaAi.Services.CommandDispatcher.Models.Abstractions;

namespace AlphaAi.Commands.ChatWithLlm;

public class ChatWithLlmCommandHandler : AbstractCommandHandler<ChatWithLlmCommand>
{
    private readonly ChannelWriter<LlmRequest> _requests;

    public ChatWithLlmCommandHandler(ChannelWriter<LlmRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);
        _requests = requests;
    }

    public override async Task HandleAsync(
        ChatWithLlmCommand command,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);
        cancellationToken.ThrowIfCancellationRequested();
        var llmRequest = new LlmRequest(command.TelegramMessage, command.IsReplyToPreviousLlmAnswer);
        await _requests.WriteAsync(llmRequest, cancellationToken);
    }
}
