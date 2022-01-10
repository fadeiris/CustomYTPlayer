using CustomYTPlayer.Common.Sets;
using CustomYTPlayer.Extensions;
using Mpv.NET.API;
using Mpv.NET.Player;
using System.Diagnostics;

namespace CustomYTPlayer;

// 阻擋設計工具。
partial class DesignerBlocker { }

public partial class MainForm
{
    private void MpvPlayer_LogMessage(object? sender, MpvLogMessageEventArgs e)
    {
        MpvLogMessage mpvLogMessage = e.Message;

        // 移除尾端的換行字元。
        string rawMsgText = mpvLogMessage.Text.TrimEnd('\r', '\n');

        // 當 rawMsgText 不為空白或空值時，才輸出至 TBLog。
        if (!string.IsNullOrEmpty(rawMsgText))
        {
            string rawLogText = $"[{mpvLogMessage.Level}]" +
                $"[{mpvLogMessage.Prefix}] 訊息：{rawMsgText}";

            WriteLog($"{rawLogText}");
        }
    }

    private void MpvPlayer_MediaEndedBuffering(object? sender, EventArgs e)
    {
        WriteLog(StringSet.MsgMediaEndedBuffering);
    }

    private void MpvPlayer_MediaEndedSeeking(object? sender, EventArgs e)
    {
        WriteLog(StringSet.MsgMediaEndedSeeking);
    }

    private void MpvPlayer_MediaError(object? sender, EventArgs e)
    {
        try
        {
            WriteLog(StringSet.MsgMediaError);

            ShowNotify(Text, StringSet.MsgMediaError);

            SharedCurrentIndex++;

            // 當播放清單內還有項目時，則繼續播放。
            if (SharedCurrentIndex < SharedDataSource.Count)
            {
                DoPlay();
            }
            else
            {
                SharedCurrentIndex--;

                // 停止播放。
                BtnStop_Click(this, null);

                // 重設播放狀態。
                ResetPlayStatus();
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void MpvPlayer_MediaFinished(object? sender, EventArgs e)
    {
        try
        {
            WriteLog(StringSet.MsgMediaFinished);

            ResetPlayStatus();
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void MpvPlayer_MediaLoaded(object? sender, EventArgs e)
    {
        try
        {
            WriteLog(StringSet.MsgMediaLoaded);

            // 當 SharedEndTime 等於 00:00:00 時。
            if (SharedEndTime == TimeSpan.FromSeconds(0))
            {
                // 當結束時間為 00:00:00 時，
                // 用 libmpv 取得的 Duration 回設
                // SharedEndTime，並更新相關的控制項。
                if (MpvPlayer?.Duration != null)
                {
                    SharedEndTime = MpvPlayer?.Duration.StripMilliseconds();

                    // 將結束時間回寫到播放清單。
                    DataGridViewRow currentRow = DgvSongList.Rows[SharedCurrentIndex];

                    if (currentRow != null)
                    {
                        DataGridViewCell targetCell = currentRow.Cells[3];

                        if (targetCell != null)
                        {
                            targetCell.Value = SharedEndTime;
                        }
                    }

                    if (SharedStartTime.HasValue)
                    {
                        SharedTargetDuration = SharedEndTime?.Subtract(SharedStartTime.Value);
                    }

                    int maxVal = Convert.ToInt32(SharedEndTime?.TotalSeconds);

                    PBProgress.InvokeIfRequired(() =>
                    {
                        PBProgress.Maximum = maxVal;
                        PBProgress.Minimum = 0;
                    });

                    TBSeek.InvokeIfRequired(() =>
                    {
                        // 用於避免 TBSeek 觸發 expression2。（往回拉到開頭）
                        TBSeek.SetRange(VariableSet.TBSeekMinimum, maxVal);
                    });
                }
            }

            if (SharedStartTime.HasValue)
            {
                MpvPlayer?.SeekAsync(SharedStartTime.Value.TotalSeconds);
            }
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void MpvPlayer_MediaPaused(object? sender, EventArgs e)
    {
        WriteLog(StringSet.MsgMediaPaused);

        IsPlaying = false;
    }

    private void MpvPlayer_MediaResumed(object? sender, EventArgs e)
    {
        WriteLog(StringSet.MsgMediaResumed);

        IsPlaying = true;
    }

    private void MpvPlayer_MediaStartedBuffering(object? sender, EventArgs e)
    {
        WriteLog(StringSet.MsgMediaStartedBuffering);
    }

    private void MpvPlayer_MediaStartedSeeking(object? sender, EventArgs e)
    {
        WriteLog(StringSet.MsgMediaStartedSeeking);
    }

    private void MpvPlayer_MediaUnloaded(object? sender, EventArgs e)
    {
        try
        {
            WriteLog(StringSet.MsgMediaUnloaded);

            ResetPlayStatus();
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }

    private void MpvPlayer_PositionChanged(object? sender, MpvPlayerPositionChangedEventArgs e)
    {
        try
        {
            int currentSeconds = Convert.ToInt32(e.NewPosition.TotalSeconds),
                targetSeconds = Convert.ToInt32(SharedEndTime?.TotalSeconds);

            #region 更新 UI

            // 當 currentSeconds 不為 0 時才更新控制項的值。
            if (currentSeconds != 0)
            {
                PBProgress.InvokeIfRequired(() =>
                {
                    // 避免超出上下限制而發生例外。
                    if (currentSeconds >= PBProgress.Minimum &&
                        currentSeconds <= PBProgress.Maximum)
                    {
                        PBProgress.Value = currentSeconds;
                    }
                });

                TBSeek.InvokeIfRequired(() =>
                {
                    // 當 TBSeek 沒有焦點時才允許更新。
                    if (!TBSeek.Focused)
                    {
                        // 避免超出上下限制而發生例外。
                        if (currentSeconds >= TBSeek.Minimum &&
                            currentSeconds <= TBSeek.Maximum)
                        {
                            TBSeek.Value = currentSeconds;
                        }
                    }
                });
            }

            if (SharedStartTime.HasValue)
            {
                TimeSpan currentTimespan = e.NewPosition.Subtract(SharedStartTime.Value);

                // TotalSeconds 大於 0 時才更新。
                if (currentTimespan.TotalSeconds > 0)
                {
                    string durationText = $"{currentTimespan:hh\\:mm\\:ss} / " +
                        $"{SharedTargetDuration:hh\\:mm\\:ss}";

                    LDuration.InvokeIfRequired(() =>
                    {
                        LDuration.Text = durationText;
                    });
                }
            }

            #endregion

            #region 判斷停止播放或自動播放下一個項目

            // 當 SharedEndTime 不為 00:00:00 時才更新。 
            if (SharedEndTime != TimeSpan.FromSeconds(0))
            {
                /**
                 * 說明
                 * 
                 * 1. expression1: 當影片自然的結束播放或是被停止時。
                 * 2. expression2: 操控 TBSeek 控制播放時間時。（往回拉到開頭、往後拉到結尾）[^1][^3]
                 * 3. expression3: 在一個長時間的影片中，控制影片結束播放。[^2]
                 * 
                 * [^1] 因目前是在此（Player_PositionChanged()）事件中控制影片的自動播放下一個項目，
                 *      故而無法判斷目前到底是快進還是快退的狀態，因此需要透過其他方式，來避免操控 TBSeek
                 *      往回拉到開頭造成直接自動播放下一個項目的狀況發生。
                 * 
                 * [^2] 觀察到 e.NewPosition 透過 TBSeek 控制時會有 -1 秒誤差的狀況，可能需要特別處理。
                 *      當 currentSeconds 大於等於 targetSeconds 時即停止播放。
                 * 
                 * [^3] expression2 會有多種條件會使其為 true，需要特別注意。
                 */

                bool expression1 = currentSeconds == 0 && targetSeconds == 0 && PreviousCurrentSeconds > 0,
                    expression2 = currentSeconds == 0 && targetSeconds > 0 && PreviousCurrentSeconds > 0,
                    expression3 = currentSeconds != 0 && currentSeconds - targetSeconds >= 0;

                if (expression1 || expression2 || expression3)
                {
                    // 檢測判斷邏輯用。
                    // 從 App.config 讀取設定。
                    bool logVerbose = Properties.Settings.Default.EnableLogVerbose;

                    if (logVerbose)
                    {
                        WriteLog($"exp1: {expression1}, exp2: {expression2}, exp3: {expression3}");
                        WriteLog($"css: {currentSeconds}, tss: {targetSeconds}, pcs: {PreviousCurrentSeconds}");
                    }
                    else
                    {
                        Debug.WriteLine($"exp1: {expression1}, exp2: {expression2}, exp3: {expression3}");
                        Debug.WriteLine($"css: {currentSeconds}, tss: {targetSeconds}, pcs: {PreviousCurrentSeconds}");
                    }

                    // 2022-10-14
                    // 重設 PreviousCurrentSeconds，
                    // 使其無法滿足 MpvPlayer_PositionChanged()
                    // 事件內 expression2 的條件。
                    PreviousCurrentSeconds = -1;

                    MpvPlayer?.Stop();

                    SharedCurrentIndex++;

                    if (SharedCurrentIndex < SharedDataSource.Count)
                    {
                        DoPlay();
                    }
                    else
                    {
                        SharedCurrentIndex--;

                        BtnStop_Click(this, null);

                        // 延後執行，避免欲與 BtnStop_Click() 內的 ShowNotify() 發生衝突。
                        Task.Delay(700).ContinueWith(task =>
                        {
                            WriteLog(StringSet.MsgPlaylistFinished);

                            ShowNotify(Text, StringSet.MsgPlaylistFinished);
                        });
                    }
                }
                else
                {
                    // 記錄 currentSeconds。
                    PreviousCurrentSeconds = currentSeconds;
                }
            }

            #endregion
        }
        catch (Exception ex)
        {
            WriteLog(ex.Message);
        }
    }
}