using CustomYTPlayer.Extensions;
using Mpv.NET.Player;
using System.Diagnostics;
using System.IO.Enumeration;
using System.Text.RegularExpressions;

namespace CustomYTPlayer.Common;

/// <summary>
/// 自定義函式
/// </summary>
public static partial class CustomFunction
{
    /// <summary>
    /// 取得影片畫質
    /// </summary>
    /// <param name="index">數值</param>
    /// <returns>YouTubeDlVideoQuality</returns>
    public static YouTubeDlVideoQuality GetQuality(int index)
    {
        return index switch
        {
            0 => YouTubeDlVideoQuality.Highest,
            1 => YouTubeDlVideoQuality.High,
            2 => YouTubeDlVideoQuality.MediumHigh,
            3 => YouTubeDlVideoQuality.Medium,
            4 => YouTubeDlVideoQuality.LowMedium,
            5 => YouTubeDlVideoQuality.Low,
            6 => YouTubeDlVideoQuality.Lowest,
            _ => YouTubeDlVideoQuality.LowMedium
        };
    }

    /// <summary>
    /// 取得 YouTube 影片網址的影片 ID
    /// <para>來源：https://gist.github.com/takien/4077195 </para>
    /// </summary>
    /// <param name="url">字串，網址</param>
    /// <returns>字串，影片 ID</returns>
    public static string GetYouTubeID(string url)
    {
        string rawResult = string.Empty;
        string rawPattern = @"(vi\/|v%3D|v=|\/v\/|youtu\.be\/|\/embed\/)";

        // 0：網址；1：v=；2：影片 ID。 
        string[] rawSplitted = Regex.Split(url, rawPattern);

        if (rawSplitted.Length == 3)
        {
            rawResult = rawSplitted[2];
        }

        return rawResult;
    }

    /// <summary>
    /// 判斷是不是支援的網站
    /// </summary>
    /// <param name="url">字串，網址</param>
    /// <returns>布林值</returns>
    public static bool IsSupportedSite(string? url)
    {
        bool isSupportedSite = true;

        string rawStr = Properties.Settings.Default.UnsupportedDomains;

        if (!string.IsNullOrEmpty(rawStr))
        {
            string[]? rawUnsupportedSites = rawStr?.Split(
                new char[] { ';' },
                StringSplitOptions.RemoveEmptyEntries);

            if (rawUnsupportedSites != null)
            {
                if (!string.IsNullOrEmpty(url))
                {
                    foreach (string rawDomain in rawUnsupportedSites)
                    {
                        if (url.Contains(rawDomain))
                        {
                            isSupportedSite = false;

                            break;
                        }
                    }
                }
                else
                {
                    isSupportedSite = false;
                }
            }
        }

        return isSupportedSite;
    }

    /// <summary>
    /// 寫紀錄
    /// </summary>
    /// <param name="control">TextBox，TBLog</param>
    /// <param name="message">字串，訊息</param>
    /// <param name="enableDebug">布林值，輸出 Debug，預設值為 false</param>
    /// <param name="enableAutoClear">布林值，自動執行 TextBox.Clear()，預設值為 true</param>
    public static void WriteLog(TextBox control,
        string message,
        bool enableDebug = false,
        bool enableAutoClear = true)
    {
        try
        {
            // 當 message 不為空白或空值時才輸出。
            if (!string.IsNullOrEmpty(message))
            {
                string outputMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

                control.InvokeIfRequired(() =>
                {
                    // 當有相同的內容存在時，則不輸出。
                    if (!control.Text.Contains(outputMessage))
                    {
                        if (enableDebug)
                        {
                            Debug.WriteLine(outputMessage);
                        }

                        outputMessage += Environment.NewLine;

                        if (enableAutoClear)
                        {
                            // 預測文字字串長度。
                            int predictTextLength = control.Text.Length + outputMessage.Length;

                            // 當 predictTextLength 大於或等於 TextBox 的上限值時。
                            if (predictTextLength >= control.MaxLength)
                            {
                                // 清除 TextBox。
                                control.Clear();

                                if (enableDebug)
                                {
                                    Debug.WriteLine("已執行 TextBox.Clear()。");
                                }
                            }
                        }

                        control.AppendText(outputMessage);
                    }
                });
            }
            else
            {
                Debug.WriteLine("變數 message 為空白或是空值。");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"輸入的訊息：{message}");
            Debug.WriteLine(ex);
        }
    }

    /// <summary>
    /// 批次設定啟用
    /// </summary>
    /// <param name="controls">Control[]</param>
    /// <param name="enabled">布林值，啟用，預設值為 true</param>
    public static void BatchSetEnabled(Control[] controls, bool enabled = true)
    {
        foreach (Control control in controls)
        {
            if (control is DataGridView dataGridView)
            {
                dataGridView.ReadOnly = !enabled;
                dataGridView.AllowUserToAddRows = enabled;
                dataGridView.AllowUserToDeleteRows = enabled;
                dataGridView.AllowDrop = enabled;
            }
            else
            {
                control.Enabled = enabled;
            }
        }
    }

    /// <summary>
    /// 批次設定啟用
    /// </summary>
    /// <param name="controls">ToolStripMenuItem[]</param>
    /// <param name="enabled">布林值，啟用，預設值為 true</param>
    public static void BatchSetEnabled(ToolStripMenuItem[] controls, bool enabled = true)
    {
        foreach (ToolStripMenuItem control in controls)
        {
            control.Enabled = enabled;
        }
    }

    /// <summary>
    /// 開啟網址
    /// </summary>
    /// <param name="url">字串，網址</param>
    /// <returns>Process</returns>
    public static Process? OpenUrl(string url)
    {
        return Process.Start(new ProcessStartInfo()
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    /// <summary>
    /// EnumerateFiles
    /// <para>來源：https://stackoverflow.com/a/72291652 </para>
    /// </summary>
    /// <param name="path">字串，路徑</param>
    /// <param name="searchPatterns">字串，搜尋模式</param>
    /// <param name="searchOption">SearchOption，預設值是 SearchOption.TopDirectoryOnly</param>
    /// <returns>IEnumerable&lt;string&gt;</returns>
    public static IEnumerable<string> EnumerateFiles(
        string path,
        string[] searchPatterns,
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return Directory.EnumerateFiles(path, "*", searchOption)
            .Where(fileName => searchPatterns
            .Any(pattern =>
            {
                if (!pattern.StartsWith("*"))
                {
                    pattern = $"*{pattern}";
                }

                return FileSystemName.MatchesSimpleExpression(pattern, fileName);
            }));
    }
}