using Microsoft.Extensions.AI;

namespace AlphaAi.Services.Llm.ChatClientFactory;

public interface IChatClientFactory
{
    public IChatClient CreateChatClient();
}
