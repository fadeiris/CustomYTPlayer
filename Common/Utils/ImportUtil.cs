using CustomYTPlayer.Extensions;
using CustomYTPlayer.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace CustomYTPlayer.Common.Utils;

/// <summary>
/// 匯入工具類別
/// </summary>
public static class ImportUtil
{
    // TODO: 2022-09-30 未來待看如何改善 BindingList<T>.Add() 的效能。
    // 參考內容：https://junglecoder.com/blog/PerformanceGotchasBindingList

    /// <summary>
    /// 匯入資料（*.json 時間標記）
    /// </summary>
    /// <param name="control1">DataGridView</param>
    /// <param name="sharedDataSource">BindingList&lt;SongDataTimeStamp&gt;</param>
    /// <param name="importDataSource">List&lt;SongDataTimeStamp&gt;?</param>
    /// <param name="filePath">字串，匯入檔案的路徑</param>
    /// <param name="control2">TextBox</param>
    /// <param name="appName">字串，應用程式的名稱</param>
    public static void ImportData(
        DataGridView control1,
        BindingList<SongDataTimeStamp> sharedDataSource,
        List<SongDataTimeStamp>? importDataSource,
        string filePath,
        TextBox control2,
        string appName)
    {
        if (importDataSource != null)
        {
            control1.InvokeIfRequired(() =>
            {
                control1.SuspendLayout();

                int addCount = 0;

                Stopwatch stopwatch = new();

                stopwatch.Start();

                // 排除在 sharedDataSource 內已存在的資料。
                List<SongDataTimeStamp> filteredDataSource = importDataSource
                    .Where(n => !sharedDataSource.Any(m => m.VideoID == n.VideoID &&
                        m.Name == n.Name))
                    .ToList();

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"資料過濾共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                sharedDataSource.RaiseListChangedEvents = false;

                foreach (SongDataTimeStamp importData in filteredDataSource)
                {
                    sharedDataSource.Add(importData);

                    addCount++;
                }

                sharedDataSource.RaiseListChangedEvents = true;
                sharedDataSource.ResetBindings();

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"資料處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                control1.AutoResizeRowHeadersWidth(
                    DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                control1.ResumeLayout();

                if (control1.RowCount > 0)
                {
                    control1.FirstDisplayedScrollingRowIndex = control1.RowCount - 1;
                }

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"UI 處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                string rawMsgOpsResult = $"已載入時間標記播放清單檔案：{filePath}，" +
                    $"共載入 {addCount}/{importDataSource.Count} 個項目。";

                CustomFunction.WriteLog(control2, rawMsgOpsResult);
            });
        }
        else
        {
            string rawMsgInvalidFormat = "所選之檔案，無法解析出有效的播放清單資訊，" +
                "請確認該檔案的內容格式是否正確。";

            CustomFunction.WriteLog(control2, rawMsgInvalidFormat);

            MessageBox.Show(rawMsgInvalidFormat, appName);
        }
    }

    /// <summary>
    /// 匯入資料（*.json 秒數）
    /// </summary>
    /// <param name="control1">DataGridView</param>
    /// <param name="sharedDataSource">BindingList&lt;SongDataTimeStamp&gt;</param>
    /// <param name="importDataSource">List&lt;SongDataSeconds&gt;?</param>
    /// <param name="filePath">字串，匯入檔案的路徑</param>
    /// <param name="control2">TextBox</param>
    /// <param name="appName">字串，應用程式的名稱</param>
    public static void ImportData(
        DataGridView control1,
        BindingList<SongDataTimeStamp> sharedDataSource,
        List<SongDataSeconds>? importDataSource,
        string filePath,
        TextBox control2,
        string appName)
    {
        if (importDataSource != null)
        {
            control1.InvokeIfRequired(() =>
            {
                control1.SuspendLayout();

                int addCount = 0;

                Stopwatch stopwatch = new();

                stopwatch.Start();

                // 排除在 sharedDataSource 內已存在的資料。
                List<SongDataSeconds> filteredDataSource = importDataSource
                    .Where(n => !sharedDataSource.Any(m => n.VideoID == n.VideoID &&
                        m.Name == n.Name))
                    .ToList();

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"資料過濾共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                sharedDataSource.RaiseListChangedEvents = false;

                foreach (SongDataSeconds importData in filteredDataSource)
                {
                    sharedDataSource.Add(new SongDataTimeStamp()
                    {
                        VideoID = importData.VideoID,
                        Name = importData.Name,
                        StartTime = importData.StartSeconds.HasValue ?
                            TimeSpan.FromSeconds(importData.StartSeconds.Value) :
                            TimeSpan.FromSeconds(0),
                        EndTime = importData.EndSeconds.HasValue ?
                            TimeSpan.FromSeconds(importData.EndSeconds.Value) :
                            TimeSpan.FromSeconds(0),
                        SubSrc = importData.SubSrc
                    });

                    addCount++;
                }

                sharedDataSource.RaiseListChangedEvents = true;
                sharedDataSource.ResetBindings();

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"資料處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                control1.AutoResizeRowHeadersWidth(
                    DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                control1.ResumeLayout();

                if (control1.RowCount > 0)
                {
                    control1.FirstDisplayedScrollingRowIndex = control1.RowCount - 1;
                }

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"UI 處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                string rawMsgOpsResult = $"已載入秒數播放清單檔案：{filePath}，" +
                    $"共載入 {addCount}/{importDataSource.Count} 個項目。";

                CustomFunction.WriteLog(control2, rawMsgOpsResult);
            });
        }
        else
        {
            string rawMsgInvalidFormat = "所選之檔案，無法解析出有效的播放清單資訊，" +
                "請確認該檔案的內容格式是否正確。";

            CustomFunction.WriteLog(control2, rawMsgInvalidFormat);

            MessageBox.Show(rawMsgInvalidFormat, appName);
        }
    }

    /// <summary>
    /// 匯入資料（*.jsonc）
    /// </summary>
    /// <param name="control1">DataGridView</param>
    /// <param name="sharedDataSource">BindingList&lt;SongDataTimeStamp&gt;</param>
    /// <param name="importDataSource">List&lt;List&lt;object&gt;&gt;?</param>
    /// <param name="filePath">字串，匯入檔案的路徑（或網址）</param>
    /// <param name="control2">TextBox</param>
    /// <param name="appName">字串，應用程式的名稱</param>
    public static void ImportData(
        DataGridView control1,
        BindingList<SongDataTimeStamp> sharedDataSource,
        List<List<object>>? importDataSource,
        string filePath,
        TextBox control2,
        string appName)
    {
        if (importDataSource != null && importDataSource.Count > 0)
        {
            control1.InvokeIfRequired(() =>
            {
                control1.SuspendLayout();

                Stopwatch stopwatch = new();

                stopwatch.Start();

                // 排除不支援網站的域名的資料、在 sharedDataSource 內已存在的資料。
                List<List<object>> filteredDataSource = importDataSource
                    .Where(n => CustomFunction.IsSupportedSite(n[0].ToString()) == true &&
                        !sharedDataSource.Any(m => m.VideoID == n[0].ToString() &&
                            m.Name == n[3].ToString()))
                    .ToList();

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"資料過濾共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                // 取得不支援網站的域名的資料。
                List<List<object>> notSupportedDataSource = importDataSource
                    .Where(n => !CustomFunction.IsSupportedSite(n[0].ToString()))
                    .ToList();

                foreach (List<object> data in notSupportedDataSource)
                {
                    CustomFunction.WriteLog(control2, $"不支援：{data[3]} [{data[0]}]");
                }

                CustomFunction.WriteLog(control2, $"共 {notSupportedDataSource.Count} 個來自不支援網站的域名的資料。");

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"顯示不支援的資料共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                sharedDataSource.RaiseListChangedEvents = false;

                foreach (List<object> data in filteredDataSource)
                {
                    sharedDataSource.Add(new SongDataTimeStamp()
                    {
                        VideoID = data[0].ToString(),
                        Name = data[3].ToString(),
                        StartTime = TimeSpan.FromSeconds(Convert.ToInt32(data[1].ToString())),
                        EndTime = TimeSpan.FromSeconds(Convert.ToInt32(data[2].ToString())),
                        SubSrc = data.Count > 4 ? data[4].ToString() : string.Empty
                    });
                }

                sharedDataSource.RaiseListChangedEvents = true;
                sharedDataSource.ResetBindings();

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"資料處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                control1.AutoResizeRowHeadersWidth(
                    DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                control1.ResumeLayout();

                if (control1.RowCount > 0)
                {
                    control1.FirstDisplayedScrollingRowIndex = control1.RowCount - 1;
                }

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"UI 處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                string rawSuccessCount = filteredDataSource.Count != importDataSource.Count ?
                    $"{filteredDataSource.Count}/{importDataSource.Count}" :
                    $"{filteredDataSource.Count}";

                string rawMsgOpsResult = $"已載入 JSON 檔案（含備註）：{filePath}，" +
                    $"共載入 {rawSuccessCount} 個項目。";

                CustomFunction.WriteLog(control2, rawMsgOpsResult);
            });
        }
        else
        {
            string rawMsgInvalidFormat = "所選之檔案，無法解析出有效的播放清單資訊，" +
                "請確認該檔案的內容格式是否正確。";

            CustomFunction.WriteLog(control2, rawMsgInvalidFormat);

            MessageBox.Show(rawMsgInvalidFormat, appName);
        }
    }

    /// <summary>
    /// 匯入資料（*.txt 時間標記）
    /// </summary>
    /// <param name="control1">DataGridView</param>
    /// <param name="sharedDataSource">BindingList&lt;SongDataTimeStamp&gt;</param>
    /// <param name="filePath">字串，匯入檔案的路徑</param>
    /// <param name="control2">TextBox</param>
    /// <param name="appName">字串，應用程式的名稱</param>
    public static void ImportData(
        DataGridView control1,
        BindingList<SongDataTimeStamp> sharedDataSource,
        string filePath,
        TextBox control2,
        string appName)
    {
        Stopwatch stopwatch = new();

        stopwatch.Start();

        string[] rawLines = File.ReadAllLines(filePath);

        bool canProcess = false;

        List<SongDataTimeStamp> importDataSource = new();

        SongDataTimeStamp? songData = null;

        int timestampCount = 0;

        string videoID = string.Empty;

        foreach (string rawLine in rawLines)
        {
            // 分隔用行。
            if (rawLine == "-------")
            {
                canProcess = false;

                continue;
            }

            // 從網址取得影片的 ID。
            if (rawLine.Contains("網址："))
            {
                canProcess = false;

                videoID = CustomFunction.GetYouTubeID(rawLine.Replace("網址：", string.Empty));

                continue;
            }

            if (rawLine == "時間標記：")
            {
                canProcess = true;

                continue;
            }

            if (canProcess && !string.IsNullOrEmpty(rawLine))
            {
                // 判斷是否為備註列。
                if (rawLine.Contains('#'))
                {
                    string songName = string.Empty;

                    // 判斷是否為開始的點。
                    if (rawLine.Contains("（開始）"))
                    {
                        songData = new();

                        // 去除不必要的內容。
                        songName = rawLine.Replace("#", string.Empty)
                            .Replace("（開始）", string.Empty)
                            .TrimStart();
                    }

                    if (!string.IsNullOrEmpty(songName))
                    {
                        if (songData != null)
                        {
                            songData.VideoID = videoID;
                            songData.Name = songName;
                            songData.StartTime = null;
                            songData.EndTime = null;
                        }
                    }
                }
                else
                {
                    string[] timestampSet = rawLine.Split(
                        new char[] { '｜' },
                        StringSplitOptions.RemoveEmptyEntries);

                    if (timestampSet.Length > 0 &&
                        !string.IsNullOrEmpty(timestampSet[2]))
                    {
                        if (songData != null)
                        {
                            int rawSeconds = Convert.ToInt32(timestampSet[2]);

                            // timestampCount 為 0 時，設定 songData.StartTime。
                            if (timestampCount == 0)
                            {
                                songData.StartTime = TimeSpan.FromSeconds(rawSeconds);

                                timestampCount++;
                            }
                            else if (timestampCount == 1)
                            {
                                // timestampCount 為 1 時，設定 songData.EndTime。
                                songData.EndTime = TimeSpan.FromSeconds(rawSeconds);

                                // 重置 timestampCount 為 0，以供下一個流程使用。
                                timestampCount = 0;

                                // 將 songData 加入 songDataList;
                                importDataSource.Add(songData);

                                // 在加入 songDataList 後清空。
                                songData = null;
                            }
                        }
                    }
                }
            }
        }

        stopwatch.Stop();

        CustomFunction.WriteLog(control2, $"資料前處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

        if (importDataSource.Count > 0)
        {
            control1.InvokeIfRequired(() =>
            {
                control1.SuspendLayout();

                stopwatch.Restart();

                // 排除在 sharedDataSource 內已存在的資料。
                List<SongDataTimeStamp> filteredDataSource = importDataSource
                    .Where(n => !sharedDataSource.Any(m =>
                        m.VideoID == n.VideoID &&
                        m.Name == n.Name))
                    .ToList();

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"資料過濾共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                sharedDataSource.RaiseListChangedEvents = false;

                foreach (SongDataTimeStamp currentSongData in filteredDataSource)
                {
                    sharedDataSource.Add(currentSongData);
                }

                sharedDataSource.RaiseListChangedEvents = true;
                sharedDataSource.ResetBindings();

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"資料處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                stopwatch.Restart();

                control1.AutoResizeRowHeadersWidth(
                    DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                control1.ResumeLayout();

                if (control1.RowCount > 0)
                {
                    control1.FirstDisplayedScrollingRowIndex = control1.RowCount - 1;
                }

                stopwatch.Stop();

                CustomFunction.WriteLog(control2, $"UI 處理共耗時 {stopwatch.Elapsed.TotalSeconds} 秒。");

                string rawSuccessCount = filteredDataSource.Count != importDataSource.Count ?
                    $"{filteredDataSource.Count}/{importDataSource.Count}" :
                    $"{filteredDataSource.Count}";

                string rawMsgOpsResult = $"已載入時間標記文字檔案：{filePath}，" +
                    $"共載入 {rawSuccessCount} 個項目。";

                CustomFunction.WriteLog(control2, rawMsgOpsResult);
            });
        }
        else
        {
            string rawMsgInvalidformat = "所選之檔案，無法解析出有效的時間標記資訊，" +
                "請確認該檔案的內容格式是否正確。";

            CustomFunction.WriteLog(control2, rawMsgInvalidformat);

            MessageBox.Show(rawMsgInvalidformat, appName);
        }
    }
}