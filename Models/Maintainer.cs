using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CustomYTPlayer.Models;

/// <summary>
/// 類別：維護者
/// </summary>
public class Maintainer
{
    [JsonPropertyName("name")]
    [Description("名稱")]
    public string? Name { get; set; }

    [JsonPropertyName("url")]
    [Description("網址")]
    public string? Url { get; set; }
}