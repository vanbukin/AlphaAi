using System.Threading;
using System.Threading.Tasks;

namespace AlphaAi.Services.CommandDispatcher.Models.Abstractions;

public abstract class AbstractCommandHandler<TCommand> where TCommand : AbstractCommand<TCommand>
{
    public abstract Task HandleAsync(TCommand command, CancellationToken cancellationToken);
}
