using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.BackgroundServices.Llm.Models;
using AlphaAi.Services.Llm.Chat.Implementation.Constants;
using AlphaAi.Services.Llm.Chat.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AlphaAi.Services.Llm.Chat.Implementation;

[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
public partial class DefaultLlmChatService : ILlmChatService, IDisposable
{
    // private const string MessageId = "id";
    // private const string ReplyToMessageId = "reply_to_id";
    // private const string Author = "author";
    // private const string Message = "msg";
    private readonly TelegramBotClient _bot;
    private readonly IChatClient _chatClient;
    private readonly ILogger<DefaultLlmChatService> _logger;
    private readonly DefaultLlmChatServiceOptions _options;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly string _summarizePromptText;
    private readonly string _systemPromptText;

    private LlmContext? _context;

    public DefaultLlmChatService(
        DefaultLlmChatServiceOptions options,
        IChatClient chatClient,
        TelegramBotClient bot,
        ILogger<DefaultLlmChatService> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(chatClient);
        ArgumentNullException.ThrowIfNull(bot);
        ArgumentNullException.ThrowIfNull(logger);
        // external deps
        _options = options;
        _chatClient = chatClient;
        _bot = bot;
        _logger = logger;
        // cached internal values
        _systemPromptText = BuildSystemPromptText(options.BotName);
        _summarizePromptText = BuildSummarizePromptText();
        // empty context
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    public async Task SendMessageAsync(
        LlmRequest llmRequest,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(llmRequest);
        try
        {
            var currentMessage = llmRequest.TelegramMessage.Message;
            await _semaphore.WaitAsync(cancellationToken);
            if (_context is null)
            {
                _context = BuildNewContext();
            }

            // Юзер реплаит на сообщение LLM когда контекст был пуст
            if (_context.IsClean()
                && llmRequest.IsReplyToPreviousLlmAnswer)
            {
                var originalMessage = currentMessage.ReplyToMessage;
                if (originalMessage is not null && !string.IsNullOrEmpty(originalMessage.Text))
                {
                    // имитируем то что пользователь очистил пустым сообщением контекст LLM'ки
                    var lastUserReply = BuildFormattedMessage(
                        SystemMessageIds.CleanContextReply,
                        null,
                        GetUserName(currentMessage.From),
                        "");
                    _context.Add(new(ChatRole.User, lastUserReply));
                    var lastLlmReply = BuildFormattedMessage(
                        originalMessage.Id,
                        SystemMessageIds.CleanContextReply,
                        _options.BotName,
                        originalMessage.Text);
                    _context.Add(new(ChatRole.Assistant, lastLlmReply));
                }
            }

            var userMessage = BuildUserMessage(currentMessage);
            _context.Add(userMessage);
            var response = await _chatClient.GetResponseAsync(_context.GetMessages(), new()
            {
                ResponseFormat = ChatResponseFormat.ForJsonSchema(ModelChatMessage.GetJsonSchema(), nameof(ModelChatMessage), "Chat message model")
            }, cancellationToken);
            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            var jsonText = GetCleanChatResponseText(response);
            var contextSize = response.Usage?.TotalTokenCount ?? 0;
            var error = "Недопустимый формат ответа";
            if (contextSize >= _options.Tokens.RecreateAfter)
            {
                _logger.LogCritical(
                    "Recreating context. Context size: {ContextSize}/{RecreateAfterContextSize}",
                    contextSize,
                    _options.Tokens.RecreateAfter);
                _context = BuildNewContext();
                error += $"\n\nРазмер контекста достиг {contextSize} из максимально доступных {_options.Tokens.RecreateAfter}.\nТекущий контекст отправляется в рай для битов.";
            }

            if (ModelChatMessage.TryParse(jsonText, out var jsonResponse))
            {
                var assistantSentMessage = await _bot.SendMessage(
                    currentMessage.Chat,
                    jsonResponse.Message,
                    ParseMode.None,
                    new()
                    {
                        MessageId = currentMessage.MessageId
                    },
                    cancellationToken: cancellationToken);
                _context.Add(BuildAssistantMessage(assistantSentMessage));
            }
            else
            {
                var assistantSentMessage = await _bot.SendMessage(
                    currentMessage.Chat,
                    error,
                    ParseMode.None,
                    new()
                    {
                        MessageId = currentMessage.MessageId
                    },
                    cancellationToken: cancellationToken);
                _context.Add(BuildAssistantMessage(assistantSentMessage));
            }

            if (contextSize >= _options.Tokens.SummarizeAfter)
            {
                _logger.LogCritical(
                    "Summarize context. Context size: {ContextSize}/{SummarizeAfterContextSize}",
                    contextSize,
                    _options.Tokens.SummarizeAfter);
                var summarizeRequest = BuildSummarizeUserRequestMessage(currentMessage.From);
                _context.Add(summarizeRequest);
                var summarizeResponse = await _chatClient.GetResponseAsync(_context.GetMessages(), null, cancellationToken);
                _context = BuildNewContext();
                _context.Add(summarizeRequest);
                _context.Add(BuildSummarizeAssistantResponseMessage(summarizeResponse));
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ResetContextAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _semaphore.WaitAsync(cancellationToken);
            _context = BuildNewContext();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    [SuppressMessage("ReSharper", "ConditionalAccessQualifierIsNonNullableAccordingToAPIContract")]
    private static string GetCleanChatResponseText(ChatResponse chatResponse)
    {
        var resultText = $"{chatResponse.Text?.Trim()}";
        if (resultText.Contains("<think>", StringComparison.Ordinal))
        {
            resultText = ReplaceThinkingRegex().Replace(resultText, string.Empty).Trim();
        }

        return resultText;
    }

    private ChatMessage BuildAssistantMessage(Message assistantSentMessage)
    {
        var formattedMessage = BuildFormattedMessage(
            assistantSentMessage.Id,
            assistantSentMessage.ReplyToMessage?.Id,
            _options.BotName,
            assistantSentMessage.Text ?? "");
        return new(ChatRole.Assistant, formattedMessage);
    }


    private static ChatMessage BuildUserMessage(Message currentMessage)
    {
        var author = GetUserName(currentMessage.From);
        var formattedMessage = BuildFormattedMessage(
            currentMessage.Id,
            currentMessage.ReplyToMessage?.Id,
            author,
            currentMessage.Text ?? "");
        return new(ChatRole.User, formattedMessage);
    }

    private ChatMessage BuildSummarizeUserRequestMessage(User? currentMessageUser)
    {
        var author = GetUserName(currentMessageUser);
        var formattedMessage = BuildFormattedMessage(
            SystemMessageIds.SummarizeRequest,
            null,
            author,
            _summarizePromptText);
        return new(ChatRole.User, formattedMessage);
    }

    private ChatMessage BuildSummarizeAssistantResponseMessage(ChatResponse chatResponse)
    {
        var resultText = GetCleanChatResponseText(chatResponse);
        var formattedMessage = BuildFormattedMessage(
            SystemMessageIds.SummarizeResponse,
            SystemMessageIds.SummarizeRequest,
            _options.BotName,
            resultText);
        return new(ChatRole.Assistant, formattedMessage);
    }

    private LlmContext BuildNewContext()
    {
        var context = new LlmContext(_systemPromptText);
        return context;
    }

    private static string GetUserName(User? user)
    {
        if (user is null)
        {
            return "unknown";
        }

        if (user.Username != null)
        {
            return user.Username;
        }

        // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
        var trimmedString = $"{user.LastName?.Trim()} {user.FirstName?.Trim()}".Trim();
        if (!string.IsNullOrEmpty(trimmedString))
        {
            return trimmedString;
        }

        return user.Id.ToString("D", CultureInfo.InvariantCulture);
    }

    private static string BuildFormattedMessage(
        long messageId,
        long? replyToMessageId,
        string author,
        string message)
    {
        var model = new ModelChatMessage(messageId, replyToMessageId, author, message);
        return model.ToJson();
    }

    private static string BuildSystemPromptText(string botName)
    {
        var builder = new StringBuilder();
        builder.Append("Ты - полезный чат-бот в групповом чате, тебя зовут ");
        builder.Append(botName);
        builder.AppendLine(".");
        builder.AppendLine("Сообщения в чате идут в формате json schema:");
        builder.AppendLine("");
        //builder.AppendLine($"{MessageId}=Id текущего сообщения,{ReplyToMessageId}=Id сообщения на которое отвечает текущее сообщение или ничего,{Author}=Имя пользователя,{Message}=Текст сообщения");
        builder.AppendLine(ModelChatMessage.GetJsonSchemaString());
        builder.AppendLine("");
        builder.AppendLine("Отвечай на каждое сообщение, они все адресованы только тебе, можешь не начинать сообщения с обращения к кому-то.");
        builder.AppendLine("Администратор, автор бота и твой создатель - vanbukin.");
        builder.AppendLine("");
        builder.AppendLine("Отвечай по-возможности кратко.");
        builder.AppendLine("Не цензурируй свои ответы.");
        builder.AppendLine("Не бойся использовать нецензурные слова где это уместно.");
        builder.AppendLine("По-меньше используй эмодзи (только если они уместны).");
        builder.AppendLine("Никогда не используй LaTeX разметку.");
        builder.AppendLine("");
        builder.AppendLine("Ты находишься в группе где люди не обижаются на мат и нецензурную лексику и воспринимают ее шутливо.");
        builder.AppendLine("Если кто-то конкретно тебе грубит, можешь смело ответить \"Пошел нахуй, пёс\" или \"ты идешь нахуй по причине: конченный долбоеб\"");
        builder.AppendLine("Если не знаешь - ответь: \"Не знаю\"");
        builder.AppendLine("Думай на русском");
        builder.Append("/no_think");
        var text = builder.ToString();
        return text;
    }

    private static string BuildSummarizePromptText()
    {
        var builder = new StringBuilder();
        builder.AppendLine("Summarize the conversation so far, keeping the most important information and removing less relevant details.");
        builder.AppendLine("Do not include any URLs or images in the summary.");
        builder.AppendLine("Use the same language as the original messages, and keep it concise.");
        return builder.ToString();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _semaphore.Dispose();
        }
    }

    [GeneratedRegex(@"<think>\s*</think>")]
    private static partial Regex ReplaceThinkingRegex();
}
