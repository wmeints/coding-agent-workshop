# Lab: Integrating MCP servers in the agent loop - Solution

This directory contains the solution code for the lab on integrating MCP server tools
into your agent's core loop.

## Solution Overview

This solution demonstrates:

- Complete tool discovery implementation
- Dynamic Semantic Kernel plugin generation
- Proper parameter mapping from MCP schemas
- Robust error handling and timeout management
- End-to-end integration with the agent loop

## Key Components

- `McpToolDiscovery.cs` - Discovers and catalogs tools from MCP servers
- `McpToolsPlugin.cs` - Dynamic plugin that exposes MCP tools to Semantic Kernel
- Updated agent loop to register and use MCP tools
- Comprehensive error handling for tool execution failures

## Testing the Solution

1. Configure one or more MCP servers in `.agent/mcp.json`
2. Run the agent and observe tool discovery in the logs
3. Use prompts that trigger MCP tools
4. Verify that tools execute correctly and results are returned to the LLM

## Advanced Features

The solution includes:
- Tool name prefixing to avoid conflicts between servers
- Graceful degradation when servers fail
- Clear error messages for debugging
- Logging of tool executions

Refer to the [lab guide](../../../../src/content/docs/modules/mcp-servers/integrating-mcp-servers.mdx)
for detailed explanations of the implementation.
