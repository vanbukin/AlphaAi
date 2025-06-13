using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaAi.McpServer.Services.WebPageDownload.Implementation;

public class DefaultWebPageDownloadService : IWebPageDownloadService
{
    private readonly HttpClient _httpClient;

    public DefaultWebPageDownloadService(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    [SuppressMessage("ReSharper", "ConvertToUsingDeclaration")]
    public async Task<string> DownloadWebPageAsync(string url, CancellationToken cancellationToken)
    {
        using (var response = await _httpClient.GetAsync(
                   url,
                   HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                return content;
            }

            response.EnsureSuccessStatusCode();
            throw new InvalidOperationException("Cannot download web page.");
        }
    }
}
