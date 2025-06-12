using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AlphaAi.Configuration.Options;

namespace AlphaAi.Configuration.TypedConfigurations;

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
public class ApplicationConfiguration
{
    private ApplicationConfiguration(string telegramBotToken, IReadOnlySet<long> allowedChatIds)
    {
        if (string.IsNullOrEmpty(telegramBotToken))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(telegramBotToken));
        }

        ArgumentNullException.ThrowIfNull(allowedChatIds);
        if ((allowedChatIds.Count > 0) is not true)
        {
            throw new ArgumentException("Value sshould contain at least 1 element", nameof(allowedChatIds));
        }

        TelegramBotToken = telegramBotToken;
        AllowedChatIds = allowedChatIds;
    }

    public string TelegramBotToken { get; }

    public IReadOnlySet<long> AllowedChatIds { get; }

    public static ApplicationConfiguration Convert(ApplicationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var uniqueChatIds = options.AllowedChatIds.ToHashSet();
        return new(
            options.TelegramBotToken,
            uniqueChatIds);
    }
}
