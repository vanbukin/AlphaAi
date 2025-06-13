using System;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OpenAI;

namespace AlphaAi.Services.Llm.ChatClientFactory.Implementation;

public class DefaultChatClientFactory : IChatClientFactory
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly DefaultChatClientFactoryOptions _options;

    public DefaultChatClientFactory(DefaultChatClientFactoryOptions options, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(loggerFactory);
        _options = options;
        _loggerFactory = loggerFactory;
    }

    public IChatClient CreateChatClient()
    {
        var openAiClient = new OpenAIClient(
            new(_options.ApiKey),
            new()
            {
                Endpoint = _options.Endpoint
            });
        var chatClient = openAiClient.GetChatClient(_options.Model);
        var msChatClient = chatClient.AsIChatClient()
            .AsBuilder()
            .UseLogging(_loggerFactory)
            .UseFunctionInvocation()
            .Build();

        return msChatClient;
    }
}
