using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace AlphaAi.Services.Telegram.SelfProvider;

public interface ITelegramSelfProvider
{
    Task InitializeAsync(CancellationToken cancellationToken);
    User GetSelf();
}
