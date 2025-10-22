# Lab: Integrating MCP servers in the agent loop - Starter Code

This directory contains the starter code for the lab on integrating MCP server tools
into your agent's core loop.

## Prerequisites

Complete the previous lab on [extending the application with MCP server support](../extending-with-mcp-servers/)
before starting this lab.

## Getting Started

1. Follow the instructions in the [lab guide](../../../../src/content/docs/modules/mcp-servers/integrating-mcp-servers.mdx)
2. Implement tool discovery from MCP servers
3. Create a dynamic Semantic Kernel plugin for MCP tools
4. Test with real MCP servers

## What You'll Build

- `McpToolDiscovery` - Discovers tools from connected MCP servers
- `McpToolsPlugin` - Dynamic Semantic Kernel plugin for MCP tools
- Integration with the agent loop to execute MCP tools
- Error handling and timeout management

## Testing

Use the filesystem MCP server for testing:

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

Try prompts like:
- "List the files in /tmp"
- "Read the contents of /tmp/test.txt"
- "Create a new file at /tmp/hello.txt with the text 'Hello, World!'"
