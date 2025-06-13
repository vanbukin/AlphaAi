using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using AlphaAi.BackgroundServices.Llm.Models;
using AlphaAi.Services.Llm.Chat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AlphaAi.BackgroundServices.Llm;

[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
public class LlmRequestsBackgroundService : BackgroundService
{
    private readonly ILlmChatService _llmChat;
    private readonly ILogger<LlmRequestsBackgroundService> _logger;
    private readonly ChannelReader<LlmRequest> _requests;

    public LlmRequestsBackgroundService(
        ChannelReader<LlmRequest> requests,
        ILlmChatService llmChat,
        ILogger<LlmRequestsBackgroundService> logger)
    {
        ArgumentNullException.ThrowIfNull(requests);
        ArgumentNullException.ThrowIfNull(llmChat);
        ArgumentNullException.ThrowIfNull(logger);
        _requests = requests;
        _llmChat = llmChat;
        _logger = logger;
    }

    [SuppressMessage("Design", "CA1031:Do not catch general exception types")]
    [SuppressMessage("ReSharper", "RedundantWithCancellation")]
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (stoppingToken.IsCancellationRequested)
        {
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    return;
                }

                await foreach (var request in _requests.ReadAllAsync(stoppingToken).WithCancellation(stoppingToken))
                {
                    await HandleRequestAsync(request, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown exception");
            }
        }
    }

    private async Task HandleRequestAsync(LlmRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await _llmChat.SendMessageAsync(request, cancellationToken);
    }
}
