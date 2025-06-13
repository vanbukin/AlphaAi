using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using AlphaAi.McpServer.Services.YandexSearch;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;

namespace AlphaAi.McpServer.Mcp.Tools;

public class YandexSearchTool
{
    [McpServerTool(Name = "Internet search")]
    [Description("Realtime internet search. Returns internet search result as XML.")]
    public static async Task<Content> YandexSearch(
        [Description("Search query")] string searchQuery,
        IYandexSearchService yandexSearchService,
        ILogger<YandexSearchTool> logger,
        CancellationToken cancellationToken)
    {
        var result = await yandexSearchService.SearchAsync(searchQuery, cancellationToken);
        logger.LogInformation($"Returning result for: {searchQuery}");
        if (result is null)
        {
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
                Text = "Error: Operation failed. Can't find anything. Results is empty."
            };
        }

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
            Text = $"Operation completed successfully. Result in XML: {result}"
        };
    }
}
