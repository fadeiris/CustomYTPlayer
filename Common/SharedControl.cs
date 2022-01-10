namespace CustomYTPlayer.Controls;

/// <summary>
/// 共用的控制項
/// </summary>
public partial class SharedControl
{
    /// <summary>
    /// NotifyIcon
    /// </summary>
    private static NotifyIcon? sharedNotifyIcon;

    /// <summary>
    /// 共用的 Tooltip
    /// </summary>
    public static readonly ToolTip SharedTooltip = new()
    {
        ShowAlways = true
    };

    /// <summary>
    /// 共用的 NotifyIcon
    /// </summary>
    public static NotifyIcon? SharedNotifyIcon { get => sharedNotifyIcon; set => sharedNotifyIcon = value; }

    /// <summary>
    /// 共用的 ContextMenuStrip
    /// </summary>
    public static readonly ContextMenuStrip SharedContextMenuStrip = new();

    /// <summary>
    /// ToolStripMenuItem 顯示／隱藏
    /// </summary>
    public static readonly ToolStripMenuItem TsmiShow = new()
    {
        Text = "顯示／隱藏",
        ToolTipText = "顯示／隱藏"
    };

    /// <summary>
    /// ToolStripMenuItem 離開
    /// </summary>
    public static readonly ToolStripMenuItem TsmiExit = new()
    {
        Text = "離開",
        ToolTipText = "離開"
    };

    /// <summary>
    /// ToolStripMenuItem 播放
    /// </summary>
    public static readonly ToolStripMenuItem TsmiPlay = new()
    {
        Text = "播放",
        ToolTipText = "播放"
    };

    /// <summary>
    /// ToolStripMenuItem 隨機播放
    /// </summary>
    public static readonly ToolStripMenuItem TsmiRandomPlay = new()
    {
        Text = "隨機播放",
        ToolTipText = "隨機播放"
    };

    /// <summary>
    /// ToolStripMenuItem 暫停／恢復播放
    /// </summary>
    public static readonly ToolStripMenuItem TsmiPause = new()
    {
        Text = "暫停／恢復播放",
        ToolTipText = "暫停／恢復播放"
    };

    /// <summary>
    /// ToolStripMenuItem 靜音／取消靜音
    /// </summary>
    public static readonly ToolStripMenuItem TsmiMute = new()
    {
        Text = "靜音／取消靜音",
        ToolTipText = "靜音／取消靜音"
    };

    /// <summary>
    /// ToolStripMenuItem 上一個
    /// </summary>
    public static readonly ToolStripMenuItem TsmiPrevious = new()
    {
        Text = "上一個",
        ToolTipText = "上一個"
    };

    /// <summary>
    /// ToolStripMenuItem 下一個
    /// </summary>
    public static readonly ToolStripMenuItem TsmiNext = new()
    {
        Text = "下一個",
        ToolTipText = "下一個"
    };

    /// <summary>
    /// ToolStripMenuItem 停止播放
    /// </summary>
    public static readonly ToolStripMenuItem TsmiStop = new()
    {
        Text = "停止播放",
        ToolTipText = "停止播放"
    };
}