namespace CustomYTPlayer;

partial class MoreFunctionForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.LLPlaylistsFolder = new System.Windows.Forms.LinkLabel();
            this.LLConfigFolder = new System.Windows.Forms.LinkLabel();
            this.LLlibFolder = new System.Windows.Forms.LinkLabel();
            this.LLYtHookLua = new System.Windows.Forms.LinkLabel();
            this.LLYtDlp = new System.Windows.Forms.LinkLabel();
            this.LLlibmpv = new System.Windows.Forms.LinkLabel();
            this.LLYoutubeClipPlaylistPlaylists = new System.Windows.Forms.LinkLabel();
            this.LLYoutubeClipPlaylistLyrics = new System.Windows.Forms.LinkLabel();
            this.LFadeirisCustomPlaylist = new System.Windows.Forms.LinkLabel();
            this.BtnForceUpdateDependency = new System.Windows.Forms.Button();
            this.CBDiscordRichPresence = new System.Windows.Forms.CheckBox();
            this.CBLogVerbose = new System.Windows.Forms.CheckBox();
            this.GBOpenFolder = new System.Windows.Forms.GroupBox();
            this.GBDependency = new System.Windows.Forms.GroupBox();
            this.GBNetSource = new System.Windows.Forms.GroupBox();
            this.GBOpenFolder.SuspendLayout();
            this.GBDependency.SuspendLayout();
            this.GBNetSource.SuspendLayout();
            this.SuspendLayout();
            // 
            // LLPlaylistsFolder
            // 
            this.LLPlaylistsFolder.AutoSize = true;
            this.LLPlaylistsFolder.Location = new System.Drawing.Point(6, 49);
            this.LLPlaylistsFolder.Name = "LLPlaylistsFolder";
            this.LLPlaylistsFolder.Size = new System.Drawing.Size(91, 15);
            this.LLPlaylistsFolder.TabIndex = 3;
            this.LLPlaylistsFolder.TabStop = true;
            this.LLPlaylistsFolder.Text = "播放清單資料夾";
            this.LLPlaylistsFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLPlaylistsFolder_LinkClicked);
            // 
            // LLConfigFolder
            // 
            this.LLConfigFolder.AutoSize = true;
            this.LLConfigFolder.Location = new System.Drawing.Point(6, 34);
            this.LLConfigFolder.Name = "LLConfigFolder";
            this.LLConfigFolder.Size = new System.Drawing.Size(79, 15);
            this.LLConfigFolder.TabIndex = 2;
            this.LLConfigFolder.TabStop = true;
            this.LLConfigFolder.Text = "設定檔資料夾";
            this.LLConfigFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLConfigFolder_LinkClicked);
            // 
            // LLlibFolder
            // 
            this.LLlibFolder.AutoSize = true;
            this.LLlibFolder.Location = new System.Drawing.Point(6, 19);
            this.LLlibFolder.Name = "LLlibFolder";
            this.LLlibFolder.Size = new System.Drawing.Size(60, 15);
            this.LLlibFolder.TabIndex = 1;
            this.LLlibFolder.TabStop = true;
            this.LLlibFolder.Text = "lib 資料夾";
            this.LLlibFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLlibFolder_LinkClicked);
            // 
            // LLYtHookLua
            // 
            this.LLYtHookLua.AutoSize = true;
            this.LLYtHookLua.Location = new System.Drawing.Point(6, 34);
            this.LLYtHookLua.Name = "LLYtHookLua";
            this.LLYtHookLua.Size = new System.Drawing.Size(82, 15);
            this.LLYtHookLua.TabIndex = 2;
            this.LLYtHookLua.TabStop = true;
            this.LLYtHookLua.Text = "ytdl_hook.lua";
            this.LLYtHookLua.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLYtHookLua_LinkClicked);
            // 
            // LLYtDlp
            // 
            this.LLYtDlp.AutoSize = true;
            this.LLYtDlp.Location = new System.Drawing.Point(6, 49);
            this.LLYtDlp.Name = "LLYtDlp";
            this.LLYtDlp.Size = new System.Drawing.Size(41, 15);
            this.LLYtDlp.TabIndex = 3;
            this.LLYtDlp.TabStop = true;
            this.LLYtDlp.Text = "yt-dlp";
            this.LLYtDlp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLYtDlp_LinkClicked);
            // 
            // LLlibmpv
            // 
            this.LLlibmpv.AutoSize = true;
            this.LLlibmpv.Location = new System.Drawing.Point(6, 19);
            this.LLlibmpv.Name = "LLlibmpv";
            this.LLlibmpv.Size = new System.Drawing.Size(46, 15);
            this.LLlibmpv.TabIndex = 1;
            this.LLlibmpv.TabStop = true;
            this.LLlibmpv.Text = "libmpv";
            this.LLlibmpv.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLlibmpv_LinkClicked);
            // 
            // LLYoutubeClipPlaylistPlaylists
            // 
            this.LLYoutubeClipPlaylistPlaylists.AutoSize = true;
            this.LLYoutubeClipPlaylistPlaylists.Location = new System.Drawing.Point(6, 19);
            this.LLYoutubeClipPlaylistPlaylists.Name = "LLYoutubeClipPlaylistPlaylists";
            this.LLYoutubeClipPlaylistPlaylists.Size = new System.Drawing.Size(163, 15);
            this.LLYoutubeClipPlaylistPlaylists.TabIndex = 1;
            this.LLYoutubeClipPlaylistPlaylists.TabStop = true;
            this.LLYoutubeClipPlaylistPlaylists.Text = "YoutubeClipPlaylist/Playlists";
            this.LLYoutubeClipPlaylistPlaylists.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLYoutubeClipPlaylistPlaylists_LinkClicked);
            // 
            // LLYoutubeClipPlaylistLyrics
            // 
            this.LLYoutubeClipPlaylistLyrics.AutoSize = true;
            this.LLYoutubeClipPlaylistLyrics.Location = new System.Drawing.Point(6, 34);
            this.LLYoutubeClipPlaylistLyrics.Name = "LLYoutubeClipPlaylistLyrics";
            this.LLYoutubeClipPlaylistLyrics.Size = new System.Drawing.Size(150, 15);
            this.LLYoutubeClipPlaylistLyrics.TabIndex = 2;
            this.LLYoutubeClipPlaylistLyrics.TabStop = true;
            this.LLYoutubeClipPlaylistLyrics.Text = "YoutubeClipPlaylist/Lyrics";
            this.LLYoutubeClipPlaylistLyrics.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LLYoutubeClipPlaylistLyrics_LinkClicked);
            // 
            // LFadeirisCustomPlaylist
            // 
            this.LFadeirisCustomPlaylist.AutoSize = true;
            this.LFadeirisCustomPlaylist.Location = new System.Drawing.Point(6, 49);
            this.LFadeirisCustomPlaylist.Name = "LFadeirisCustomPlaylist";
            this.LFadeirisCustomPlaylist.Size = new System.Drawing.Size(134, 15);
            this.LFadeirisCustomPlaylist.TabIndex = 3;
            this.LFadeirisCustomPlaylist.TabStop = true;
            this.LFadeirisCustomPlaylist.Text = "fadeiris/CustomPlaylist";
            this.LFadeirisCustomPlaylist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LFadeirisCustomPlaylist_LinkClicked);
            // 
            // BtnForceUpdateDependency
            // 
            this.BtnForceUpdateDependency.Location = new System.Drawing.Point(262, 12);
            this.BtnForceUpdateDependency.Name = "BtnForceUpdateDependency";
            this.BtnForceUpdateDependency.Size = new System.Drawing.Size(130, 23);
            this.BtnForceUpdateDependency.TabIndex = 4;
            this.BtnForceUpdateDependency.Text = "強制更新相依性檔案";
            this.BtnForceUpdateDependency.UseVisualStyleBackColor = true;
            this.BtnForceUpdateDependency.Click += new System.EventHandler(this.BtnForceUpdateDependency_Click);
            // 
            // CBDiscordRichPresence
            // 
            this.CBDiscordRichPresence.AutoSize = true;
            this.CBDiscordRichPresence.Location = new System.Drawing.Point(262, 41);
            this.CBDiscordRichPresence.Name = "CBDiscordRichPresence";
            this.CBDiscordRichPresence.Size = new System.Drawing.Size(120, 19);
            this.CBDiscordRichPresence.TabIndex = 5;
            this.CBDiscordRichPresence.Text = "Discord 豐富狀態";
            this.CBDiscordRichPresence.UseVisualStyleBackColor = true;
            this.CBDiscordRichPresence.CheckedChanged += new System.EventHandler(this.CBDiscordRichPresence_CheckedChanged);
            // 
            // CBLogVerbose
            // 
            this.CBLogVerbose.AutoSize = true;
            this.CBLogVerbose.Location = new System.Drawing.Point(262, 66);
            this.CBLogVerbose.Name = "CBLogVerbose";
            this.CBLogVerbose.Size = new System.Drawing.Size(100, 19);
            this.CBLogVerbose.TabIndex = 6;
            this.CBLogVerbose.Text = "記錄 Verbose";
            this.CBLogVerbose.UseVisualStyleBackColor = true;
            this.CBLogVerbose.CheckedChanged += new System.EventHandler(this.CBLogVerbose_CheckedChanged);
            // 
            // GBOpenFolder
            // 
            this.GBOpenFolder.Controls.Add(this.LLPlaylistsFolder);
            this.GBOpenFolder.Controls.Add(this.LLlibFolder);
            this.GBOpenFolder.Controls.Add(this.LLConfigFolder);
            this.GBOpenFolder.Location = new System.Drawing.Point(12, 12);
            this.GBOpenFolder.Name = "GBOpenFolder";
            this.GBOpenFolder.Size = new System.Drawing.Size(200, 100);
            this.GBOpenFolder.TabIndex = 1;
            this.GBOpenFolder.TabStop = false;
            this.GBOpenFolder.Text = "開啟資料夾";
            // 
            // GBDependency
            // 
            this.GBDependency.Controls.Add(this.LLlibmpv);
            this.GBDependency.Controls.Add(this.LLYtDlp);
            this.GBDependency.Controls.Add(this.LLYtHookLua);
            this.GBDependency.Location = new System.Drawing.Point(12, 118);
            this.GBDependency.Name = "GBDependency";
            this.GBDependency.Size = new System.Drawing.Size(200, 100);
            this.GBDependency.TabIndex = 2;
            this.GBDependency.TabStop = false;
            this.GBDependency.Text = "相依性檔案";
            // 
            // GBNetSource
            // 
            this.GBNetSource.Controls.Add(this.LLYoutubeClipPlaylistPlaylists);
            this.GBNetSource.Controls.Add(this.LLYoutubeClipPlaylistLyrics);
            this.GBNetSource.Controls.Add(this.LFadeirisCustomPlaylist);
            this.GBNetSource.Location = new System.Drawing.Point(12, 224);
            this.GBNetSource.Name = "GBNetSource";
            this.GBNetSource.Size = new System.Drawing.Size(200, 100);
            this.GBNetSource.TabIndex = 3;
            this.GBNetSource.TabStop = false;
            this.GBNetSource.Text = "資料來源";
            // 
            // MoreFunctionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 341);
            this.Controls.Add(this.GBNetSource);
            this.Controls.Add(this.GBDependency);
            this.Controls.Add(this.GBOpenFolder);
            this.Controls.Add(this.CBLogVerbose);
            this.Controls.Add(this.CBDiscordRichPresence);
            this.Controls.Add(this.BtnForceUpdateDependency);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MoreFunctionForm";
            this.Text = "MoreFunctionForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MoreFunctionForm_FormClosing);
            this.Load += new System.EventHandler(this.MoreFunctionForm_Load);
            this.GBOpenFolder.ResumeLayout(false);
            this.GBOpenFolder.PerformLayout();
            this.GBDependency.ResumeLayout(false);
            this.GBDependency.PerformLayout();
            this.GBNetSource.ResumeLayout(false);
            this.GBNetSource.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private LinkLabel LLPlaylistsFolder;
    private LinkLabel LLConfigFolder;
    private LinkLabel LLlibFolder;
    private LinkLabel LLYtHookLua;
    private LinkLabel LLYtDlp;
    private LinkLabel LLlibmpv;
    private LinkLabel LLYoutubeClipPlaylistPlaylists;
    private LinkLabel LLYoutubeClipPlaylistLyrics;
    private LinkLabel LFadeirisCustomPlaylist;
    private Button BtnForceUpdateDependency;
    private CheckBox CBDiscordRichPresence;
    private CheckBox CBLogVerbose;
    private GroupBox GBOpenFolder;
    private GroupBox GBDependency;
    private GroupBox GBNetSource;
}