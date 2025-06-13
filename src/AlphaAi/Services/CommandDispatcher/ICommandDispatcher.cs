using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AlphaAi.Services.CommandDispatcher;

public interface ICommandDispatcher
{
    Task HandleMessageAsync(
        Message message,
        UpdateType type,
        CancellationToken cancellationToken);
}
