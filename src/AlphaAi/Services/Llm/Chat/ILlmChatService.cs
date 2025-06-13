using System.Threading;
using System.Threading.Tasks;
using AlphaAi.BackgroundServices.Llm.Models;

namespace AlphaAi.Services.Llm.Chat;

public interface ILlmChatService
{
    Task SendMessageAsync(LlmRequest llmRequest, CancellationToken cancellationToken);

    Task ResetContextAsync(CancellationToken cancellationToken);
}
