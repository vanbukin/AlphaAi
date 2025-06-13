using System.Threading;
using System.Threading.Tasks;

namespace AlphaAi.McpServer.Services.WebPageDownload;

public interface IWebPageDownloadService
{
    Task<string> DownloadWebPageAsync(string url, CancellationToken cancellationToken);
}
