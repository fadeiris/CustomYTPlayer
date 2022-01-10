using CustomYTPlayer.Common.Sets;
using CustomYTPlayer.Extensions;
using SevenZipExtractor;

namespace CustomYTPlayer.Common;

/// <summary>
/// 外部程式下載器
/// </summary>
public static class ExternalDownloader
{
    /// <summary>
    /// 下載 libmpv
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="control">TextBox，TBLog</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DownloadLibmpv(
        HttpClient httpClient,
        TextBox control,
        CancellationToken ct)
    {
        if (!Directory.Exists(VariableSet.ExecPath))
        {
            Directory.CreateDirectory(VariableSet.ExecPath);

            CustomFunction.WriteLog(
                control,
                $"已建立 lib 資料夾，路徑：{VariableSet.ExecPath}");
        }

        using FileStream fileStream = new(
            VariableSet.LibmpvArchivePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.ReadWrite);

        Progress<float> progress1 = new();

        progress1.ProgressChanged += async (object? sender, float progress) =>
        {
            if (progress == 100f)
            {
                CustomFunction.WriteLog(control, $"已下載 {VariableSet.LibmpvArchiveName}。");

                await Task.Run(() =>
                {
                    try
                    {
                        using ArchiveFile archiveFile = new(VariableSet.LibmpvArchivePath);

                        foreach (Entry entry in archiveFile.Entries)
                        {
                            if (entry.FileName == VariableSet.LibmpvDllName)
                            {
                                string outputPath = Path.Combine(VariableSet.ExecPath, entry.FileName);

                                entry.Extract(outputPath);

                                // TODO: 2022-10-13 未來待 SevenZipExtractor 函式庫更新後再調整。

                                // 2022-10-13
                                // 等待 5 秒，給 SevenZipExtractor 函式庫充裕的時間進行解壓縮。
                                // SevenZipExtractor 函式庫目前的版本不支援事件回報，故只能採
                                // 用此方式以避免檔案未正確的解壓縮完成。
                                Thread.Sleep(5000);

                                CustomFunction.WriteLog(control, $"已解壓縮 {outputPath}。");

                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        CustomFunction.WriteLog(control, e.Message);
                    }
                }).ContinueWith(task =>
                {
                    try
                    {
                        if (File.Exists(VariableSet.LibmpvArchivePath))
                        {
                            File.Delete(VariableSet.LibmpvArchivePath);

                            CustomFunction.WriteLog(control, $"已刪除 {VariableSet.LibmpvArchivePath}。");
                        }
                    }
                    catch (Exception e)
                    {
                        CustomFunction.WriteLog(control, e.Message);
                    }

                    CustomFunction.WriteLog(control, $"已下載 libmpv。");
                });
            }
        };

        Progress<string> progress2 = new();

        progress2.ProgressChanged += (object? sender, string progress) =>
        {
            CustomFunction.WriteLog(control, $"libmpv 下載進度：{progress}");
        };

        await httpClient.DownloadDataAsync(
            VariableSet.LibmpvArchiveUrl,
            fileStream,
            progress1,
            progress2,
            cancellationToken: ct);
    }

    /// <summary>
    /// 下載 ytdl_hook.lua
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="control">TextBox，TBLog</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DownloadYtDlHookLua(
        HttpClient httpClient,
        TextBox control,
        CancellationToken ct)
    {
        if (!Directory.Exists(VariableSet.ExecPath))
        {
            Directory.CreateDirectory(VariableSet.ExecPath);

            CustomFunction.WriteLog(
                control,
                $"已建立 lib 資料夾，路徑：{VariableSet.ExecPath}");
        }

        using FileStream fileStream = new(
            VariableSet.YtDlHookLuaPath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.ReadWrite);

        Progress<float> progress1 = new();

        progress1.ProgressChanged += async (object? sender, float progress) =>
        {
            if (progress == 100f)
            {
                bool isEdit = false;

                string[] lines = File.ReadAllLines(VariableSet.YtDlHookLuaPath);

                for (int i = 0; i < lines.Length; i++)
                {
                    // 替換 ytdl_path 的值。
                    if (lines[i].Contains("ytdl_path = \""))
                    {
                        lines[i] = "    ytdl_path = \"lib\\\\yt-dlp\",";

                        isEdit = true;

                        break;
                    }
                }

                await File.WriteAllLinesAsync(VariableSet.YtDlHookLuaPath, lines);

                CustomFunction.WriteLog(control, "已下載 ytdl_hook.lua。");

                if (!isEdit)
                {
                    CustomFunction.WriteLog(control, "注意！修改 ytdl_hook.lua 內容失敗，請手動檢查此檔案。");
                }
                else
                {
                    CustomFunction.WriteLog(control, "已成功修改 ytdl_hook.lua 內的 ytdl_path 的值。");
                }
            }
        };

        Progress<string> progress2 = new();

        progress2.ProgressChanged += (object? sender, string progress) =>
        {
            CustomFunction.WriteLog(control, $"ytdl_hook.lua 下載進度：{progress}");
        };

        await httpClient.DownloadDataAsync(
            VariableSet.YtDlHookLuaUrl,
            fileStream,
            progress1,
            progress2,
            cancellationToken: ct);
    }

    /// <summary>
    /// 下載 yt-dlp.exe
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="control">TextBox，TBLog</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DownloadYtDlp(
        HttpClient httpClient,
        TextBox control,
        CancellationToken ct)
    {
        if (!Directory.Exists(VariableSet.ExecPath))
        {
            Directory.CreateDirectory(VariableSet.ExecPath);

            CustomFunction.WriteLog(
                control,
                $"已建立 lib 資料夾，路徑：{VariableSet.ExecPath}");
        }

        using FileStream fileStream = new(
            VariableSet.YtDlpExePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None);

        Progress<float> progress1 = new();

        progress1.ProgressChanged += (object? sender, float progress) =>
        {
            if (progress == 100f)
            {
                CustomFunction.WriteLog(control, "已下載 yt-dlp.exe。");
            }
        };

        Progress<string> progress2 = new();

        progress2.ProgressChanged += (object? sender, string progress) =>
        {
            CustomFunction.WriteLog(control, $"yt-dlp.exe 下載進度：{progress}");
        };

        await httpClient.DownloadDataAsync(
            VariableSet.YtDlpBinaryUrl,
            fileStream,
            progress1,
            progress2,
            cancellationToken: ct);
    }
}