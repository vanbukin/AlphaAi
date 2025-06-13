using System;
using System.Diagnostics.CodeAnalysis;
using AlphaAi.Configuration.Options;
using AlphaAi.Configuration.TypedConfigurations.Llm;
using AlphaAi.Configuration.TypedConfigurations.Telegram;

namespace AlphaAi.Configuration.TypedConfigurations;

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
public class ApplicationConfiguration
{
    public ApplicationConfiguration(TelegramConfiguration telegram, LlmConfiguration llm)
    {
        ArgumentNullException.ThrowIfNull(telegram);
        ArgumentNullException.ThrowIfNull(llm);
        Telegram = telegram;
        Llm = llm;
    }

    public TelegramConfiguration Telegram { get; }
    public LlmConfiguration Llm { get; }

    public static ApplicationConfiguration Convert(ApplicationOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var telegram = TelegramConfiguration.Convert(options.Telegram);
        var llm = LlmConfiguration.Convert(options.Llm);
        return new(
            telegram,
            llm);
    }
}
