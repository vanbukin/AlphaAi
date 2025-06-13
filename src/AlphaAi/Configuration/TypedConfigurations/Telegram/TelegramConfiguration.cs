using System.Collections.Generic;
using System.Linq;
using AlphaAi.Configuration.Options.Telegram;
using ArgumentException = System.ArgumentException;
using ArgumentNullException = System.ArgumentNullException;

namespace AlphaAi.Configuration.TypedConfigurations.Telegram;

public class TelegramConfiguration
{
    private TelegramConfiguration(
        string botToken,
        IReadOnlySet<long> allowedChatIds)
    {
        ArgumentNullException.ThrowIfNull(botToken);
        if (string.IsNullOrEmpty(botToken))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(botToken));
        }

        ArgumentNullException.ThrowIfNull(allowedChatIds);
        if ((allowedChatIds.Count > 0) is not true)
        {
            throw new ArgumentException("Value should contain at least 1 element", nameof(allowedChatIds));
        }

        BotToken = botToken;
        AllowedChatIds = allowedChatIds;
    }

    public string BotToken { get; }

    public IReadOnlySet<long> AllowedChatIds { get; }

    public static TelegramConfiguration Convert(TelegramOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return new(
            options.BotToken,
            options.AllowedChatIds.ToHashSet());
    }
}
