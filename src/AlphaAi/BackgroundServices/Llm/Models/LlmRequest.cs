using AlphaAi.Services.CommandDispatcher.Models;

namespace AlphaAi.BackgroundServices.Llm.Models;

public class LlmRequest
{
    public LlmRequest(TelegramMessage telegramMessage, bool isReplyToPreviousLlmAnswer)
    {
        TelegramMessage = telegramMessage;
        IsReplyToPreviousLlmAnswer = isReplyToPreviousLlmAnswer;
    }

    public TelegramMessage TelegramMessage { get; }

    public bool IsReplyToPreviousLlmAnswer { get; }
}
