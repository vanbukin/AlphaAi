using System.Collections.Generic;
using ModelContextProtocol.Client;

namespace AlphaAi.Services.Llm.McpTools;

public interface IMcpToolsStorage
{
    IList<McpClientTool> GetTools();

    void SetTools(IList<McpClientTool> tools);
}
