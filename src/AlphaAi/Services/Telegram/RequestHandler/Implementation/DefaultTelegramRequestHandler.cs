using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.Services.CommandDispatcher;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AlphaAi.Services.Telegram.RequestHandler.Implementation;

[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
[SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
public class DefaultTelegramRequestHandler : ITelegramRequestHandler
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly ILogger<DefaultTelegramRequestHandler> _logger;
    private readonly DefaultTelegramRequestHandlerOptions _options;

    public DefaultTelegramRequestHandler(
        DefaultTelegramRequestHandlerOptions options,
        ICommandDispatcher commandDispatcher,
        IHostApplicationLifetime applicationLifetime,
        ILogger<DefaultTelegramRequestHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(commandDispatcher);
        ArgumentNullException.ThrowIfNull(applicationLifetime);
        ArgumentNullException.ThrowIfNull(logger);
        _options = options;
        _commandDispatcher = commandDispatcher;
        _applicationLifetime = applicationLifetime;
        _logger = logger;
    }

    public async Task OnMessageAsync(Message message, UpdateType type)
    {
        ArgumentNullException.ThrowIfNull(message);
        await OnMessageInternalAsync(message, type, _applicationLifetime.ApplicationStopping);
    }

    public async Task OnErrorAsync(Exception exception, HandleErrorSource source)
    {
        await OnErrorInternalAsync(exception, source, _applicationLifetime.ApplicationStopping);
    }

    private async Task OnMessageInternalAsync(Message message, UpdateType type, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (message.Date < _options.SkipMessagesOlderThan)
            {
                return;
            }

            if (!_options.AllowedChatIds.Contains(message.Chat.Id))
            {
                return;
            }

            await _commandDispatcher.HandleMessageAsync(message, type, cancellationToken);
        }
        catch (Exception ex)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            _logger.LogError(ex, $"{nameof(OnMessageInternalAsync)} error handling. Unknown exception");
        }
    }

    [SuppressMessage("Usage", "CA2254:Template should be a static expression")]
    private async Task OnErrorInternalAsync(Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            await Task.Yield();
            _logger.LogError(exception, $"{nameof(OnErrorInternalAsync)} error handling. Got exception: {source:G}");
        }
        catch (Exception ex)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            _logger.LogError(ex, $"{nameof(OnErrorInternalAsync)} error handling. Unknown exception");
        }
    }
}
