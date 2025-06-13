using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.AI;

namespace AlphaAi.Services.Llm.Chat.Models;

public class LlmContext
{
    private readonly List<ChatMessage> _messages = [];
    private bool _isClean;

    public LlmContext(string systemPrompt)
    {
        var systemPromptMessage = new ChatMessage(ChatRole.System, systemPrompt);
        _messages.Add(systemPromptMessage);
        _isClean = true;
    }

    [SuppressMessage("Design", "CA1024:Use properties where appropriate")]
    public IEnumerable<ChatMessage> GetMessages()
    {
        return _messages;
    }

    public void Add(ChatMessage message)
    {
        _isClean = false;
        _messages.Add(message);
    }

    public bool IsClean()
    {
        return _isClean;
    }
}
