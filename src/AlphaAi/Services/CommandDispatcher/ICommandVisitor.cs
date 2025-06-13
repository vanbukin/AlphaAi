using AlphaAi.Commands.ChatWithLlm;
using AlphaAi.Commands.DisplayHelp;
using AlphaAi.Commands.ResetLlmContext;
using AlphaAi.Services.CommandDispatcher.Models.Abstractions;

namespace AlphaAi.Services.CommandDispatcher;

public interface ICommandVisitor
{
    AbstractCommandHandler<ResetLlmContextCommand> Visit(ResetLlmContextCommand command);
    AbstractCommandHandler<DisplayHelpCommand> Visit(DisplayHelpCommand command);
    AbstractCommandHandler<ChatWithLlmCommand> Visit(ChatWithLlmCommand command);
}
