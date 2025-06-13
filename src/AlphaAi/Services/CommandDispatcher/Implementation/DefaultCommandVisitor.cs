using System;
using AlphaAi.Commands.ChatWithLlm;
using AlphaAi.Commands.DisplayHelp;
using AlphaAi.Commands.ResetLlmContext;
using AlphaAi.Services.CommandDispatcher.Models.Abstractions;

namespace AlphaAi.Services.CommandDispatcher.Implementation;

public class DefaultCommandVisitor : ICommandVisitor
{
    private readonly ChatWithLlmCommandHandler _chatWithLlm;
    private readonly DisplayHelpCommandHandler _displayHelp;
    private readonly ResetLlmContextCommandHandler _resetLlmContext;


    public DefaultCommandVisitor(
        ResetLlmContextCommandHandler resetLlmContext,
        DisplayHelpCommandHandler displayHelp,
        ChatWithLlmCommandHandler chatWithLlm)
    {
        ArgumentNullException.ThrowIfNull(resetLlmContext);
        ArgumentNullException.ThrowIfNull(displayHelp);
        ArgumentNullException.ThrowIfNull(chatWithLlm);
        _resetLlmContext = resetLlmContext;
        _displayHelp = displayHelp;
        _chatWithLlm = chatWithLlm;
    }

    public AbstractCommandHandler<ResetLlmContextCommand> Visit(ResetLlmContextCommand command)
    {
        return _resetLlmContext;
    }

    public AbstractCommandHandler<DisplayHelpCommand> Visit(DisplayHelpCommand command)
    {
        return _displayHelp;
    }

    public AbstractCommandHandler<ChatWithLlmCommand> Visit(ChatWithLlmCommand command)
    {
        return _chatWithLlm;
    }
}
