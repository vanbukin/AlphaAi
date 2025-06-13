using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.Infrastructure.Extensions.Configuration;
using AlphaAi.McpServer.Components.YandexSearch.Api.v2;
using AlphaAi.McpServer.Configuration.Options;
using AlphaAi.McpServer.Configuration.TypedConfigurations;
using AlphaAi.McpServer.Mcp.Tools;
using AlphaAi.McpServer.Services.WebPageDownload;
using AlphaAi.McpServer.Services.WebPageDownload.Implementation;
using AlphaAi.McpServer.Services.YandexSearch;
using AlphaAi.McpServer.Services.YandexSearch.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace AlphaAi.McpServer;

[SuppressMessage("ReSharper", "UseAwaitUsing")]
[SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var returnCode = 0;
        Console.OutputEncoding = Encoding.UTF8;
        var builder = WebApplication.CreateBuilder(args);
        builder.Logging.ClearProviders();
        builder.Logging.AddConfiguration(builder.Configuration.GetRequiredSection("Logging"));
        builder.Logging.AddSimpleConsole(options =>
        {
            options.ColorBehavior = LoggerColorBehavior.Enabled;
            options.UseUtcTimestamp = true;
        });
        if (builder.Environment.IsProduction())
        {
            builder.Configuration.AddUserSecrets(typeof(Program).Assembly, true);
        }

        var exceptionLogged = false;
        try
        {
            var config = builder.Configuration
                .GetTypedConfigurationFromOptions<ApplicationOptions, ApplicationConfiguration>(static x =>
                    ApplicationConfiguration.Convert(x));
            builder.Services.AddHttpClient<IYandexSearchApiClient, DefaultYandexSearchApiClient>()
                .AddDefaultLogger()
                .ConfigureHttpClient((_, httpClient) =>
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Api-Key {config.YandexSearch.ApiKey}");
                });
            builder.Services.AddHttpClient<IWebPageDownloadService, DefaultWebPageDownloadService>()
                .ConfigureHttpClient((_, httpClient) =>
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36");
                });
            builder.Services.AddSingleton(new DefaultYandexSearchServiceOptions(config.YandexSearch.FolderId));
            builder.Services.AddScoped<IYandexSearchService, DefaultYandexSearchService>();
            builder.Services.AddMcpServer()
                .WithHttpTransport()
                .WithTools<UnixTimestampTool>()
                .WithTools<UtcDateTimeTool>()
                .WithTools<YandexSearchTool>()
                .WithTools<WebPageDownloadTool>();
            // Start host
            using (var app = builder.Build())
            {
                var programLogger = app.Services.GetRequiredService<ILogger<Program>>();
                try
                {
                    app.MapMcp();
                    programLogger.LogInformation("MCP server initialized");
                    await app.RunAsync(CancellationToken.None);
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
