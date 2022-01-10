using CustomYTPlayer.Common;
using CustomYTPlayer.Common.Sets;
using CustomYTPlayer.Common.Utils;
using CustomYTPlayer.Controls;
using CustomYTPlayer.Extensions;
using CustomYTPlayer.Models;
using Mpv.NET.Player;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using YoutubeDLSharp;

namespace CustomYTPlayer;

public partial class MainForm : Form
{
    public MainForm(IHttpClientFactory httpClientFactory)
    {
        InitializeComponent();

        SharedHttpClientFactory = httpClientFactory;
        SharedHttpClient = SharedHttpClientFactory.CreateClient();
        SharedVideoPopupForm ??= CreateVideoPopupForm();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        CustomInit();
        CheckAppVersion();

        if (Properties.Settings.Default.EnableDiscordRichPresence)
        {
            InitDCRichPresence();
        }
    }

    private void MainForm_Shown(object? sender, EventArgs e)
    {
        // 延後 1.5 秒後再執行。
        Task.Delay(1500).ContinueWith(task =>
        {
            // 自動載入播放清單檔案。
            DoAutoLoadPlaylists();
        });
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            Task.Run(() =>
            {
                // 手動觸發事件。
                BtnStop_Click(this, null);
            }).ContinueWith(task =>
            {
                // 釋放 HttpClient。
                if (SharedHttpClient != null)
                {
                    SharedHttpClient.Dispose();
                    SharedHttpClient = null;
                }

                // 在應用程式關閉時 Dispose() MpvPlayer。
                MpvPlayer?.Dispose();

                // 清除 SharedControl.SharedNotifyIcon。
                if (SharedControl.SharedNotifyIcon != null)
                {
                    SharedControl.SharedNotifyIcon.Visible = false;
                    SharedControl.SharedNotifyIcon.Dispose();
                }

                // Dispose() Discord 的 Rich Presence。
                DisposeDCRichPresence();
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        // 排除組合按鍵。
        if (e.Control || e.Shift || e.Alt)
        {
            return;
        }

        switch (e.KeyCode)
        {
            case Keys.Q:
                Application.Exit();

                break;
            case Keys.W:
                if (IsSharedVideoPopupFormPopup)
                {
                    // 重設彈出 Form 的大小與位置。
                    if (SharedVideoPopupForm != null &&
                        !SharedVideoPopupForm.IsDisposed)
                    {
                        SharedVideoPopupForm.InvokeIfRequired(() =>
                        {
                            SharedVideoPopupForm.Width = 1280;
                            SharedVideoPopupForm.Height = 720;
                            SharedVideoPopupForm.Location = new Point(60, 60);
                        });
                    }
                }

                break;
            case Keys.E:
                PPlayerHost_DoubleClick(this, EventArgs.Empty);

                break;
            case Keys.R:
                if (IsSharedVideoPopupFormPopup)
                {
                    // 開關彈出 Form 的全螢幕顯示。
                    if (SharedVideoPopupForm != null &&
                        !SharedVideoPopupForm.IsDisposed)
                    {
                        SharedVideoPopupForm.InvokeIfRequired(() =>
                        {
                            if (SharedVideoPopupForm.WindowState != FormWindowState.Maximized)
                            {
                                SharedVideoPopupForm.FormBorderStyle = FormBorderStyle.None;
                                SharedVideoPopupForm.WindowState = FormWindowState.Normal;
                                SharedVideoPopupForm.WindowState = FormWindowState.Maximized;
                            }
                            else
                            {
                                SharedVideoPopupForm.FormBorderStyle = FormBorderStyle.Sizable;
                                SharedVideoPopupForm.WindowState = FormWindowState.Normal;
                            }
                        });
                    }
                }

                break;
            case Keys.A:
                if (BtnPlay.Enabled)
                {
                    BtnPlay_Click(this, EventArgs.Empty);
                }

                break;
            case Keys.S:
                if (BtnPause.Enabled)
                {
                    BtnPause_Click(this, EventArgs.Empty);
                }

                break;
            case Keys.D:
                if (BtnPrevious.Enabled)
                {
                    BtnPrevious_Click(this, EventArgs.Empty);
                }

                break;
            case Keys.F:
                if (BtnNext.Enabled)
                {
                    BtnNext_Click(this, EventArgs.Empty);
                }

                break;
            case Keys.G:
                if (BtnStop.Enabled)
                {
                    BtnStop_Click(this, EventArgs.Empty);
                }

                break;
            case Keys.Z:
                if (BtnRandomPlay.Enabled)
                {
                    BtnRandomPlay_Click(this, EventArgs.Empty);
                }

                break;
            case Keys.X:
                if (BtnRandomPlaylist.Enabled)
                {
                    BtnRandomPlaylist_Click(this, EventArgs.Empty);
                }

                break;
            case Keys.C:
                if (CBNotShowVideo.Enabled)
                {
                    CBNotShowVideo.Checked = !CBNotShowVideo.Checked;
                }

                break;
            case Keys.V:
                if (BtnMute.Enabled)
                {
                    BtnMute_Click(this, EventArgs.Empty);
                }

                break;
            default:
                break;
        }
    }

    private void PPlayerHost_DoubleClick(object? sender, EventArgs? e)
    {
        if (SharedVideoPopupForm != null)
        {
            if (SharedVideoPopupForm.IsDisposed)
            {
                SharedVideoPopupForm = CreateVideoPopupForm();
            }

            if (IsSharedVideoPopupFormPopup)
            {
                SharedVideoPopupForm.Close();
            }
            else
            {
                SharedVideoPopupForm.Show();
            }
        }
    }

    private void BtnPlay_Click(object sender, EventArgs? e)
    {
        DoPlay();
    }

    private void BtnPause_Click(object sender, EventArgs? e)
    {
        try
        {
            if (IsPlaying)
            {
                MpvPlayer?.Pause();

                ShowNotify(Text, StringSet.MsgPause);
            }
            else
            {
                MpvPlayer?.Resume();

                ShowNotify(Text, StringSet.MsgResume);
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void BtnPrevious_Click(object sender, EventArgs? e)
    {
        try
        {
            SharedCurrentIndex--;

            if (SharedCurrentIndex < 0)
            {
                SharedCurrentIndex = 0;

                WriteLog(StringSet.MsgNoPreviousItem);

                ShowNotify(Text, StringSet.MsgNoPreviousItem);
            }
            else
            {
                DoPlay();
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void BtnNext_Click(object sender, EventArgs? e)
    {
        try
        {
            SharedCurrentIndex++;

            if (SharedCurrentIndex >= SharedDataSource.Count)
            {
                SharedCurrentIndex--;

                WriteLog(StringSet.MsgNoNextItem);

                ShowNotify(Text, StringSet.MsgNoNextItem);
            }
            else
            {
                DoPlay();
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void BtnStop_Click(object sender, EventArgs? e)
    {
        try
        {
            // 2022-10-14
            // 重設 PreviousCurrentSeconds，
            // 使其無法滿足 MpvPlayer_PositionChanged()
            // 事件內 expression2 的條件。
            PreviousCurrentSeconds = -1;

            // 停止播放。
            MpvPlayer?.Stop();

            SetControlEnabled(true);

            // 取消靜音。
            TBVolume.InvokeIfRequired(() =>
            {
                if (!TBVolume.Enabled)
                {
                    if (MpvPlayer != null)
                    {
                        int volumeVal = Properties.Settings.Default.Volume;

                        if (volumeVal <= 0)
                        {
                            volumeVal = 100;

                            Properties.Settings.Default.Volume = volumeVal;
                            Properties.Settings.Default.Save();
                        }

                        MpvPlayer.Volume = volumeVal;
                    }

                    TBVolume.Enabled = true;
                }
            });

            ShowNotify(Text, StringSet.MsgStop);

            SharedCurrentIndex = 0;

            if (SharedVideoPopupForm != null &&
                !SharedVideoPopupForm.IsDisposed)
            {
                SharedVideoPopupForm.Close();
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private async void BtnSavePlaylist_Click(object sender, EventArgs e)
    {
        try
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnSavePlaylist,
                BtnClearPlaylist,
                BtnRandomPlaylist,
                DgvSongList
            },
            false);

            if (SharedDataSource.Count > 0)
            {
                string defaultFileName = $"{VariableSet.DefaultPlaylistFileName}{DateTime.Now:yyyyMMdd}";

                SaveFileDialog saveFileDialog = new()
                {
                    Filter = "時間標記播放清單檔案（*.json）|*.json|秒數播放清單檔案（*.json）|*.json|JSON 檔案（含備註）|*.jsonc|時間標記文字檔案（*.txt）|*.txt",
                    Title = "儲存播放清單檔案",
                    FileName = defaultFileName,
                    FilterIndex = 1,
                    AddExtension = true,
                    InitialDirectory = VariableSet.PlaylistsFolderPath
                };

                DialogResult dialogResult = saveFileDialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    string originFilePath = saveFileDialog.FileName;
                    string originFileName = Path.GetFileNameWithoutExtension(originFilePath);

                    // 當檔案名稱為預設名稱的時候，才會變更儲存的檔案名稱。
                    if (originFilePath.Contains(defaultFileName))
                    {
                        // 變更檔案名稱。
                        switch (saveFileDialog.FilterIndex)
                        {
                            case 1:
                                saveFileDialog.FileName = originFilePath.Replace(
                                    originFileName,
                                    $"CustomYTPlayer_Playlist_Timestamps_{DateTime.Now:yyyyMMdd}");

                                break;
                            case 2:
                                saveFileDialog.FileName = originFilePath.Replace(
                                    originFileName,
                                    $"CustomYTPlayer_Playlist_Seconds_{DateTime.Now:yyyyMMdd}");

                                break;
                            case 3:
                                saveFileDialog.FileName = originFilePath.Replace(
                                    originFileName,
                                    $"SongList_{DateTime.Now:yyyyMMdd}");

                                break;
                            case 4:
                                saveFileDialog.FileName = originFilePath.Replace(
                                    originFileName,
                                    $"Exported_timestamps_{DateTime.Now:yyyyMMdd}");

                                break;
                            default:
                                break;
                        }
                    }

                    using FileStream fileStream = (FileStream)saveFileDialog.OpenFile();

                    // 來源：https://stackoverflow.com/a/59260196
                    JsonSerializerOptions options = new()
                    {
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                        WriteIndented = true
                    };

                    string outputContent = string.Empty;

                    switch (saveFileDialog.FilterIndex)
                    {
                        case 1:
                            await JsonSerializer.SerializeAsync(fileStream, SharedDataSource, options);

                            break;
                        case 2:
                            List<SongDataSeconds> dataSource = new();

                            foreach (SongDataTimeStamp songData in SharedDataSource)
                            {
                                dataSource.Add(new SongDataSeconds()
                                {
                                    VideoID = songData.VideoID,
                                    Name = songData.Name,
                                    StartSeconds = songData.StartTime.HasValue ? songData.StartTime.Value.TotalSeconds : 0,
                                    EndSeconds = songData.EndTime.HasValue ? songData.EndTime.Value.TotalSeconds : 0,
                                    SubSrc = songData.SubSrc
                                });
                            }

                            await JsonSerializer.SerializeAsync(fileStream, dataSource, options);

                            break;
                        case 3:
                            // 來源：https://github.com/jim60105/Playlists/blob/BasePlaylist/Template/TemplateSongList.jsonc
                            string jsoncHeader = $"/**{Environment.NewLine}" +
                                $" * 歌單格式為JSON with Comments{Environment.NewLine}" +
                                $" * [\"VideoID\", StartTime, EndTime, \"Title\", \"SubSrc\"]{Environment.NewLine}" +
                                $" * VideoID: 必須用引號包住，為字串型態。{Environment.NewLine}" +
                                $" * StartTime: 只能是非負數。如果要從頭播放，輸入0{Environment.NewLine}" +
                                $" * EndTime: 只能是非負數。如果要播放至尾，輸入0{Environment.NewLine}" +
                                $" * Title?: 必須用引號包住，為字串型態{Environment.NewLine}" +
                                $" * SubSrc?: 必須用雙引號包住，為字串型態，可選{Environment.NewLine}" +
                                $" */{Environment.NewLine}";

                            outputContent += jsoncHeader;
                            outputContent += $"[{Environment.NewLine}";

                            int countIdx1 = 0;

                            foreach (SongDataTimeStamp songData in SharedDataSource)
                            {
                                string endComma = countIdx1 == SharedDataSource.Count - 1 ? string.Empty : ",";

                                outputContent += $"    [\"{songData.VideoID}\", " +
                                    $"{(songData.StartTime.HasValue ? songData.StartTime.Value.TotalSeconds : 0)}, " +
                                    $"{(songData.EndTime.HasValue ? songData.EndTime.Value.TotalSeconds : 0)}, " +
                                    $"\"{songData.Name}\", " +
                                    $"\"{songData.SubSrc}\"]{endComma}{Environment.NewLine}";

                                countIdx1++;
                            }

                            outputContent += "]";

                            break;
                        case 4:
                            string headerTemplate = $"網址：https://www.youtube.com/watch?v={{VideoID}}{Environment.NewLine}{Environment.NewLine}" +
                                $"格式：{{FFmpeg 時間格式}}｜{{YouTube 留言}}｜{{YouTube 秒數}}｜{{Twitch 時間格式}}{Environment.NewLine}" +
                                $"格式說明：{Environment.NewLine}" +
                                $"1. 給予在 FFmpeg 的參數 -ss 或 -t 帶入值使用。{Environment.NewLine}" +
                                $"> e.g. .\\ffmpeg.exe -ss {{時間標記}} -i input.mp4 -vcodec copy -acodec copy -t {{時間標記}} -o output.mp4{Environment.NewLine}" +
                                $"2. 給予在 YouTube 留言內標記時間點使用。{Environment.NewLine}" +
                                $"3. 給予在 YouTube 網址參數 t 帶入值使用。{Environment.NewLine}" +
                                $"> e.g. https://www.youtube.com/watch?v={{影片 ID}}&t={{時間標記}}s{Environment.NewLine}" +
                                $"4. 給予在 Twitch 網址參數 t 帶入值使用。{Environment.NewLine}" +
                                $"> e.g. https://www.twitch.tv/videos/{{影片 ID}}?t={{時間標記}}{Environment.NewLine}{Environment.NewLine}" +
                                $"※時間標記，請以 YouTube / Twitch 網站上的影片為準。{Environment.NewLine}{Environment.NewLine}";

                            var grpSongDataSet = SharedDataSource.GroupBy(n => n.VideoID);

                            foreach (var grpSongData in grpSongDataSet)
                            {
                                int countIdx2 = 0;

                                foreach (SongDataTimeStamp songData in grpSongData)
                                {
                                    if (countIdx2 == 0)
                                    {
                                        outputContent += headerTemplate.Replace("{VideoID}", grpSongData.Key);
                                        outputContent += $"時間標記：{Environment.NewLine}";
                                    }

                                    // 不輸出字幕檔來源。
                                    outputContent += $"# {songData.Name}（開始）{Environment.NewLine}" +
                                            $"{(songData.StartTime.HasValue ? songData.StartTime.Value.ToTimestampString() : string.Empty)}" +
                                            $"{Environment.NewLine}# {songData.Name}（結束）{Environment.NewLine}" +
                                            $"{(songData.EndTime.HasValue ? songData.EndTime.Value.ToTimestampString() : string.Empty)}" +
                                            $"{Environment.NewLine}{Environment.NewLine}";

                                    countIdx2++;
                                }

                                if (countIdx2 > 1 && countIdx2 < grpSongDataSet.Count() - 1)
                                {
                                    // 分隔用行。
                                    outputContent += $"{VariableSet.BlockSeparator}{Environment.NewLine}{Environment.NewLine}";
                                }
                            }

                            break;
                        default:
                            break;
                    }

                    if (saveFileDialog.FilterIndex == 3 || saveFileDialog.FilterIndex == 4)
                    {
                        using StreamWriter streamWriter = new(fileStream);

                        streamWriter.Write(outputContent);

                        await streamWriter.DisposeAsync();
                    }

                    await fileStream.DisposeAsync();

                    string rawMsgOpsResult = $"已將播放清單儲存至：{saveFileDialog.FileName}";

                    WriteLog(rawMsgOpsResult);

                    MessageBox.Show(rawMsgOpsResult, Text);
                }
            }
            else
            {
                WriteLog(StringSet.MsgPlaylistIsEmpty);

                MessageBox.Show(StringSet.MsgPlaylistIsEmpty, Text);
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
        finally
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnSavePlaylist,
                BtnClearPlaylist,
                BtnRandomPlaylist,
                DgvSongList
            },
            true);
        }
    }

    private void BtnLoadPlaylist_Click(object sender, EventArgs e)
    {
        try
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnSavePlaylist,
                BtnClearPlaylist,
                BtnRandomPlaylist,
                DgvSongList
            },
            false);

            CBNetPlaylists.InvokeIfRequired(async () =>
            {
                if (CBNetPlaylists.SelectedItem is ComboBoxItem comboBoxItem)
                {
                    string playlistName = comboBoxItem.Text,
                        playlistUrl = comboBoxItem.Value;

                    if (playlistName == StringSet.SelectPalylistFile)
                    {
                        // 手動選擇的播放清單檔案。
                        DoLoadPlaylist();
                    }
                    else if (playlistName.Contains(StringSet.LocalFilePrefix))
                    {
                        // 在 Playlists 資料夾內的本機的播放清單檔案。
                        DoLoadPlaylists(new List<string>()
                        {
                            playlistUrl
                        });
                    }
                    else
                    {
                        #region 網路播放清單檔案

                        if (playlistName.ToLower().Contains("member"))
                        {
                            MessageBox.Show(@"請先調整 lib\yt-dlp.conf 內的設定 --cookies-from-browser，以支援播放頻道會員專屬的內容。",
                                Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }

                        if (SharedHttpClient != null)
                        {
                            string rawJson = await SharedHttpClient
                                .GetStringAsync(playlistUrl);

                            if (!string.IsNullOrEmpty(rawJson))
                            {
                                JsonSerializerOptions options = new()
                                {
                                    // 忽略掉註解。
                                    ReadCommentHandling = JsonCommentHandling.Skip,
                                    WriteIndented = true
                                };

                                // 支援來自：https://github.com/jim60105/Playlists 的 *.jsonc 檔案。
                                List<List<object>>? importDataSet3 = JsonSerializer
                                    .Deserialize<List<List<object>>>(rawJson, options);

                                WriteLog($"正在載入網路播放清單「{playlistName}」。");

                                ImportUtil.ImportData(
                                    DgvSongList,
                                    SharedDataSource,
                                    importDataSet3,
                                    playlistUrl,
                                    TBLog,
                                    Text);
                            }
                        }

                        #endregion
                    }
                }
                else
                {
                    // 播放清單檔案。
                    DoLoadPlaylist();
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
        finally
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnSavePlaylist,
                BtnClearPlaylist,
                BtnRandomPlaylist,
                DgvSongList
            },
            true);

            CBNetPlaylists.InvokeIfRequired(() =>
            {
                object dataSource = CBNetPlaylists.DataSource;

                if (dataSource != null)
                {
                    CBNetPlaylists.SelectedIndex = 0;
                }
            });
        }
    }

    private void CBAutoLyric_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CBAutoLyric.InvokeIfRequired(() =>
            {
                if (CBAutoLyric.Checked != Properties.Settings.Default.EnableAutoLyric)
                {
                    Properties.Settings.Default.EnableAutoLyric = CBAutoLyric.Checked;
                    Properties.Settings.Default.Save();
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void BtnClearPlaylist_Click(object sender, EventArgs e)
    {
        try
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnSavePlaylist,
                BtnClearPlaylist,
                BtnRandomPlaylist,
                DgvSongList
            },
            false);

            if (SharedDataSource.Count > 0)
            {
                DgvSongList.InvokeIfRequired(() =>
                {
                    DgvSongList.Rows.Clear();

                    WriteLog(StringSet.MsgPlaylistCleared);

                    MessageBox.Show(StringSet.MsgPlaylistCleared, Text);
                });
            }
            else
            {
                WriteLog(StringSet.MsgPlaylistIsEmpty);

                MessageBox.Show(StringSet.MsgPlaylistIsEmpty, Text);
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
        finally
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnSavePlaylist,
                BtnClearPlaylist,
                BtnRandomPlaylist,
                DgvSongList
            },
            true);
        }
    }

    private void BtnRefreshNetPlaylists_Click(object sender, EventArgs e)
    {
        try
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
            },
            false);

            InitCBNetPlaylists();
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
        finally
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
            },
            true);
        }
    }

    private void DgvSongList_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
    {
        try
        {
            e.Row.Cells[0].Value = "影片 ID";
            e.Row.Cells[1].Value = "名稱";
            e.Row.Cells[2].Value = TimeSpan.FromSeconds(0);
            e.Row.Cells[3].Value = TimeSpan.FromSeconds(0);
            e.Row.Cells[4].Value = string.Empty;
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            // 避免雙擊標題列跟新增列，導致觸發播放。
            if (e.RowIndex != -1 && e.RowIndex != DgvSongList.NewRowIndex)
            {
                // 當點選的欄位是數字欄位時，才執行選取播放。
                if (e.ColumnIndex == -1)
                {
                    SharedCurrentIndex = e.RowIndex;

                    DoPlay();
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_DragEnter(object sender, DragEventArgs e)
    {
        try
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_DragDrop(object sender, DragEventArgs e)
    {
        try
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) != null)
                {
                    List<string>? fileList = ((string[]?)e.Data.GetData(DataFormats.FileDrop))
                        ?.Where(n => VariableSet.AllowedExts.Contains(Path.GetExtension(n)))
                        .ToList();

                    if (fileList != null)
                    {
                        if (fileList.Count == 0)
                        {
                            MessageBox.Show(
                                "請選擇有效的播放清單檔案或時間標記文字檔案。",
                                Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            return;
                        }

                        DoLoadPlaylists(fileList);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        try
        {
            DgvSongList.InvokeIfRequired(() =>
            {
                if (e.Exception != null)
                {
                    string exceptionMsg = string.Empty;

                    if (!string.IsNullOrEmpty(e.Exception.Message))
                    {
                        exceptionMsg = e.Exception.Message;
                    }
                    else
                    {
                        exceptionMsg = e.Exception.ToString();
                    }

                    if (!string.IsNullOrEmpty(exceptionMsg))
                    {
                        DgvSongList.CancelEdit();

                        MessageBox.Show(
                            exceptionMsg,
                            Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                    else
                    {
                        Debug.WriteLine("exceptionMsg 是空值。");
                    }
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        try
        {
            DgvSongList.InvokeIfRequired(() =>
            {
                // 不在新增列動作。
                if (e.RowIndex != DgvSongList.NewRowIndex)
                {
                    int currentRowIdx = e.RowIndex,
                    actualItemNo = currentRowIdx + 1;

                    DataGridViewRow row = DgvSongList.Rows[currentRowIdx];
                    DataGridViewCellCollection cells = row.Cells;
                    DataGridViewHeaderCell headerCell = row.HeaderCell;

                    // 在標題 Cell 內設定數字。
                    // 避免重複設定。
                    if (headerCell.Value != null)
                    {
                        if (headerCell.Value.ToString() != actualItemNo.ToString())
                        {
                            headerCell.Value = actualItemNo.ToString();
                        }
                    }
                    else
                    {
                        headerCell.Value = actualItemNo.ToString();
                    }

                    // 僅針對時間欄位處理。
                    if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
                    {
                        DataGridViewCell cell = cells[e.ColumnIndex];

                        TimeSpan? timeSpan = cell.Value as TimeSpan?;

                        double totalSeconds = timeSpan.HasValue ? timeSpan.Value.TotalSeconds : 0.0d;

                        if (totalSeconds != 0.0d)
                        {
                            cell.ToolTipText = totalSeconds.ToString();
                        }
                    }
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        try
        {
            DgvSongList.InvokeIfRequired(() =>
            {
                // 在新增列不動作。
                if (e.RowIndex != DgvSongList.NewRowIndex)
                {
                    DataGridViewRow row = DgvSongList.Rows[e.RowIndex];
                    DataGridViewCellCollection cells = row.Cells;
                    DataGridViewCell cell = cells[e.ColumnIndex];

                    string columnName = DgvSongList.Columns[e.ColumnIndex].HeaderCell.Value.ToString() ?? string.Empty;
                    string cellValue = cell.EditedFormattedValue?.ToString() ?? string.Empty;

                    // 判斷輸入的值是否為空值。
                    if (string.IsNullOrEmpty(cellValue))
                    {
                        // 排除字幕檔欄位。
                        if (e.ColumnIndex != 4)
                        {
                            e.Cancel = true;

                            DgvSongList.CancelEdit();

                            MessageBox.Show(
                                $"「{columnName}」欄位的值不得為空值。",
                                Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }
                    else
                    {
                        // 僅針對字幕檔欄位。
                        if (e.ColumnIndex == 4)
                        {
                            if (!Uri.TryCreate(cellValue, new UriCreationOptions(), out Uri? _))
                            {
                                e.Cancel = true;

                                DgvSongList.CancelEdit();

                                MessageBox.Show(
                                    $"請在「{columnName}」欄位輸入有效的網址。",
                                    Text,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }
                        }
                    }

                    // 僅針對時間欄位處理。
                    if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
                    {
                        string column2Name = DgvSongList.Columns[2].HeaderCell.Value.ToString() ?? string.Empty;
                        string column3Name = DgvSongList.Columns[3].HeaderCell.Value.ToString() ?? string.Empty;

                        TimeSpan? tsStart = null;
                        TimeSpan? tsEnd = null;

                        #region 當輸入數值時

                        if (int.TryParse(cellValue, out int result))
                        {
                            if (result > 86399)
                            {
                                e.Cancel = true;

                                DgvSongList.CancelEdit();

                                MessageBox.Show(
                                    $"「{columnName}」欄位的數值不得大於 86399。",
                                    Text,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }

                            switch (e.ColumnIndex)
                            {
                                case 2:
                                    tsEnd = cells[3].Value as TimeSpan?;

                                    if (tsEnd.HasValue && tsEnd.Value.TotalSeconds < result)
                                    {
                                        e.Cancel = true;

                                        DgvSongList.CancelEdit();

                                        MessageBox.Show(
                                            $"「{columnName}」不得晚於「{column3Name}」。",
                                            Text,
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);

                                        return;
                                    }

                                    break;

                                case 3:
                                    tsStart = cells[2].Value as TimeSpan?;

                                    if (tsStart.HasValue && tsStart.Value.TotalSeconds > result)
                                    {
                                        e.Cancel = true;

                                        DgvSongList.CancelEdit();

                                        MessageBox.Show(
                                            $"「{columnName}」不得早於「{column2Name}」。",
                                            Text,
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);

                                        return;
                                    }

                                    break;

                                default:
                                    break;
                            }

                            return;
                        }

                        #endregion

                        #region 當輸入 TimeSpan 時

                        // 判斷 TimeSpan 的時間是否超過 23 小時。
                        if (cellValue.Contains('.'))
                        {
                            e.Cancel = true;

                            DgvSongList.CancelEdit();

                            MessageBox.Show(
                                $"「{columnName}」欄位的值不得超過 23:59:59。",
                                Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }

                        switch (e.ColumnIndex)
                        {
                            case 2:
                                tsStart = TimeSpan.Parse(cellValue);
                                tsEnd = cells[3].Value as TimeSpan?;

                                if (tsStart.HasValue &&
                                    tsEnd.HasValue &&
                                    tsEnd.Value < tsStart.Value)
                                {
                                    e.Cancel = true;

                                    DgvSongList.CancelEdit();

                                    MessageBox.Show(
                                        $"「{columnName}」不得晚於「{column3Name}」。",
                                        Text,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);

                                    return;
                                }

                                break;

                            case 3:
                                tsStart = cells[2].Value as TimeSpan?;
                                tsEnd = TimeSpan.Parse(cellValue);

                                if (tsStart.HasValue &&
                                    tsEnd.HasValue &&
                                    tsStart.Value > tsEnd.Value)
                                {
                                    e.Cancel = true;

                                    DgvSongList.CancelEdit();

                                    MessageBox.Show(
                                        $"「{columnName}」不得早於「{column2Name}」。",
                                        Text,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);

                                    return;
                                }

                                break;

                            default:
                                break;
                        }

                        #endregion
                    }
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_CellValidated(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            DgvSongList.InvokeIfRequired(() =>
            {
                DataGridViewRow row = DgvSongList.Rows[e.RowIndex];
                DataGridViewCellCollection cells = row.Cells;
                DataGridViewCell cell = cells[e.ColumnIndex];

                string cellValue = cell.EditedFormattedValue?.ToString() ?? string.Empty;

                // 僅針對時間欄位處理。
                if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
                {
                    // 當輸入的值為數值時，則將其轉換成對應的 TimeSpan。
                    if (int.TryParse(cellValue, out int result))
                    {
                        cell.Value = TimeSpan.FromSeconds(result);

                        DgvSongList.Refresh();
                    }
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
        try
        {
            if (SharedCurrentIndex > -1)
            {
                // 取得目前的 SongDataTimeStamp。
                SharedCurrentSongData = SharedDataSource[SharedCurrentIndex];
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void DgvSongList_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
    {
        try
        {
            if (SharedCurrentSongData != null)
            {
                SongDataTimeStamp[] dataSet = SharedDataSource.ToArray();

                int newIndex = Array.FindIndex(
                    dataSet,
                    n => n.VideoID == SharedCurrentSongData.VideoID &&
                        n.Name == SharedCurrentSongData.Name);

                if (newIndex > -1)
                {
                    newIndex = newIndex--;
                }

                SharedCurrentIndex = newIndex;
            }
            else
            {
                SharedCurrentIndex = -1;
            }

            if (SharedCurrentIndex == -1)
            {
                WriteLog("因播放清單的內容已異動，將會從頭開始播放。");
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
        finally
        {
            // 重設 SharedCurrentSongData。
            SharedCurrentSongData = null;
        }
    }

    private void TBSeek_ValueChanged(object? sender, EventArgs e)
    {
        try
        {
            TBSeek.InvokeIfRequired(() =>
            {
                // 當多媒體載入時才可以操控控制項。
                if (MpvPlayer?.IsMediaLoaded == true)
                {
                    // 當 TBSeek 有焦點時才 Seek。
                    if (TBSeek.Focused)
                    {
                        // totalSeconds 有可能會小於 positionSeconds。
                        int positionSeconds = TBSeek.Value;

                        double totalSeconds = MpvPlayer?.Duration.TotalSeconds ?? 0.0d;

                        if (totalSeconds != 0 && positionSeconds > totalSeconds)
                        {
                            positionSeconds = Convert
                                .ToInt32(Math.Floor(Convert.ToDecimal(totalSeconds)));
                        }

                        // 最終的修正判斷。
                        if (positionSeconds < VariableSet.TBSeekMinimum)
                        {
                            positionSeconds = VariableSet.TBSeekMinimum;
                        }

                        MpvPlayer?.SeekAsync(positionSeconds);
                    }
                }
                else
                {
                    // 強制回歸最低值。
                    TBSeek.Value = TBSeek.Minimum;
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void TBVolume_ValueChanged(object? sender, EventArgs e)
    {
        try
        {
            TBVolume.InvokeIfRequired(() =>
            {
                if (MpvPlayer != null)
                {
                    if (TBVolume.Value != Properties.Settings.Default.Volume)
                    {
                        Properties.Settings.Default.Volume = TBVolume.Value;
                        Properties.Settings.Default.Save();
                    }

                    MpvPlayer.Volume = Properties.Settings.Default.Volume;

                    LVolume.InvokeIfRequired(() =>
                    {
                        LVolume.Text = Properties.Settings.Default.Volume.ToString();
                    });
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void CBNotShowVideo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CBNotShowVideo.InvokeIfRequired(() =>
            {
                if (CBNotShowVideo.Checked != Properties.Settings.Default.EnableNoVideo)
                {
                    Properties.Settings.Default.EnableNoVideo = CBNotShowVideo.Checked;
                    Properties.Settings.Default.Save();
                }

                string? rawStatus = MpvPlayer?.API.GetPropertyString("vid");

                if (CBNotShowVideo.Checked)
                {
                    if (rawStatus != "no")
                    {
                        MpvPlayer?.API.SetPropertyString("vid", "no");
                    }
                }
                else
                {
                    if (rawStatus != "auto")
                    {
                        MpvPlayer?.API.SetPropertyString("vid", "auto");
                    }
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void CBQuality_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CBQuality.InvokeIfRequired(() =>
            {
                if (MpvPlayer != null)
                {
                    if (CBQuality.SelectedIndex != Properties.Settings.Default.YouTubeVideoQuality)
                    {
                        Properties.Settings.Default.YouTubeVideoQuality = CBQuality.SelectedIndex;
                        Properties.Settings.Default.Save();

                        MpvPlayer.YouTubeDlVideoQuality = CustomFunction
                            .GetQuality(Properties.Settings.Default.YouTubeVideoQuality);

                        WriteLog(StringSet.MsgApplyOnNextItem);

                        MessageBox.Show(StringSet.MsgApplyOnNextItem, Text);
                    }
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void NUDSpeed_ValueChanged(object sender, EventArgs e)
    {
        try
        {
            if (MpvPlayer != null)
            {
                MpvPlayer.Speed = Convert.ToDouble(NUDSpeed.Value);

                LSpeed.InvokeIfRequired(() =>
                {
                    LSpeed.Text = $"{MpvPlayer.Speed}x";
                });
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void LSpeed_DoubleClick(object sender, EventArgs e)
    {
        try
        {
            if (MpvPlayer != null)
            {
                NUDSpeed.Value = 1;

                MpvPlayer.Speed = Convert.ToDouble(NUDSpeed.Value);

                LSpeed.InvokeIfRequired(() =>
                {
                    LSpeed.Text = $"{MpvPlayer.Speed}x";
                });
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void BtnMute_Click(object sender, EventArgs? e)
    {
        try
        {
            if (MpvPlayer != null)
            {
                if (MpvPlayer.Volume > 0)
                {
                    // 靜音。
                    MpvPlayer.Volume = 0;

                    TBVolume.InvokeIfRequired(() =>
                    {
                        TBVolume.Enabled = false;
                    });
                }
                else
                {
                    // 取消靜音。
                    int volumeVal = Properties.Settings.Default.Volume;

                    if (volumeVal <= 0)
                    {
                        volumeVal = 100;

                        Properties.Settings.Default.Volume = volumeVal;
                        Properties.Settings.Default.Save();
                    }

                    MpvPlayer.Volume = volumeVal;

                    TBVolume.InvokeIfRequired(() =>
                    {
                        TBVolume.Enabled = true;
                    });
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void BtnExportLog_Click(object sender, EventArgs e)
    {
        try
        {
            TBLog.InvokeIfRequired(async () =>
            {
                if (TBLog.TextLength > 0)
                {
                    SaveFileDialog saveFileDialog = new()
                    {
                        Filter = "文字檔|*.txt",
                        Title = "儲存檔案",
                        FileName = $"CustomYTPlayer_Log_{DateTime.Now:yyyyMMdd}",
                        FilterIndex = 1,
                        AddExtension = true
                    };

                    DialogResult dialogResult = saveFileDialog.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                    {
                        using FileStream fileStream = (FileStream)saveFileDialog.OpenFile();

                        byte[] bytes = Encoding.UTF8.GetBytes(TBLog.Text);

                        fileStream.Seek(0, SeekOrigin.Begin);

                        await fileStream.WriteAsync(bytes);
                        await fileStream.DisposeAsync();

                        string rawMsgOpsResult = $"已將紀錄儲存至：{saveFileDialog.FileName}";

                        WriteLog(rawMsgOpsResult);

                        MessageBox.Show(rawMsgOpsResult, Text);
                    }
                }
                else
                {
                    string rawMsgLogIsEmpty = "紀錄是空的。";

                    WriteLog(rawMsgLogIsEmpty);

                    MessageBox.Show(rawMsgLogIsEmpty, Text);
                }
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void BtnClearLog_Click(object sender, EventArgs e)
    {
        try
        {
            TBLog.InvokeIfRequired(() =>
            {
                TBLog.Clear();

                MessageBox.Show("已清除紀錄。", Text);
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    public async void BtnUpdateYtDlp_Click(object sender, EventArgs? e)
    {
        try
        {
            SetControlEnabled(false);

            // 手動控制控制項的啟用禁用。
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnPause,
                BtnPrevious,
                BtnNext,
                BtnStop,
                BtnMute,
                BtnRandomPlay
            },
            false);

            CustomFunction.BatchSetEnabled(new ToolStripMenuItem[]
            {
                SharedControl.TsmiPause,
                SharedControl.TsmiPrevious,
                SharedControl.TsmiNext,
                SharedControl.TsmiStop,
                SharedControl.TsmiMute,
                SharedControl.TsmiRandomPlay
            },
            false);

            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnClearPlaylist
            },
            true);

            if (IsForceUpdateDependency)
            {
                if (SharedHttpClient != null)
                {
                    SharedCTS = new();
                    SharedCT = SharedCTS.Token;

                    // 檢查 yt-dlp 的相依性檔案及資料夾是否已存在。
                    await ExternalProgram.CheckAppDeps(
                        SharedHttpClient,
                        TBLog,
                        LYtDlpVersion,
                        SharedCT,
                        isForceDownload: true);
                }
            }
            else
            {
                YoutubeDL ytdl = ExternalProgram.GetYoutubeDL();

                string result = await ytdl.RunUpdate();

                WriteLog(result);
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
        finally
        {
            SetControlEnabled(true);

            // 手動控制控制項的啟用禁用。
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnRandomPlay
            },
            true);

            CustomFunction.BatchSetEnabled(new ToolStripMenuItem[]
            {
                SharedControl.TsmiRandomPlay
            },
            true);

            // 更新顯示版本號。
            ExternalProgram.SetYtDlpVer(LYtDlpVersion);

            // 判斷是否要重新啟動應用程式。
            if (IsForceUpdateDependency)
            {
                // 重設 IsForceUpdateDependency。
                IsForceUpdateDependency = false;

                // 重新啟動應用程式。
                Application.Restart();
                Environment.Exit(0);
            }
            else
            {
                // 重設 IsForceUpdateDependency。
                IsForceUpdateDependency = false;
            }
        }
    }

    private void BtnRandomPlaylist_Click(object sender, EventArgs? e)
    {
        try
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnSavePlaylist,
                BtnClearPlaylist,
                DgvSongList,
                BtnRandomPlaylist,
                BtnRandomPlay
            }, false);

            if (SharedDataSource.Count > 0)
            {
                DgvSongList.InvokeIfRequired(() =>
                {
                    DgvSongList.SuspendLayout();

                    // 隨機化 SharedDataSource 的排序。
                    // 來源：https://www.codegrepper.com/code-examples/csharp/shuffle+arraylist+c+sharp
                    for (int i = 0; i < SharedDataSource.Count; i++)
                    {
                        SongDataTimeStamp currentSong = SharedDataSource[i];

                        int randomIndex = RandomNumberGenerator
                            .GetInt32(0, SharedDataSource.Count - 1);

                        SharedDataSource[i] = SharedDataSource[randomIndex];
                        SharedDataSource[randomIndex] = currentSong;
                    }

                    DgvSongList.AutoResizeRowHeadersWidth(
                        DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                    DgvSongList.ResumeLayout();

                    WriteLog(StringSet.MsgRandomlizePlaylist);

                    MessageBox.Show(StringSet.MsgRandomlizePlaylist, Text);
                });
            }
            else
            {
                WriteLog(StringSet.MsgPlaylistIsEmpty);

                ShowNotify(Text, StringSet.MsgPlaylistIsEmpty);
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
        finally
        {
            CustomFunction.BatchSetEnabled(new Control[]
            {
                BtnLoadPlaylist,
                BtnSavePlaylist,
                BtnClearPlaylist,
                DgvSongList,
                BtnRandomPlaylist,
                BtnRandomPlay
            }, true);
        }
    }

    private void BtnRandomPlay_Click(object sender, EventArgs? e)
    {
        try
        {
            if (SharedDataSource.Count > 0)
            {
                // 隨機產生新數值更新 SharedCurrentIndex。
                SharedCurrentIndex = RandomNumberGenerator
                    .GetInt32(0, SharedDataSource.Count - 1);

                DoPlay();
            }
            else
            {
                WriteLog(StringSet.MsgPlaylistIsEmpty);

                ShowNotify(Text, StringSet.MsgPlaylistIsEmpty);
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void BtnMoreFunction_Click(object sender, EventArgs e)
    {
        try
        {
            MoreFunctionForm moreFunctionForm = new(this);

            moreFunctionForm.Show();
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }
}