using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AlphaAi.Services.Telegram.SelfProvider.Implementation;

public class DefaultTelegramSelfProvider : ITelegramSelfProvider
{
    private readonly TelegramBotClient _bot;
    private User? _self;

    public DefaultTelegramSelfProvider(TelegramBotClient bot)
    {
        ArgumentNullException.ThrowIfNull(bot);
        _bot = bot;
        _self = null;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _self = await _bot.GetMe(cancellationToken);
    }

    public User GetSelf()
    {
        var selfCopy = _self;
        if (selfCopy is null)
        {
            throw new InvalidOperationException("Self provider has not been initialized");
        }

        return selfCopy;
    }
}
