# Lab: Extending the application with MCP server support - Starter Code

This directory contains the starter code for the lab on extending your coding agent
with MCP server support.

## Getting Started

1. Follow the instructions in the [lab guide](../../../../src/content/docs/modules/mcp-servers/extending-with-mcp-servers.mdx)
2. Implement the required components to load and connect to MCP servers
3. Test your implementation with a sample MCP server

## What You'll Build

- Configuration model for MCP servers (`McpServerConfig`, `McpConfiguration`)
- MCP client manager to handle connections to servers
- Integration with the agent's initialization code

## Testing

Create a `.agent/mcp.json` file in your workspace to test the configuration:

```json
{
  "mcpServers": {
    "filesystem": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-filesystem", "/tmp"]
    }
  }
}
```

Run your agent and verify that it successfully connects to the configured server.
