using CustomYTPlayer.Common;
using CustomYTPlayer.Common.Utils;
using CustomYTPlayer.Controls;
using CustomYTPlayer.Extensions;
using CustomYTPlayer.Models;
using DiscordRPC.Logging;
using DiscordRPC;
using Mpv.NET.API;
using Mpv.NET.Player;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using DiscordRPC.Message;
using CustomYTPlayer.Common.Sets;

namespace CustomYTPlayer;

// 阻擋設計工具。
partial class DesignerBlocker { }

public partial class MainForm
{
    /// <summary>
    /// 自定義初始化
    /// </summary>
    private async void CustomInit()
    {
        try
        {
            Text = StringSet.AppName;
            Icon = Properties.Resources.app_icon;

            // 設定顯示應用程式的版本號。
            LVersion.InvokeIfRequired(() =>
            {
                Version? version = Assembly.GetEntryAssembly()?.GetName().Version;

                if (version != null)
                {
                    LVersion.Text = $"版本：{version}";
                }
                else
                {
                    LVersion.Text = $"版本：無";
                }
            });

            // 設定控制項。
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

            SharedCTS = new();
            SharedCT = SharedCTS.Token;

            // 檢查 yt-dlp 的相依性檔案及資料夾是否已存在。
            await ExternalProgram.CheckAppDeps(SharedHttpClient, TBLog, LYtDlpVersion, SharedCT);

            PPlayerHost.InvokeIfRequired(() =>
            {
                PPlayerHost.DoubleClick += PPlayerHost_DoubleClick;

                // lib\mpv-1.dll 檔案路徑，注意！只能使用 mpv-1.dll，目前不支援 mpv-2.dll。
                string libMpvPath = Path.Combine(AppContext.BaseDirectory, @"lib\mpv-1.dll");

                // lib\mpv.conf 檔案路徑。
                string mpvConfigFilePath = Path.Combine(AppContext.BaseDirectory, @"lib\mpv.conf");

                // lib\ytdl_hook.lua 檔案路徑，注意！可能需要定期更新檔案。
                string ytdlHookScriptPath = Path.Combine(AppContext.BaseDirectory, @"lib\ytdl_hook.lua");

                // 從 App.config 讀取設定。
                bool logVerbose = Properties.Settings.Default.EnableLogVerbose;

                // 初始化 MpvPlayer。
                MpvPlayer = new(hwnd: PPlayerHost.Handle, libMpvPath: libMpvPath)
                {
                    YouTubeDlVideoQuality = CustomFunction
                        .GetQuality(Properties.Settings.Default.YouTubeVideoQuality),
                    Volume = Properties.Settings.Default.Volume,
                    Speed = Convert.ToDouble(NUDSpeed.Value),
                    LogLevel = logVerbose ? MpvLogLevel.V : MpvLogLevel.Info
                };

                // 依據設定值決定是否要顯示影像。
                if (Properties.Settings.Default.EnableNoVideo)
                {
                    MpvPlayer.API.SetPropertyString("vid", "no");
                }
                else
                {
                    MpvPlayer.API.SetPropertyString("vid", "auto");
                }

                /**
                 * 2022-01-18
                 * 
                 * 關於 ytdl_hook.lua 的注意事項。
                 * 經過測試，在 libmpv 內有其實有內置一個 ytdl_hook.lua。
                 * 因此當手動設定 Player.EnableYouTubeDl() 時，
                 * 等同於又再載入了另一份 ytdl_hook.lua。
                 * 
                 * 因為此情況的緣故，所以會發現本程式啟動的 yt-dlp，
                 * 是被加入至環境變數的 yt-dlp，而不是在 ytdl_hook.lua 指定的 yt-dlp。
                 * 
                 * 為了避免此問題，需要設定 Player.API.LoadConfigFile()，
                 * 並在載入的設定檔案內設定 no-ytdl，以取消使用內置的 ytdl_hook.lua。
                 * 
                 * 之後才能載入 lib 資料夾內的 ytdl_hook.lua。
                 * 
                 * ※注意！當設定了 no-ytdl 後，其他與其有關的設定將會無效。
                 */

                // 載入 lib\mpv.conf 檔案。
                MpvPlayer.API.LoadConfigFile(mpvConfigFilePath);

                // 載入 lib\ytdl_hook.lua 檔案。
                MpvPlayer.EnableYouTubeDl(ytdlHookScriptPath: ytdlHookScriptPath);

                // 註冊事件監聽。
                MpvPlayer.API.LogMessage += MpvPlayer_LogMessage;
                MpvPlayer.MediaEndedBuffering += MpvPlayer_MediaEndedBuffering;
                MpvPlayer.MediaEndedSeeking += MpvPlayer_MediaEndedSeeking;
                MpvPlayer.MediaError += MpvPlayer_MediaError;
                MpvPlayer.MediaFinished += MpvPlayer_MediaFinished;
                MpvPlayer.MediaLoaded += MpvPlayer_MediaLoaded;
                MpvPlayer.MediaPaused += MpvPlayer_MediaPaused;
                MpvPlayer.MediaResumed += MpvPlayer_MediaResumed;
                MpvPlayer.MediaStartedBuffering += MpvPlayer_MediaStartedBuffering;
                MpvPlayer.MediaStartedSeeking += MpvPlayer_MediaStartedSeeking;
                MpvPlayer.MediaUnloaded += MpvPlayer_MediaUnloaded;
                MpvPlayer.PositionChanged += MpvPlayer_PositionChanged;
            });

            components = new Container();

            SharedControl.SharedNotifyIcon = new(components)
            {
                Icon = Properties.Resources.app_icon,
                Text = Text,
                Visible = true
            };

            SharedControl.SharedNotifyIcon.MouseDoubleClick += (sender, e) =>
            {
                ShowMainForm();
            };

            SharedControl.TsmiShow.Click += (sender, e) =>
            {
                ShowMainForm();
            };

            SharedControl.TsmiExit.Click += (sender, e) =>
            {
                Application.Exit();
            };

            SharedControl.TsmiPlay.Click += (sender, e) =>
            {
                BtnPlay_Click(this, e);
            };

            SharedControl.TsmiRandomPlay.Click += (sender, e) =>
            {
                BtnRandomPlay_Click(this, e);
            };

            SharedControl.TsmiPause.Click += (sender, e) =>
            {
                BtnPause_Click(this, e);
            };

            SharedControl.TsmiMute.Click += (sender, e) =>
            {
                BtnMute_Click(this, e);
            };

            SharedControl.TsmiPrevious.Click += (sender, e) =>
            {
                BtnPrevious_Click(this, e);
            };

            SharedControl.TsmiNext.Click += (sender, e) =>
            {
                BtnNext_Click(this, e);
            };

            SharedControl.TsmiStop.Click += (sender, e) =>
            {
                BtnStop_Click(this, e);
            };

            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiShow);
            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiExit);
            SharedControl.SharedContextMenuStrip.Items.Add(new ToolStripSeparator());
            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiPlay);
            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiRandomPlay);
            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiPause);
            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiMute);
            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiPrevious);
            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiNext);
            SharedControl.SharedContextMenuStrip.Items.Add(SharedControl.TsmiStop);

            SharedControl.SharedNotifyIcon.ContextMenuStrip = SharedControl.SharedContextMenuStrip;

            DgvSongList.InvokeIfRequired(() =>
            {
                #region 為 DataGridView 設定啟用 DoubleBuffered

                // 來源：https://stackoverflow.com/a/35984368
                if (!SystemInformation.TerminalServerSession)
                {
                    Type type = DgvSongList.GetType();

                    PropertyInfo? propertyInfo = type.GetProperty(
                        "DoubleBuffered",
                        BindingFlags.Instance | BindingFlags.NonPublic);

                    propertyInfo?.SetValue(DgvSongList, true, null);
                }

                #endregion

                DgvSongList.DataSource = SharedDataSource;
            });

            TBSeek.InvokeIfRequired(() =>
            {
                // 註冊事件監聽。
                TBSeek.ValueChanged += TBSeek_ValueChanged;
            });

            LSpeed.InvokeIfRequired(() =>
            {
                LSpeed.Text = $"{MpvPlayer?.Speed}x";
            });

            CBQuality.InvokeIfRequired(() =>
            {
                CBQuality.SelectedIndex = Properties.Settings.Default.YouTubeVideoQuality;
            });

            TBVolume.InvokeIfRequired(() =>
            {
                if (MpvPlayer != null)
                {
                    TBVolume.Value = Properties.Settings.Default.Volume;
                }

                // 註冊事件監聽。
                TBVolume.ValueChanged += TBVolume_ValueChanged;
            });

            LVolume.InvokeIfRequired(() =>
            {
                LVolume.Text = Properties.Settings.Default.Volume.ToString();
            });

            CBNotShowVideo.InvokeIfRequired(() =>
            {
                CBNotShowVideo.Checked = Properties.Settings.Default.EnableNoVideo;
            });

            CBNetPlaylists.InvokeIfRequired(() =>
            {
                CBNetPlaylists.DataSource = SharedPlaylistSource;
            });

            CBAutoLyric.InvokeIfRequired(() =>
            {
                CBAutoLyric.Checked = Properties.Settings.Default.EnableAutoLyric;
            });

            // 加入 Tooltip 提示。
            SharedControl.SharedTooltip.SetToolTip(PPlayerHost, "雙擊以開關彈出視窗檢視");
            SharedControl.SharedTooltip.SetToolTip(PBProgress, "播放進度");
            SharedControl.SharedTooltip.SetToolTip(LDuration, "影片時長");
            SharedControl.SharedTooltip.SetToolTip(TBSeek, "時間軸控制器（※在失去焦點後才會與播放進度同步）");
            SharedControl.SharedTooltip.SetToolTip(CBQuality, "影片畫質（※僅對 YouTube 網站的影片有效）");
            SharedControl.SharedTooltip.SetToolTip(TBVolume, "音量調整");
            SharedControl.SharedTooltip.SetToolTip(LVolume, "目前音量");
            SharedControl.SharedTooltip.SetToolTip(LSpeed, "雙擊兩次以重設影片播放速度至 1x。");
            SharedControl.SharedTooltip.SetToolTip(TBLog, "紀錄");
            SharedControl.SharedTooltip.SetToolTip(DgvSongList, "播放清單");
            SharedControl.SharedTooltip.SetToolTip(BtnRandomPlay,
                "只會進行一次性的隨機選擇，後續的項目會照順序播放，直到最後一個項目為止。");
            SharedControl.SharedTooltip.SetToolTip(CBNetPlaylists, "播放清單檔案清單");
            SharedControl.SharedTooltip.SetToolTip(BtnRefreshNetPlaylists, "重新整理播放清單檔案清單");
            SharedControl.SharedTooltip.SetToolTip(CBAutoLyric,
                "※僅對來自 YoutubeClipPlaylist/Playlists 的播放清單可能有效");

            // 初始化 CBNetPlaylists。
            InitCBNetPlaylists();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, Text);
        }
        finally
        {
            // 設定控制項。
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
        }
    }

    /// <summary>
    /// 執行播放
    /// </summary>
    private void DoPlay()
    {
        try
        {
            Task.Run(() =>
            {
                // 2022-10-14
                // 重設 PreviousCurrentSeconds，
                // 使其無法滿足 MpvPlayer_PositionChanged()
                // 事件內 expression2 的條件。
                PreviousCurrentSeconds = -1;

                // 停止播放。
                MpvPlayer?.Stop();
            }).ContinueWith(task =>
            {
                // 延後 700 毫秒再執行。
                Task.Delay(700).ContinueWith(task =>
                {
                    DgvSongList.InvokeIfRequired(() =>
                    {
                        if (SharedDataSource.Count <= 0)
                        {
                            WriteLog(StringSet.MsgPlaylistIsEmpty);

                            ShowNotify(Text, StringSet.MsgPlaylistIsEmpty);

                            return;
                        }

                        if (SharedCurrentIndex >= 0 && SharedCurrentIndex < SharedDataSource.Count)
                        {
                            SetControlEnabled(false);

                            #region 設定 DataGridView 的顯示狀態

                            DataGridViewRowCollection rows = DgvSongList.Rows;

                            // 只取第一個。
                            DataGridViewRow singleRow = rows[SharedCurrentIndex];

                            // 設定 DgvSongList 的顯示狀態。
                            // 設定已選擇。
                            singleRow.Selected = true;

                            // 設定指示器的位置。
                            DgvSongList.CurrentCell = singleRow.Cells[0];
                            // 設定第一個顯示的列。
                            DgvSongList.FirstDisplayedScrollingRowIndex = SharedCurrentIndex;
                            // 故意讓此控制項取得焦點，讓快捷鍵不會觸發 DgvSongList 的編輯。
                            LVersion.Focus();

                            #endregion

                            SongDataTimeStamp? rawSong = SharedDataSource
                                .ElementAtOrDefault(SharedCurrentIndex);

                            string? videoID = rawSong?.VideoID;
                            string? videoName = rawSong?.Name;
                            string? videoSubSrc = rawSong?.SubSrc;

                            TimeSpan? StartTime = rawSong?.StartTime;
                            TimeSpan? EndTime = rawSong?.EndTime;

                            // 設定共用的開始時間。
                            SharedStartTime = StartTime;

                            // 設定共用的結束時間。
                            SharedEndTime = EndTime;

                            if (SharedStartTime != null)
                            {
                                // 設定共用的影片長度。
                                SharedTargetDuration = SharedEndTime?.Subtract(SharedStartTime.Value);
                            }

                            #region 處理影片名稱

                            LName.InvokeIfRequired(() =>
                            {
                                if (videoName?.Length > 30)
                                {
                                    string tempName = videoName[..28] + "……";

                                    LName.Text = tempName;
                                }
                                else
                                {
                                    LName.Text = videoName;
                                }

                                // 來源：https://stackoverflow.com/a/7238622
                                // 下方的留言內。
                                SharedControl.SharedTooltip.SetToolTip(LName, videoName);

                                // 當 videoName 不為 null 或空值時。
                                if (!string.IsNullOrEmpty(videoName))
                                {
                                    // 顯示氣球通知。
                                    ShowNotify(Text, $"{StringSet.MsgNowPlaying}{videoName}");
                                }
                            });

                            #endregion

                            #region 設定控制項

                            int minVal = Convert.ToInt32(StartTime?.TotalSeconds),
                                maxVal = Convert.ToInt32(EndTime?.TotalSeconds);

                            PBProgress.InvokeIfRequired(() =>
                            {
                                PBProgress.Minimum = minVal;
                                PBProgress.Maximum = maxVal;

                                // 避免發生例外，先判斷 minVal 與 maxVal 的大小關係。
                                if (minVal <= maxVal)
                                {
                                    PBProgress.Value = minVal;
                                }
                            });

                            TBSeek.InvokeIfRequired(() =>
                            {
                                TBSeek.Minimum = minVal;
                                TBSeek.Maximum = maxVal;

                                // 避免發生例外，先判斷 minVal 與 maxVal 的大小關係。
                                if (minVal <= maxVal)
                                {
                                    TBSeek.Value = minVal;
                                }
                            });

                            #endregion

                            if (videoID != null)
                            {
                                bool enableAutoLyric = false;

                                CBAutoLyric.InvokeIfRequired(() =>
                                {
                                    enableAutoLyric = CBAutoLyric.Checked;
                                });

                                if (videoName != null &&
                                    StartTime != null &&
                                    string.IsNullOrEmpty(videoSubSrc) &&
                                    enableAutoLyric)
                                {
                                    // 自動查詢歌曲字幕檔。
                                    string[] lyricData = GetLrcFileUrl(
                                        videoID,
                                        videoName,
                                        StartTime.Value.TotalSeconds.ToString());

                                    // *.lrc 檔案的網址。
                                    videoSubSrc = lyricData[0];

                                    // 設定字幕檔的延遲秒數。
                                    MpvPlayer?.API.SetPropertyString("sub-delay", lyricData[1]);

                                    if (!string.IsNullOrEmpty(videoSubSrc))
                                    {
                                        // 更新回 DgvSongList 的字幕檔欄位。
                                        singleRow.Cells[4].Value = videoSubSrc;
                                    }
                                }

                                // 針對網址特別處裡。
                                if (videoID.Contains("http://") || videoID.Contains("https://"))
                                {
                                    // 非 YouTube 網站的影片套用此設定。 
                                    MpvPlayer?.API.SetPropertyString("ytdl-format", "bv*+ba/b");
                                    MpvPlayer?.Load(videoID);
                                }
                                else
                                {
                                    if (MpvPlayer != null)
                                    {
                                        MpvPlayer.YouTubeDlVideoQuality = CustomFunction
                                            .GetQuality(Properties.Settings.Default.YouTubeVideoQuality);
                                    }

                                    MpvPlayer?.Load($"https://www.youtube.com/watch?v={videoID}");
                                }

                                // 手動設定，以免事件無法觸發造成值未設定。
                                IsPlaying = true;

                                // 播放影片。
                                MpvPlayer?.Resume();

                                // 延後 3 秒後才執行載入字幕檔。
                                Task.Delay(3000).ContinueWith(task =>
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(videoSubSrc))
                                        {
                                            MpvPlayer?.API.Command(new string[]
                                            {
                                                "sub-add",
                                                videoSubSrc
                                            });
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        WriteLog(ex.Message);
                                    }
                                });

                                // 將彈出的 Form 帶到最前方。
                                if (IsSharedVideoPopupFormPopup &&
                                    SharedVideoPopupForm != null &&
                                    !SharedVideoPopupForm.IsDisposed)
                                {
                                    Task.Delay(1000).ContinueWith(task =>
                                    {
                                        SharedVideoPopupForm.InvokeIfRequired(() =>
                                        {
                                            SharedVideoPopupForm.BringToFront();
                                        });
                                    });
                                }
                            }
                            else
                            {
                                CustomFunction.WriteLog(TBLog, $"VideoID「{videoID}」解析失敗，無法播放。");
                            }
                        }
                        else
                        {
                            CustomFunction.WriteLog(TBLog, $"SharedCurrentIndex 的值（{SharedCurrentIndex}）" +
                                $"超出播放清單的陣列長度（{SharedDataSource.Count}），無法播放。");
                        }
                    });
                });
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 重設播放狀態
    /// </summary>
    private void ResetPlayStatus()
    {
        try
        {
            SharedStartTime = null;
            SharedEndTime = null;
            SharedTargetDuration = null;
            IsPlaying = false;

            LName.InvokeIfRequired(() =>
            {
                LName.Text = "無";

                // 清除 Tooltip。
                SharedControl.SharedTooltip.SetToolTip(LName, null);
            });

            LDuration.InvokeIfRequired(() =>
            {
                LDuration.Text = "00:00:00 / 00:00:00";
            });

            PBProgress.InvokeIfRequired(() =>
            {
                PBProgress.Minimum = 0;
                PBProgress.Maximum = 100;
                PBProgress.Value = 0;
            });

            TBSeek.InvokeIfRequired(() =>
            {
                TBSeek.Minimum = 0;
                TBSeek.Maximum = 100;
                TBSeek.Value = 0;
            });

            NUDSpeed.InvokeIfRequired(() =>
            {
                // 重設播放速度。
                NUDSpeed.Value = 1;

                if (MpvPlayer != null)
                {
                    MpvPlayer.Speed = Convert.ToDouble(NUDSpeed.Value);
                }

                LSpeed.InvokeIfRequired(() =>
                {
                    LSpeed.Text = $"{MpvPlayer?.Speed}x";
                });
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 顯示通知
    /// </summary>
    /// <param name="title">字串，標題</param>
    /// <param name="text">字串，文字</param>
    private void ShowNotify(string title, string text)
    {
        try
        {
            if (SharedControl.SharedNotifyIcon != null)
            {
                SharedControl.SharedNotifyIcon.BalloonTipTitle = title;
                SharedControl.SharedNotifyIcon.BalloonTipText = text;
                SharedControl.SharedNotifyIcon.ShowBalloonTip(3 * 1000);

                string videoName = string.Empty;

                LName.InvokeIfRequired(() =>
                {
                    videoName = LName.Text;
                });

                // 顯示應用程式的名稱。
                SharedControl.SharedNotifyIcon.Text = Text;

                if (text.Contains(StringSet.MsgNowPlaying))
                {
                    // 顯示正在播放的項目的名稱。
                    SharedControl.SharedNotifyIcon.Text = text;

                    Timestamps timestamps = Timestamps.Now;

                    if (SharedTargetDuration != null &&
                        SharedTargetDuration.HasValue)
                    {
                        DateTime endTime = DateTime.UtcNow.AddTicks(SharedTargetDuration.Value.Ticks);

                        timestamps.End = endTime;
                    }

                    SetDCRichPresence(
                        videoName,
                        StringSet.StatePlay,
                        timestamps,
                        AssetsSet.AssetsPlay);
                }
                else if (text.Contains(StringSet.MsgPause))
                {
                    // 將目前的豐富狀態的 Timestamps 設給 SharedTempTimestamps 以暫存。
                    SharedTempTimestamps = SharedDiscordRpcClient?.CurrentPresence.Timestamps;

                    SetDCRichPresence(
                        videoName,
                        StringSet.StatePause,
                        assets: AssetsSet.AssetsPause);
                }
                else if (text.Contains(StringSet.MsgResume))
                {
                    SetDCRichPresence(
                        videoName,
                        StringSet.StatePlay,
                        SharedTempTimestamps ?? Timestamps.Now,
                        AssetsSet.AssetsPlay);

                    // 重設 SharedTempTimestamps。
                    SharedTempTimestamps = null;
                }
                else if (text.Contains(StringSet.MsgStop))
                {
                    SetDCRichPresence(
                        state: StringSet.StateStop,
                        assets: AssetsSet.AssetsStop);
                }
                else if (text.Contains(StringSet.MsgPlaylistFinished))
                {
                    SetDCRichPresence(
                        state: StringSet.StateStop,
                        assets: AssetsSet.AssetsStop);
                }
                else
                {
                    SetDCRichPresence();
                }
            }
            else
            {
                Debug.WriteLine("SharedNotifyIcon 是 null。");
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 設定控制項啟用狀態
    /// </summary>
    /// <param name="enable">布林值，啟用，預設值為 true</param>
    private void SetControlEnabled(bool enable = true)
    {
        try
        {
            BtnPlay.InvokeIfRequired(() =>
            {
                BtnPlay.Enabled = enable;
            });

            SharedControl.TsmiPlay.Enabled = enable;

            BtnPause.InvokeIfRequired(() =>
            {
                BtnPause.Enabled = !enable;
            });

            SharedControl.TsmiPause.Enabled = !enable;

            BtnPrevious.InvokeIfRequired(() =>
            {
                BtnPrevious.Enabled = !enable;
            });

            SharedControl.TsmiPrevious.Enabled = !enable;

            BtnNext.InvokeIfRequired(() =>
            {
                BtnNext.Enabled = !enable;
            });

            SharedControl.TsmiNext.Enabled = !enable;

            BtnStop.InvokeIfRequired(() =>
            {
                BtnStop.Enabled = !enable;
            });

            SharedControl.TsmiStop.Enabled = !enable;

            BtnClearPlaylist.InvokeIfRequired(() =>
            {
                BtnClearPlaylist.Enabled = enable;
            });

            BtnMute.InvokeIfRequired(() =>
            {
                BtnMute.Enabled = !enable;
            });

            SharedControl.TsmiMute.Enabled = !enable;

            BtnUpdateYtDlp.InvokeIfRequired(() =>
            {
                BtnUpdateYtDlp.Enabled = enable;
            });
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 設定 MainForm 的顯示／隱藏
    /// </summary>
    private void ShowMainForm()
    {
        if (Visible)
        {
            Hide();
        }
        else
        {
            Show();
        };
    }

    /// <summary>
    /// 初始化網路播放清單的 ComboBox
    /// </summary>
    private async void InitCBNetPlaylists()
    {
        try
        {
            SharedHttpClient ??= SharedHttpClientFactory?.CreateClient();

            if (SharedHttpClient != null)
            {
                string[] urls =
                {
                    VariableSet.YouTubeClipPlaylistPlaylistsJsonUrl,
                    VariableSet.FadeirisCustomPlaylistPlaylistsJsonUrl,
                    VariableSet.FadeirisCustomPlaylistB23PlaylistsJsonUrl
                };

                // 當 SharedPlaylistSource 有資料時。
                if (SharedPlaylistSource.Count > 0)
                {
                    // 先清除舊有資料。
                    SharedPlaylistSource.Clear();
                }

                SharedPlaylistSource.Add(new ComboBoxItem(
                    StringSet.SelectPalylistFile,
                    string.Empty));

                foreach (string url in urls)
                {
                    string rawJson = await SharedHttpClient
                        .GetStringAsync(url);

                    if (!string.IsNullOrEmpty(rawJson))
                    {
                        JsonSerializerOptions options = new()
                        {
                            // 忽略掉註解。
                            ReadCommentHandling = JsonCommentHandling.Skip,
                            WriteIndented = true
                        };

                        List<Playlists>? dataSet = JsonSerializer
                            .Deserialize<List<Playlists>>(rawJson, options);

                        if (dataSet != null)
                        {
                            // 2022-10-03
                            // name 有 backup 的，通常都是放置於 OneDrive，目前 libmpv + yt-dlp 並不支援 OneDrive。
                            // twitcasting 則是不支援 seek。

                            // 過濾資料。
                            dataSet = dataSet.Where(n => !string.IsNullOrEmpty(n.Name) &&
                                !n.Name.ToLower().Contains("backup") &&
                                !string.IsNullOrEmpty(n.NameDisplay) &&
                                !string.IsNullOrEmpty(n.Route) &&
                                n.Tag != null &&
                                !n.Tag.Contains("onedrive") &&
                                !n.Tag.Contains("twitcasting"))
                                .ToList();

                            foreach (Playlists playlists in dataSet)
                            {
                                string text = $"{playlists.NameDisplay}", playlistFileUrl = string.Empty;

                                if (url.Contains(VariableSet.YouTubeClipPlaylistBaseUrl))
                                {
                                    playlistFileUrl = $"{VariableSet.YouTubeClipPlaylistBaseUrl}{playlists.Route}";
                                }
                                else if (url.Contains(VariableSet.FadeirisCustomPlaylistBaseUrl))
                                {
                                    if (playlists.Tag != null &&
                                        playlists.Tag.Contains("bilibili"))
                                    {
                                        text = $"[Bilibili] {text}";
                                    }

                                    playlistFileUrl = $"{VariableSet.FadeirisCustomPlaylistBaseUrl}{playlists.Route}";
                                }
                                else
                                {
                                    playlistFileUrl = playlists.Route ?? string.Empty;
                                }

                                if (!string.IsNullOrEmpty(playlistFileUrl))
                                {
                                    if (playlists.Maintainer != null &&
                                        !string.IsNullOrEmpty(playlists.Maintainer.Name) &&
                                        playlists.Maintainer.Name != "AutoGenerator")
                                    {
                                        text += $" [維護者：{playlists.Maintainer.Name}]";
                                    }

                                    SharedPlaylistSource.Add(new ComboBoxItem(text, playlistFileUrl));
                                }
                            }
                        }
                    }
                }

                // 當 SharedPlaylistSource 有資料時。
                if (SharedPlaylistSource.Count > 0)
                {
                    // 需要去除加入 VariableSet.SelectPalylistFile 所增加的數量。
                    CustomFunction.WriteLog(
                        TBLog,
                        $"已載入網路播放清單列表的項目，共 {SharedPlaylistSource.Count - 1} 項。");
                }
            }

            // 取得 Playlists 資料夾內的播放清單檔案。
            List<string> files = CustomFunction
                .EnumerateFiles(
                    VariableSet.PlaylistsFolderPath,
                    VariableSet.AllowedExts,
                    SearchOption.AllDirectories)
            .ToList();

            foreach (string filePath in files)
            {
                string fileName = $"{StringSet.LocalFilePrefix}{Path.GetFileNameWithoutExtension(filePath)}";

                if (fileName.Contains(VariableSet.DefaultPlaylistFileName))
                {
                    fileName = fileName.Replace(VariableSet.DefaultPlaylistFileName, string.Empty);
                }

                SharedPlaylistSource.Add(new ComboBoxItem(fileName, filePath));
            }

            if (files.Count > 0)
            {
                CustomFunction.WriteLog(
                    TBLog,
                    $"已載入本機播放清單列表的項目，共 {files.Count} 項。");
            }

            // Fallback 使用，當取不到資料時則新增 VariableSet.SelectPalylistFile。
            if (SharedPlaylistSource.Count <= 0)
            {
                SharedPlaylistSource.Add(new ComboBoxItem(StringSet.SelectPalylistFile, string.Empty));
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// 執行自動載入播放清單檔案
    /// </summary>
    private void DoAutoLoadPlaylists()
    {
        List<string> files = CustomFunction
            .EnumerateFiles(
                VariableSet.PlaylistsFolderPath,
                VariableSet.AllowedExts,
                SearchOption.TopDirectoryOnly)
            .ToList();

        DoLoadPlaylists(files);
    }

    /// <summary>
    /// 執行載入播放清單檔案
    /// </summary>
    /// <param name="fileList">List&lt;string&gt;</param>
    private void DoLoadPlaylists(List<string> fileList)
    {
        Task.Run(() =>
        {
            try
            {
                foreach (string filePath in fileList)
                {
                    string extName = Path.GetExtension(filePath),
                        rawJson = File.ReadAllText(filePath);

                    JsonSerializerOptions options = new()
                    {
                        // 忽略掉註解。
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        WriteIndented = true
                    };

                    switch (extName)
                    {
                        case ".txt":
                            ImportUtil.ImportData(
                                DgvSongList,
                                SharedDataSource,
                                filePath,
                                TBLog,
                                Text);

                            break;
                        case ".json":
                            List<SongDataTimeStamp>? importDataSet1 = JsonSerializer
                                .Deserialize<List<SongDataTimeStamp>>(rawJson, options);

                            List<SongDataSeconds>? importDataSet2 = JsonSerializer
                                .Deserialize<List<SongDataSeconds>>(rawJson, options);

                            TimeSpan? timeSpan = importDataSet1?.FirstOrDefault()?.StartTime;

                            double? seconds = importDataSet2?.FirstOrDefault()?.StartSeconds;

                            // 當 timeSpan 不為 null 時，則表示讀取到的是時間標記的 *.json。
                            if (timeSpan != null)
                            {
                                ImportUtil.ImportData(
                                    DgvSongList,
                                    SharedDataSource,
                                    importDataSet1,
                                    filePath,
                                    TBLog,
                                    Text);
                            }
                            else
                            {
                                // 當 seconds 為 null 或大於等於 0 時，則表示讀取到的是秒數的 *.json。
                                if (seconds == null || seconds >= 0)
                                {
                                    ImportUtil.ImportData(
                                        DgvSongList,
                                        SharedDataSource,
                                        importDataSet2,
                                        filePath,
                                        TBLog,
                                        Text);
                                }
                            }

                            break;
                        case ".jsonc":
                            // 支援來自：https://github.com/jim60105/Playlists 的 *.jsonc 檔案。
                            List<List<object>>? importDataSet3 = JsonSerializer
                                .Deserialize<List<List<object>>>(rawJson, options);

                            ImportUtil.ImportData(
                                DgvSongList,
                                SharedDataSource,
                                importDataSet3,
                                filePath,
                                TBLog,
                                Text);

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
                CustomFunction.WriteLog(TBLog, "請手動重新整理播放清單檔案列表。");
            }
        });
    }

    /// <summary>
    /// 執行載入播放清單檔案
    /// </summary>
    private void DoLoadPlaylist()
    {
        OpenFileDialog openFileDialog = new()
        {
            Title = "請選擇播放清單檔案",
            Filter = "時間標記播放清單檔案、秒數播放清單檔案（*.json）|*.json|JSON 檔案（含備註）|*.jsonc|時間標記文字檔案（*.txt）|*.txt",
            FilterIndex = 1,
            InitialDirectory = VariableSet.PlaylistsFolderPath
        };

        DialogResult dialogResult = openFileDialog.ShowDialog();

        if (dialogResult == DialogResult.OK)
        {
            string filePath = openFileDialog.FileName;

            if (!string.IsNullOrEmpty(filePath))
            {
                string rawJson = File.ReadAllText(filePath);

                JsonSerializerOptions options = new()
                {
                    // 忽略掉註解。
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    WriteIndented = true
                };

                switch (openFileDialog.FilterIndex)
                {
                    case 1:
                        List<SongDataTimeStamp>? importDataSet1 = JsonSerializer
                             .Deserialize<List<SongDataTimeStamp>>(rawJson, options);

                        List<SongDataSeconds>? importDataSet2 = JsonSerializer
                            .Deserialize<List<SongDataSeconds>>(rawJson, options);

                        TimeSpan? timeSpan = importDataSet1?.FirstOrDefault()?.StartTime;

                        double? seconds = importDataSet2?.FirstOrDefault()?.StartSeconds;

                        // 當 timeSpan 不為 null 時，則表示讀取到的是時間標記的 *.json。
                        if (timeSpan != null)
                        {
                            ImportUtil.ImportData(
                                DgvSongList,
                                SharedDataSource,
                                importDataSet1,
                                filePath,
                                TBLog,
                                Text);
                        }
                        else
                        {
                            // 當 seconds 為 null 或大於等於 0 時，則表示讀取到的是秒數的 *.json。
                            if (seconds == null || seconds >= 0)
                            {
                                Debug.WriteLine("789");

                                ImportUtil.ImportData(
                                    DgvSongList,
                                    SharedDataSource,
                                    importDataSet2,
                                    filePath,
                                    TBLog,
                                    Text);
                            }
                        }

                        break;
                    case 2:
                        // 支援來自：https://github.com/jim60105/Playlists 的 *.jsonc 檔案。
                        List<List<object>>? importDataSet3 = JsonSerializer
                            .Deserialize<List<List<object>>>(rawJson, options);

                        ImportUtil.ImportData(
                            DgvSongList,
                            SharedDataSource,
                            importDataSet3,
                            filePath,
                            TBLog,
                            Text);

                        break;
                    case 3:
                        ImportUtil.ImportData(
                            DgvSongList,
                            SharedDataSource,
                            filePath,
                            TBLog,
                            Text);

                        break;
                    default:
                        break;
                }
            }
            else
            {
                string rawMsgNotSelectedFile = "請確認是否已選擇檔案。";

                CustomFunction.WriteLog(TBLog, rawMsgNotSelectedFile);

                MessageBox.Show(rawMsgNotSelectedFile, Text);
            }
        }
    }

    /// <summary>
    /// 建立彈出用的 Form
    /// </summary>
    /// <returns>Form</returns>
    private Form CreateVideoPopupForm()
    {
        int originWidth = PPlayerHost.Width,
            originHeight = PPlayerHost.Height;

        BorderStyle originBorderStyle = PPlayerHost.BorderStyle;

        Point originLocation = PPlayerHost.Location;

        string videoName = string.Empty;

        LName.InvokeIfRequired(() =>
        {
            videoName = LName.Text;

            if (videoName == "無")
            {
                videoName = "影片";
            }
        });

        Form form = new()
        {
            Text = $"{videoName} - {StringSet.AppName}",
            Icon = Properties.Resources.app_icon,
            // 來源： https://support.google.com/youtube/answer/6375112?hl=zh-Hant
            // 2022-10-18 大小會有誤差。
            // 720p
            Width = 1280,
            Height = 720,
            MaximizeBox = true,
            // 240p
            MinimumSize = new Size(originWidth, originHeight),
            // 7680p (8K)
            MaximumSize = new Size(7680, 4320)
        };

        form.KeyDown += MainForm_KeyDown;

        form.Load += (object? sender, EventArgs e) =>
        {
            PPlayerHost.InvokeIfRequired(() =>
            {
                PPlayerHost.Parent = form;
                PPlayerHost.Location = new Point(0, 0);
                PPlayerHost.Width = form.Width;
                PPlayerHost.Height = form.Height;
                PPlayerHost.BorderStyle = BorderStyle.None;
                PPlayerHost.Dock = DockStyle.Fill;

                IsSharedVideoPopupFormPopup = true;
            });
        };

        form.FormClosing += (object? sender, FormClosingEventArgs e) =>
        {
            PPlayerHost.InvokeIfRequired(() =>
            {
                PPlayerHost.Parent = this;
                PPlayerHost.Location = originLocation;
                PPlayerHost.Width = originWidth;
                PPlayerHost.Height = originHeight;
                PPlayerHost.BorderStyle = originBorderStyle;
                PPlayerHost.Dock = DockStyle.None;

                IsSharedVideoPopupFormPopup = false;
            });
        };

        form.SizeChanged += (object? sender, EventArgs e) =>
        {
            PPlayerHost.InvokeIfRequired(() =>
            {
                PPlayerHost.Width = form.Width;
                PPlayerHost.Height = form.Height;
            });
        };

        return form;
    }

    /// <summary>
    /// 寫紀錄
    /// </summary>
    /// <param name="message">字串，訊息</param>
    /// <param name="enableDebug">布林值，輸出 Debug，預設值為 false</param>
    /// <param name="enableAutoClear">布林值，自動執行 TextBox.Clear()，預設值為 true</param>
    private async void WriteLog(
        string message,
        bool enableDebug = false,
        bool enableAutoClear = true)
    {
        await Task.Run(() =>
        {
            // 當 message 不為空白或空值時才輸出。
            if (!string.IsNullOrEmpty(message))
            {
                string outputMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

                TBLog.InvokeIfRequired(() =>
                {
                    // 當有相同的內容存在時，則不輸出。
                    if (!TBLog.Text.Contains(outputMessage))
                    {
                        if (enableDebug)
                        {
                            Debug.WriteLine(outputMessage);
                        }

                        outputMessage += Environment.NewLine;

                        if (enableAutoClear)
                        {
                            // 預測文字字串長度。
                            int predictTextLength = TBLog.Text.Length + outputMessage.Length;

                            // 當 predictTextLength 大於或等於 TextBox 的上限值時。
                            if (predictTextLength >= TBLog.MaxLength)
                            {
                                // 清除 TextBox。
                                TBLog.Clear();

                                if (enableDebug)
                                {
                                    Debug.WriteLine("已執行 TextBox.Clear()。");
                                }
                            }
                        }

                        TBLog.AppendText(outputMessage);
                    }
                });
            }
            else
            {
                Debug.WriteLine("變數 message 為空白或是空值。");
            }
        });
    }

    /// <summary>
    /// 檢查應用程式的版本
    /// </summary>
    private async void CheckAppVersion()
    {
        if (SharedHttpClient != null)
        {
            UpdateNotifier.CheckResult checkResult = await UpdateNotifier.CheckVersion(SharedHttpClient);

            if (!string.IsNullOrEmpty(checkResult.MessageText))
            {
                WriteLog(checkResult.MessageText);
            }

            if (checkResult.HasNewVersion &&
                !string.IsNullOrEmpty(checkResult.DownloadUrl))
            {
                DialogResult dialogResult = MessageBox.Show($"您是否要下載新版本 v{checkResult.VersionText}？",
                    Text,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (dialogResult == DialogResult.OK)
                {
                    CustomFunction.OpenUrl(checkResult.DownloadUrl);

                    // 結束應用程式。
                    Application.Exit();
                }
            }

            if (checkResult.NetVersionIsOdler &&
                !string.IsNullOrEmpty(checkResult.DownloadUrl))
            {
                DialogResult dialogResult = MessageBox.Show($"您是否要下載舊版本 v{checkResult.VersionText}？",
                    Text,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question);

                if (dialogResult == DialogResult.OK)
                {
                    CustomFunction.OpenUrl(checkResult.DownloadUrl);

                    // 結束應用程式。
                    Application.Exit();
                }
            }
        }
    }

    /// <summary>
    /// 取得 *.lrc 檔案的網址
    /// </summary>
    /// <param name="videoID">字串，影片的 ID 值</param>
    /// <param name="videoName">字串，影片的名稱</param>
    /// <param name="startSeconds">字串，開始秒數</param>
    /// <returns>字串陣列</returns>
    private string[] GetLrcFileUrl(string videoID, string videoName, string startSeconds)
    {
        string lrcFileUrl = string.Empty, offsetSeconds = "0";

        try
        {
            SharedHttpClient ??= SharedHttpClientFactory?.CreateClient();

            if (SharedHttpClient != null)
            {
                string rawJson = SharedHttpClient
                    .GetStringAsync(VariableSet.YouTubeClipPlaylistLyricsJsonUrl).Result;

                if (!string.IsNullOrEmpty(rawJson))
                {
                    JsonSerializerOptions options = new()
                    {
                        // 忽略掉註解。
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        WriteIndented = true
                    };

                    List<List<object>>? dataSet = JsonSerializer
                        .Deserialize<List<List<object>>>(rawJson, options);

                    if (dataSet != null)
                    {
                        List<object>? lyricData = dataSet.FirstOrDefault(n => n[0].ToString() == videoID &&
                            n[1].ToString() == startSeconds);

                        if (lyricData != null)
                        {
                            if (int.TryParse(lyricData[2].ToString(), out int songID))
                            {
                                if (songID > 0)
                                {
                                    WriteLog($"SongId：{songID}");

                                    lrcFileUrl = VariableSet.YouTubeClipPlaylistLrcFileTemplateUrl
                                        .Replace("[SongId]", songID.ToString());

                                    WriteLog($"已找到 [{videoID}]「{videoName}」可用的 *.lrc 檔案，網址：{lrcFileUrl}");

                                    string tempOffsetSecnds = lyricData[4].ToString() ?? "0";

                                    if (int.TryParse(startSeconds, out int iStartSecnds) &&
                                        int.TryParse(tempOffsetSecnds, out int iOffsetSecnds))
                                    {
                                        WriteLog($"開始秒數：{iStartSecnds}");
                                        WriteLog($"*.lrc 檔案的偏移秒數：{iOffsetSecnds}");

                                        offsetSeconds = (iStartSecnds + iOffsetSecnds).ToString();
                                    }
                                    else
                                    {
                                        offsetSeconds = "0";
                                    }

                                    WriteLog($"*.lrc 檔案的實際偏移秒數：{offsetSeconds}");
                                }
                                else if (songID == 0)
                                {
                                    WriteLog($"[{videoID}]「{videoName}」已被手動禁用歌詞搜尋功能。");
                                }
                                else if (songID == -1)
                                {
                                    WriteLog($"[{videoID}]「{videoName}」歌曲搜尋失敗。");
                                }
                                else
                                {
                                    // result < -1。
                                    WriteLog($"[{videoID}]「{videoName}」有找到歌曲但歌詞搜尋失敗，記錄為 -SongId（{songID}）。");
                                }
                            }
                        }
                        else
                        {
                            WriteLog($"[{videoID}]「{videoName}」找不到對應的 *.lrc 資訊。");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }

        return new string[]
        {
            lrcFileUrl,
            offsetSeconds
        };
    }

    /// <summary>
    /// 初始化 Discord 豐富狀態
    /// </summary>
    public void InitDCRichPresence()
    {
        try
        {
            // 從 App.config 讀取設定。
            bool logVerbose = Properties.Settings.Default.EnableLogVerbose;

            SharedDiscordRpcClient ??= new(Properties.Settings.Default.DiscordApplicationID)
            {
                Logger = new ConsoleLogger { Level = logVerbose ? LogLevel.Trace : LogLevel.Warning }
            };

            SharedDiscordRpcClient.OnConnectionEstablished += (object sender, ConnectionEstablishedMessage e) =>
            {
                WriteLog($"[{e.Type}]：已連接的管道：{e.ConnectedPipe}");
            };

            SharedDiscordRpcClient.OnConnectionFailed += (object sender, ConnectionFailedMessage e) =>
            {
                ConnectionFailedCount++;

                WriteLog($"[{e.Type}]：已失敗的管道：{e.FailedPipe}");

                if (ConnectionFailedCount == Properties.Settings.Default.MaxConnectionFailedCount)
                {
                    DisposeDCRichPresence();

                    WriteLog($"Discord 豐富狀態，連線失敗已達 {ConnectionFailedCount} 次，已暫時關閉此功能。");

                    ConnectionFailedCount = 0;
                }
            };

            SharedDiscordRpcClient.OnReady += (object sender, ReadyMessage e) =>
            {
                WriteLog($"[{e.Type}]：已接收到使用者 {e.User.Username} 的準備完成。");
            };

            SharedDiscordRpcClient.OnPresenceUpdate += (object sender, PresenceMessage e) =>
            {
                WriteLog($"[{e.Type}][{e.Name}（{e.ApplicationID}）]：（{e.Presence.State}）{e.Presence.Details}");
            };

            SharedDiscordRpcClient.OnError += (object sender, ErrorMessage e) =>
            {
                WriteLog($"[{e.Type}][{e.Code}]：{e.Message}");
            };

            SharedDiscordRpcClient.OnClose += (object sender, CloseMessage e) =>
            {
                WriteLog($"[{e.Type}][{e.Code}]：{e.Reason}");
            };

            SharedDiscordRpcClient.Initialize();

            SetDCRichPresence(
                state: StringSet.StateStop,
                assets: AssetsSet.AssetsStop);

            WriteLog("已啟用 Discord 豐富狀態。");
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    /// <summary>
    /// Dispose 掉 Discord 豐富狀態
    /// </summary>
    public void DisposeDCRichPresence()
    {
        SharedDiscordRpcClient?.ClearPresence();
        SharedDiscordRpcClient?.Dispose();
        SharedDiscordRpcClient = null;

        WriteLog("已停用 Discord 豐富狀態。");
    }

    /// <summary>
    /// 設定 Discord 的豐富狀態
    /// </summary>
    /// <param name="details">字串，細節，預設值為空白</param>
    /// <param name="state">字串，狀態，預設值為空白</param>
    /// <param name="timestamps">Timestamps，預設值為 null</param>
    /// <param name="assets">Assets，預設值為 null</param>
    private void SetDCRichPresence(
        string details = "",
        string state = "",
        Timestamps? timestamps = null,
        Assets? assets = null)
    {
        try
        {
            if (!string.IsNullOrEmpty(details) ||
                !string.IsNullOrEmpty(state) ||
                assets != null)
            {
                SharedDiscordRpcClient?.SetPresence(new()
                {
                    Details = details,
                    State = state,
                    Timestamps = timestamps,
                    Assets = assets
                });
            }
            else
            {
                SharedDiscordRpcClient?.SetPresence(null);
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }
}