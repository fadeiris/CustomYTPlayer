using CustomYTPlayer.Common.Sets;
using CustomYTPlayer.Extensions;
using YoutubeDLSharp;

namespace CustomYTPlayer.Common;

/// <summary>
/// 外部程式
/// </summary>
public static class ExternalProgram
{
    /// <summary>
    /// 檢查本應用程式的相依性檔案
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="control1">TextBox，TBLog</param>
    /// <param name="control2">Label，LYtDlpVersion</param>
    /// <param name="ct">CancellationToken</param>
    /// <param name="isForceDownload">布林值，是否強制下載，預設值為 false</param>
    /// <returns>Task</returns>
    public static async Task CheckAppDeps(
        HttpClient? httpClient,
        TextBox control1,
        Label control2,
        CancellationToken ct,
        bool isForceDownload = false)
    {
        ct.ThrowIfCancellationRequested();

        // 檢查 Playlists 資料夾。
        if (!Directory.Exists(VariableSet.PlaylistsFolderPath))
        {
            Directory.CreateDirectory(VariableSet.PlaylistsFolderPath);

            CustomFunction.WriteLog(control1, $"已建立資料夾 {VariableSet.PlaylistsFolderPath}");
        }

        // 檢查 ytdl_hook.lua。
        if (!File.Exists(VariableSet.YtDlHookLuaPath) || isForceDownload)
        {
            if (httpClient != null)
            {
                await ExternalDownloader.DownloadYtDlHookLua(httpClient, control1, ct);
            }
            else
            {
                CustomFunction.WriteLog(control1, "httpClient 是 null。");
            }
        }
        else
        {
            CustomFunction.WriteLog(control1, "已找到 ytdl_hook.lua。");
        }

        // 檢查 libmpv。
        if (!File.Exists(VariableSet.LibmpvDllPath) || isForceDownload)
        {
            if (httpClient != null)
            {
                await ExternalDownloader.DownloadLibmpv(httpClient, control1, ct);
            }
            else
            {
                CustomFunction.WriteLog(control1, "httpClient 是 null。");
            }
        }
        else
        {
            CustomFunction.WriteLog(control1, "已找到 mpv-1.dll。");
        }

        // 檢查 yt-dlp.exe。
        if (!File.Exists(VariableSet.YtDlpExePath) || isForceDownload)
        {
            if (httpClient != null)
            {
                await ExternalDownloader.DownloadYtDlp(httpClient, control1, ct);

                // 在下載後寫入檢查時間，以避免不必要的檢查執行。
                Properties.Settings.Default.YtDlpCheckTime = DateTime.Now;
                Properties.Settings.Default.Save();
            }
            else
            {
                CustomFunction.WriteLog(control1, "httpClient 是 null。");
            }
        }
        else
        {
            CustomFunction.WriteLog(control1, "已找到 yt-dlp.exe。");

            // 當今天的日期晚於設定檔中的日期時。
            if (DateTime.Now.Date > Properties.Settings.Default.YtDlpCheckTime.Date)
            {
                YoutubeDL ytdl = GetYoutubeDL();

                string result = await ytdl.RunUpdate();

                CustomFunction.WriteLog(control1, result);

                Properties.Settings.Default.YtDlpCheckTime = DateTime.Now;
                Properties.Settings.Default.Save();
            }
            else
            {
                string alreadyUpdatedMessage = $"已於 {Properties.Settings.Default.YtDlpCheckTime:yyyy/MM/dd HH:mm:ss} " +
                    "檢查 yt-dlp 是否有新版本，故今日不會再自動檢查；如有需要，請手動執行更新。";

                CustomFunction.WriteLog(control1, alreadyUpdatedMessage);
            }
        }
        
        SetYtDlpVer(control2);

        if (isForceDownload)
        {
            CustomFunction.WriteLog(control1, "※請重新啟動應用程式。");
            CustomFunction.WriteLog(control1, "即將重新啟動應用程式……");
        }
    }

    /// <summary>
    /// 取得 YoutubeDL
    /// </summary>
    /// <returns>YoutubeDL</returns>
    public static YoutubeDL GetYoutubeDL()
    {
        YoutubeDL ytdl = new()
        {
            IgnoreDownloadErrors = true,
            OverwriteFiles = true,
            YoutubeDLPath = VariableSet.YtDlpExePath
        };

        return ytdl;
    }

    /// <summary>
    /// 設定顯示 yt-dlp 的版本號
    /// </summary>
    /// <param name="control">Label，LYtDlpVersion</param>
    public static void SetYtDlpVer(Label control)
    {
        control.InvokeIfRequired(() =>
        {
            YoutubeDL ytdl = GetYoutubeDL();

            control.Text = $"yt-dlp 版本：{ytdl.Version}";
        });
    }
}