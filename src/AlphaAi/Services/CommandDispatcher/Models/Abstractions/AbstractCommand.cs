namespace AlphaAi.Services.CommandDispatcher.Models.Abstractions;

public abstract class AbstractCommand<TCommand> where TCommand : AbstractCommand<TCommand>
{
    public abstract TelegramMessage TelegramMessage { get; }
    public abstract AbstractCommandHandler<TCommand> Accept(ICommandVisitor visitor);
}
