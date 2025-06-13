using System;

namespace AlphaAi.McpServer.Services.YandexSearch.Implementation;

public class DefaultYandexSearchServiceOptions
{
    public DefaultYandexSearchServiceOptions(string folderId)
    {
        ArgumentNullException.ThrowIfNull(folderId);
        FolderId = folderId;
    }

    public string FolderId { get; }
}
