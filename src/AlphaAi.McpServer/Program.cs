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
            options.UseUtcTimestamp = false;
            options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
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
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Google Chrome\";v=\"137\", \"Chromium\";v=\"137\", \"Not/A)Brand\";v=\"24\"");
                    httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
                    httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
                    httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
                    httpClient.DefaultRequestHeaders.Add("DNT", "1");
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36");
                    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
                    httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "cross-site");
                    httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
                    httpClient.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
                    httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
                    httpClient.DefaultRequestHeaders.Add("Referer", "https://www.google.com/");
                    httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br, zstd");
                    httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru,en-US;q=0.9,en;q=0.8");
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
