using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Protocol;
using ModelContextProtocol.Server;
using NodaTime;

namespace AlphaAi.McpServer.Mcp.Tools;

[McpServerToolType]
public class UnixTimestampTool
{
    [McpServerTool(Name = "UnixTimestamp")]
    [Description("Returns current unix timestamp in seconds")]
    public static string UnixTimestamp(ILogger<UnixTimestampTool> logger)
    {
        var result = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString("D");
        logger.LogInformation("Returning: {result}", result);
        return result;
    }

    [McpServerTool(Name = "UnixTimestampToDateInTimeZone")]
    [Description("Converts provided unix timestamp into date time in specified timezone using NodaTime")]
    public static Content UnixTimestampToDateInTimeZone(
        [Description("UnixTimestamp in seconds")]
        long unixTimestamp,
        [Description("Specified TimeZone")] string timeZone,
        ILogger<UnixTimestampTool> logger)
    {
        logger.LogInformation($"{nameof(unixTimestamp)}={unixTimestamp}, {nameof(timeZone)}={timeZone}");
        var utcDateTime = Instant.FromUnixTimeSeconds(unixTimestamp);
        var parsedTimeZone = DateTimeZoneProviders.Bcl.GetZoneOrNull(timeZone);
        if (parsedTimeZone is null)
        {
            parsedTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZone);
        }

        if (parsedTimeZone is null)
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
                Text = "Error: Operation failed. Provided time zone is invalid. Only .NET BCL and TZDB timezones supported."
            };
        }

        var result = utcDateTime.InZone(parsedTimeZone).ToString();
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
            Text = $"Operation completed successfully: {result}"
        };
    }
}
