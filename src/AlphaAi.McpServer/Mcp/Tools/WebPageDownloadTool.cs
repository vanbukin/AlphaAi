using System;
using System.ComponentModel;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.McpServer.Services.WebPageDownload;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace AlphaAi.McpServer.Mcp.Tools;

public class WebPageDownloadTool
{
    private static readonly JsonSerializerOptions SafeCompact = new(JsonSerializerDefaults.General)
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    [McpServerTool(Name = "Internet document download using HTTP GET request")]
    [Description("Realtime internet document download. Returns result as string.")]
    public static async Task<Content> WebPageDownload(
        [Description("Web page address. Url with http:// or https:// scheme.")]
        string webPageAddress,
        IWebPageDownloadService webPageDownloadService,
        ILogger<WebPageDownloadTool> logger,
        CancellationToken cancellationToken)
    {
        logger.LogInformation($"Downloading: {webPageAddress}");
        try
        {
            using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken))
                {
                    var result = await webPageDownloadService.DownloadWebPageAsync(webPageAddress, linkedCts.Token);
                    logger.LogInformation($"Downloaded web page: {result}");
                    return new()
                    {
                        Annotations = new()
                        {
                            Audience =
                            [
                                Role.Assistant
                            ],
                            Priority = 1.0f
                        },
                        Type = "text",
                        Text = result
                    };
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error downloading web page");
            return new()
            {
                Annotations = new()
                {
                    Audience =
                    [
                        Role.Assistant
                    ],
                    Priority = 1.0f
                },
                Type = "text",
                Text = "Error: Operation failed. Can't download web page."
            };
        }
    }
}
