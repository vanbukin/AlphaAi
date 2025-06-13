using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using AlphaAi.BackgroundServices.Llm;
using AlphaAi.BackgroundServices.Llm.Models;
using AlphaAi.Commands.ChatWithLlm;
using AlphaAi.Commands.DisplayHelp;
using AlphaAi.Commands.ResetLlmContext;
using AlphaAi.Configuration.Options;
using AlphaAi.Configuration.TypedConfigurations;
using AlphaAi.Extensions.Configuration;
using AlphaAi.Services.CommandDispatcher;
using AlphaAi.Services.CommandDispatcher.Implementation;
using AlphaAi.Services.Llm.Chat;
using AlphaAi.Services.Llm.Chat.Implementation;
using AlphaAi.Services.Llm.ChatClientFactory;
using AlphaAi.Services.Llm.ChatClientFactory.Implementation;
using AlphaAi.Services.Telegram.RequestHandler;
using AlphaAi.Services.Telegram.RequestHandler.Implementation;
using AlphaAi.Services.Telegram.SelfProvider;
using AlphaAi.Services.Telegram.SelfProvider.Implementation;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Telegram.Bot;

namespace AlphaAi;

[SuppressMessage("Maintainability", "CA1515:Consider making public types internal")]
[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
[SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable")]
public class Program
{
    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    [SuppressMessage("Style", "IDE0063:Use simple \'using\' statement")]
    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates")]
    public static async Task<int> Main(string[] args)
    {
        var returnCode = 0;
        Console.OutputEncoding = Encoding.UTF8;

        var builder = Host.CreateApplicationBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetRequiredSection("Logging"));
        builder.Logging.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.UseUtcTimestamp = true;
            options.IncludeScopes = true;
        });
        if (builder.Environment.IsProduction())
        {
            builder.Configuration.AddUserSecrets(typeof(Program).Assembly, true);
        }

        var exceptionLogged = false;
        try
        {
            var dropMessagesOlderThan = DateTimeOffset.UtcNow;

            var config = builder.Configuration
                .GetTypedConfigurationFromOptions<ApplicationOptions, ApplicationConfiguration>(static x =>
                    ApplicationConfiguration.Convert(x));
            // Llm
            builder.Services.AddSingleton(
                new DefaultLlmChatServiceOptions(
                    config.Llm.BotName,
                    new(config.Llm.Tokens.SummarizeAfter,
                        config.Llm.Tokens.RecreateAfter)));
            builder.Services.AddSingleton<ILlmChatService, DefaultLlmChatService>();
            builder.Services.AddSingleton(new DefaultChatClientFactoryOptions(
                config.Llm.Endpoint,
                config.Llm.ApiKey,
                config.Llm.Model));
            builder.Services.AddSingleton<IChatClientFactory, DefaultChatClientFactory>();
            builder.Services.AddSingleton<IChatClient>(resolver =>
            {
                var factory = resolver.GetRequiredService<IChatClientFactory>();
                return factory.CreateChatClient();
            });
            // Dispatching
            builder.Services.AddSingleton(new DefaultCommandDispatcherOptions(config.Llm.BotName));
            builder.Services.AddSingleton<ICommandDispatcher, DefaultCommandDispatcher>();
            builder.Services.AddSingleton<ICommandVisitor, DefaultCommandVisitor>();
            // Command Handlers
            builder.Services.AddSingleton<ResetLlmContextCommandHandler>();
            builder.Services.AddSingleton(new DisplayHelpCommandHandlerOptions(config.Llm.BotName));
            builder.Services.AddSingleton<DisplayHelpCommandHandler>();
            builder.Services.AddSingleton<ChatWithLlmCommandHandler>();
            // Telegram client
            builder.Services.AddSingleton(new TelegramBotClient(config.Telegram.BotToken));
            builder.Services.AddSingleton<ITelegramSelfProvider, DefaultTelegramSelfProvider>();
            builder.Services.AddSingleton(new DefaultTelegramRequestHandlerOptions(dropMessagesOlderThan, config.Telegram.AllowedChatIds));
            builder.Services.AddSingleton<ITelegramRequestHandler, DefaultTelegramRequestHandler>();
            // Channels
            var llmRequestChannel = Channel.CreateBounded<LlmRequest>(new BoundedChannelOptions(20)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = false,
                SingleWriter = false,
                AllowSynchronousContinuations = false
            });
            builder.Services.AddSingleton(llmRequestChannel.Reader);
            builder.Services.AddSingleton<ChannelWriter<LlmRequest>>(resolver =>
            {
                var hostLifetime = resolver.GetRequiredService<IHostApplicationLifetime>();
                hostLifetime.ApplicationStopping.Register(() => llmRequestChannel.Writer.Complete());
                return llmRequestChannel.Writer;
            });
            // Background services
            builder.Services.AddHostedService<LlmRequestsBackgroundService>();
            // Infrastructure
            builder.Services.AddSingleton(TimeProvider.System);
            // Start host
            using (var host = builder.Build())
            {
                var programLogger = host.Services.GetRequiredService<ILogger<Program>>();
                try
                {
                    programLogger.LogInformation("Initializing application");
                    var selfProvider = host.Services.GetRequiredService<ITelegramSelfProvider>();

                    await selfProvider.InitializeAsync(CancellationToken.None);
                    var requestHandler = host.Services.GetRequiredService<ITelegramRequestHandler>();
                    var botClient = host.Services.GetRequiredService<TelegramBotClient>();
                    botClient.OnMessage += requestHandler.OnMessageAsync;
                    botClient.OnError += requestHandler.OnErrorAsync;
                    programLogger.LogInformation("Initialization complete");
                    await host.RunAsync(CancellationToken.None);
                }
                catch (Exception ex)
                {
                    programLogger.LogCritical(ex, "Program terminated unexpectedly");
                    exceptionLogged = true;
                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            if (!exceptionLogged)
            {
                Console.WriteLine(ex);
            }

            returnCode = -1;
        }

        return returnCode;
    }
}
