using CustomYTPlayer.Common;
using CustomYTPlayer.Common.Sets;
using CustomYTPlayer.Extensions;
using DiscordRPC.Logging;
using Mpv.NET.API;
using System.Configuration;
using System.Diagnostics;

namespace CustomYTPlayer;

public partial class MoreFunctionForm : Form
{
    public MoreFunctionForm(MainForm control)
    {
        InitializeComponent();

        mainForm = control;

        TBLog = mainForm.Controls
            .OfType<TextBox>()
            .FirstOrDefault(n => n.Name == "TBLog")!;
        BtnMoreFunction = mainForm.Controls
            .OfType<Button>()
            .FirstOrDefault(n => n.Name == "BtnMoreFunction")!;
    }

    private void MoreFunctionForm_Load(object sender, EventArgs e)
    {
        try
        {
            IsInitializing = true;
            
            CustomInit();
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
        finally
        {
            IsInitializing = false;
        }
    }

    private void MoreFunctionForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            BtnMoreFunction.Enabled = true;
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LLlibFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            // 來源：https://stackoverflow.com/a/1132559
            Process.Start(new ProcessStartInfo()
            {
                FileName = VariableSet.ExecPath,
                UseShellExecute = true,
                Verb = "Open"
            });
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LLConfigFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            // 來源：https://stackoverflow.com/a/7069366
            string configFilePath = ConfigurationManager
                .OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath,
                fileName = Path.GetFileName(configFilePath),
                folderPath = Path.GetFullPath(configFilePath).Replace(fileName, string.Empty);

            if (Directory.Exists(folderPath))
            {
                // 來源：https://stackoverflow.com/a/1132559
                Process.Start(new ProcessStartInfo()
                {
                    FileName = folderPath,
                    UseShellExecute = true,
                    Verb = "Open"
                });
            }
            else
            {
                CustomFunction.WriteLog(TBLog, "設定檔資料夾尚未存在，請先調整選項後再使用此功能。");
            }
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LLPlaylistsFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            // 來源：https://stackoverflow.com/a/1132559
            Process.Start(new ProcessStartInfo()
            {
                FileName = VariableSet.PlaylistsFolderPath,
                UseShellExecute = true,
                Verb = "Open"
            });
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LLlibmpv_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://sourceforge.net/projects/mpv-player-windows/files/libmpv/");
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LLYtHookLua_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://github.com/mpv-player/mpv/blob/master/player/lua/ytdl_hook.lua");
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LLYtDlp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://github.com/yt-dlp/yt-dlp");
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LLYoutubeClipPlaylistPlaylists_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://github.com/YoutubeClipPlaylist/Playlists");
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LLYoutubeClipPlaylistLyrics_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://github.com/YoutubeClipPlaylist/Lyrics");
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void LFadeirisCustomPlaylist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            CustomFunction.OpenUrl("https://github.com/fadeiris/CustomPlaylist");
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void BtnForceUpdateDependency_Click(object sender, EventArgs e)
    {
        try
        {
            mainForm.IsForceUpdateDependency = true;
            mainForm.BtnUpdateYtDlp_Click(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void CBDiscordRichPresence_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CBDiscordRichPresence.InvokeIfRequired(() =>
            {
                bool value = CBDiscordRichPresence.Checked;

                if (Properties.Settings.Default.EnableDiscordRichPresence != value)
                {
                    Properties.Settings.Default.EnableDiscordRichPresence = value;
                    Properties.Settings.Default.Save();
                }

                if (!IsInitializing)
                {
                    if (value)
                    {
                        mainForm.InitDCRichPresence();
                    }
                    else
                    {
                        mainForm.DisposeDCRichPresence();
                    }
                }
            });
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }

    private void CBLogVerbose_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CBLogVerbose.InvokeIfRequired(() =>
            {
                bool value = CBLogVerbose.Checked;

                if (Properties.Settings.Default.EnableLogVerbose != value)
                {
                    Properties.Settings.Default.EnableLogVerbose = value;
                    Properties.Settings.Default.Save();
                }

                if (mainForm.MpvPlayer != null)
                {
                    mainForm.MpvPlayer.LogLevel = value ? MpvLogLevel.V : MpvLogLevel.Info;
                }

                if (mainForm.SharedDiscordRpcClient != null)
                {
                    mainForm.SharedDiscordRpcClient.Logger.Level = value ? LogLevel.Trace : LogLevel.Warning;
                }
            });
        }
        catch (Exception ex)
        {
            CustomFunction.WriteLog(TBLog, ex.Message);
        }
    }
}