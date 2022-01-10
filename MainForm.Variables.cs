using CustomYTPlayer.Models;
using DiscordRPC;
using Mpv.NET.Player;
using System.ComponentModel;

namespace CustomYTPlayer;

// 阻擋設計工具。
partial class DesignerBlocker { }

public partial class MainForm
{
    /// <summary>
    /// 強制更新相依性檔案
    /// </summary>
    public bool IsForceUpdateDependency = false;

    /// <summary>
    /// MpvPlayer
    /// </summary>
    public MpvPlayer? MpvPlayer = null;

    /// <summary>
    /// 共用的 DiscordRpcClient
    /// </summary>
    public DiscordRpcClient? SharedDiscordRpcClient = null;

    /// <summary>
    /// IHttpClientFactory
    /// </summary>
    private static IHttpClientFactory? SharedHttpClientFactory = null;

    /// <summary>
    /// 共用的 HttpClient
    /// </summary>
    private static HttpClient? SharedHttpClient = null;

    /// <summary>
    /// 共用的 CancellationTokenSource
    /// </summary>
    private CancellationTokenSource? SharedCTS = null;

    /// <summary>
    /// 共用的 CancellationToken
    /// </summary>
    private CancellationToken SharedCT;

    /// <summary>
    /// 共用的資料來源
    /// </summary>
    private readonly BindingList<SongDataTimeStamp> SharedDataSource = new();

    /// <summary>
    /// 共用的網路播放清單資料來源
    /// </summary>
    private readonly BindingList<ComboBoxItem> SharedPlaylistSource = new();

    /// <summary>
    /// 共用的開始時間
    /// </summary>
    private TimeSpan? SharedStartTime = null;

    /// <summary>
    /// 共用的結束時間
    /// </summary>
    private TimeSpan? SharedEndTime = null;

    /// <summary>
    /// 共用的影片長度
    /// </summary>
    private TimeSpan? SharedTargetDuration = null;

    /// <summary>
    /// 共用的索引值
    /// </summary>
    private int SharedCurrentIndex = 0;

    /// <summary>
    /// 播放狀態
    /// </summary>
    private bool IsPlaying = false;

    /// <summary>
    /// 先前的目前秒數
    /// </summary>
    private int PreviousCurrentSeconds = -1;

    /// <summary>
    /// 影片彈出 Form 的彈出狀態
    /// </summary>
    private bool IsSharedVideoPopupFormPopup = false;

    /// <summary>
    /// 共用的影片彈出 Form
    /// </summary>
    private Form? SharedVideoPopupForm = null;

    /// <summary>
    /// 共用的暫存 Timestamps
    /// </summary>
    private Timestamps? SharedTempTimestamps = null;

    /// <summary>
    /// Discord 豐富狀態連線失敗次數
    /// </summary>
    private int ConnectionFailedCount = 0;

    /// <summary>
    /// 共用的目前歌曲資料
    /// </summary>
    private SongDataTimeStamp? SharedCurrentSongData = null;
}