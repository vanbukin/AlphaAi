using System;
using System.Collections.Generic;

namespace AlphaAi.Services.Telegram.RequestHandler.Implementation;

public class DefaultTelegramRequestHandlerOptions
{
    public DefaultTelegramRequestHandlerOptions(DateTimeOffset skipMessagesOlderThan, IReadOnlySet<long> allowedChatIds)
    {
        ArgumentNullException.ThrowIfNull(allowedChatIds);
        SkipMessagesOlderThan = skipMessagesOlderThan;
        AllowedChatIds = allowedChatIds;
    }

    public DateTimeOffset SkipMessagesOlderThan { get; }
    public IReadOnlySet<long> AllowedChatIds { get; }
}
