using System.ComponentModel;
using System.Text.Json.Serialization;

namespace CustomYTPlayer.Models;

/// <summary>
/// 類別：播放清單
/// </summary>
public class Playlists
{
    [JsonPropertyName("name")]
    [Description("檔案名稱")]
    public string? Name { get; set; }

    [JsonPropertyName("name_display")]
    [Description("顯示的名稱")]
    public string? NameDisplay { get; set; }

    [JsonPropertyName("tag")]
    [Description("標籤")]
    public List<string>? Tag { get; set; }

    [JsonPropertyName("route")]
    [Description("路由")]
    public string? Route { get; set; }

    [JsonPropertyName("maintainer")]
    [Description("維護者")]
    public Maintainer? Maintainer { get; set; }

    [JsonPropertyName("singer")]
    [Description("歌手")]
    public string? Singer { get; set; }
}