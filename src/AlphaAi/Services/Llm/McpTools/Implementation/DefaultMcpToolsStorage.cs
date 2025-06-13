using System.Collections.Generic;
using ModelContextProtocol.Client;

namespace AlphaAi.Services.Llm.McpTools.Implementation;

public class DefaultMcpToolsStorage : IMcpToolsStorage
{
    private IList<McpClientTool> _currentTools = new List<McpClientTool>();

    public IList<McpClientTool> GetTools()
    {
        var toolsCopy = _currentTools;
        return toolsCopy;
    }

    public void SetTools(IList<McpClientTool> tools)
    {
        _currentTools = tools;
    }
}
