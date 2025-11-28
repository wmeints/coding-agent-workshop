using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CodingAgent.Plugins.TaskMemory;

public class TodoItem
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
    
    [JsonPropertyName("status")]
    [Description("The status of the TODO item (pending, in_progress, or completed)")]
    public string Status { get; set; }
}