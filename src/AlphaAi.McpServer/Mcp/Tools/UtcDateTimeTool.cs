using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using NodaTime;

namespace AlphaAi.McpServer.Mcp.Tools;

public class UtcDateTimeTool
{
    [McpServerTool(Name = "CurrentDateTimeUtc")]
    [Description("Returns current DateTime in UTC")]
    public static string CurrentDateTimeUtc(ILogger<UtcDateTimeTool> logger)
    {
        var utcDateTime = Instant.FromDateTimeOffset(DateTimeOffset.UtcNow);
        var result = utcDateTime.ToString();
        logger.LogInformation($"Returning: {result}");
        return result;
    }
}
