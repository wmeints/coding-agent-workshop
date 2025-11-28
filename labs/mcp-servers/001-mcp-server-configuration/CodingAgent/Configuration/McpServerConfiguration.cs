using System.Text.Json;
using ModelContextProtocol.Client;

namespace CodingAgent.Configuration;

public class McpServerConfiguration
{
    private McpServerConfiguration(Dictionary<string, McpServerConfigurationItem> servers)
    {
        Servers = servers;
    }

    public Dictionary<string, McpServerConfigurationItem> Servers { get; }

    public static async Task<McpServerConfiguration> Load(string baseDirectory)
    {
        var configurationFilePath = Path.Join(baseDirectory, ".agent", "mcp.json");
        await using var fileStream = File.OpenRead(configurationFilePath);

        var configuration = await JsonSerializer.DeserializeAsync<Dictionary<string, McpServerConfigurationItem>>(fileStream);

        return new McpServerConfiguration(configuration!);
    }

    public async Task<List<McpClient>> CreateAllClientsAsync()
    {
        var clients = new List<McpClient>();

        foreach (var serverConfiguration in Servers)
        {
            clients.Add(await serverConfiguration.Value.CreateClientAsync());
        }

        return clients;
    }
}