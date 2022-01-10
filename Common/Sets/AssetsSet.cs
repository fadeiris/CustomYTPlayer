using DiscordRPC;

namespace CustomYTPlayer.Common.Sets;

/// <summary>
/// Assets 組
/// </summary>
public static class AssetsSet
{
    /// <summary>
    /// 資產：正在播放
    /// </summary>
    public static readonly Assets AssetsPlay = new()
    {
        LargeImageKey = "app_icon",
        LargeImageText = StringSet.AppName,
        SmallImageKey = "play",
        SmallImageText = StringSet.StatePlay
    };

    /// <summary>
    /// 資產：暫停播放
    /// </summary>
    public static readonly Assets AssetsPause = new()
    {
        LargeImageKey = "app_icon",
        LargeImageText = StringSet.AppName,
        SmallImageKey = "pause",
        SmallImageText = StringSet.StatePause
    };

    /// <summary>
    /// 資產：停止播放
    /// </summary>
    public static readonly Assets AssetsStop = new()
    {
        LargeImageKey = "app_icon",
        LargeImageText = StringSet.AppName,
        SmallImageKey = "stop",
        SmallImageText = StringSet.StateStop
    };
}