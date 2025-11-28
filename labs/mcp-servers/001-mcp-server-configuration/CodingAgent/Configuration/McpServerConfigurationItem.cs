using System.Text.Json.Serialization;
using ModelContextProtocol.Client;

namespace CodingAgent.Configuration;

public class McpServerConfigurationItem
{
    [JsonPropertyName("url")] public string? Url { get; set; }

    [JsonPropertyName("command")] public string? Command { get; set; }

    [JsonPropertyName("args")] public List<string>? Arguments { get; set; }

    [JsonPropertyName("env")] public Dictionary<string, string?>? EnvironmentVariables { get; set; }

    public async Task<McpClient> CreateClientAsync()
    {
        if (!HasValidHttpConfiguration() && !HasValidStdioConfiguration())
        {
            throw new InvalidOperationException(
                "The specified configuration is invalid. Either a valid URL or command and arguments must be provided.");
        }

        if (HasValidHttpConfiguration())
        {
            var transportOptions = new HttpClientTransportOptions
            {
                Endpoint = new Uri(Url!),
            };
            
            return await McpClient.CreateAsync(new HttpClientTransport(transportOptions));
        }
        else
        {
            var transportOptions = new StdioClientTransportOptions
            {
                Command = Command!,
                Arguments = Arguments,
                EnvironmentVariables = EnvironmentVariables
            };

            return await McpClient.CreateAsync(new StdioClientTransport(transportOptions));
        }
    }

    private bool HasValidStdioConfiguration()
    {
        return !String.IsNullOrEmpty(Command);
    }

    private bool HasValidHttpConfiguration()
    {
        return !string.IsNullOrEmpty(Url) && Uri.IsWellFormedUriString(Url, UriKind.Absolute);
    }
}