namespace CustomYTPlayer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PPlayerHost = new System.Windows.Forms.Panel();
            this.BtnPlay = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.TBLog = new System.Windows.Forms.TextBox();
            this.PBProgress = new System.Windows.Forms.ProgressBar();
            this.TBSeek = new System.Windows.Forms.TrackBar();
            this.BtnClearPlaylist = new System.Windows.Forms.Button();
            this.LDuration = new System.Windows.Forms.Label();
            this.TBVolume = new System.Windows.Forms.TrackBar();
            this.LVolume = new System.Windows.Forms.Label();
            this.BtnPause = new System.Windows.Forms.Button();
            this.DgvSongList = new System.Windows.Forms.DataGridView();
            this.VideoID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SubSrc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnNext = new System.Windows.Forms.Button();
            this.BtnPrevious = new System.Windows.Forms.Button();
            this.LName = new System.Windows.Forms.Label();
            this.BtnClearLog = new System.Windows.Forms.Button();
            this.BtnLoadPlaylist = new System.Windows.Forms.Button();
            this.BtnSavePlaylist = new System.Windows.Forms.Button();
            this.NUDSpeed = new System.Windows.Forms.NumericUpDown();
            this.CBQuality = new System.Windows.Forms.ComboBox();
            this.LSpeed = new System.Windows.Forms.Label();
            this.BtnMute = new System.Windows.Forms.Button();
            this.BtnRandomPlay = new System.Windows.Forms.Button();
            this.BtnRandomPlaylist = new System.Windows.Forms.Button();
            this.BtnExportLog = new System.Windows.Forms.Button();
            this.CBNotShowVideo = new System.Windows.Forms.CheckBox();
            this.LVersion = new System.Windows.Forms.Label();
            this.LYtDlpVersion = new System.Windows.Forms.Label();
            this.BtnUpdateYtDlp = new System.Windows.Forms.Button();
            this.CBNetPlaylists = new System.Windows.Forms.ComboBox();
            this.BtnRefreshNetPlaylists = new System.Windows.Forms.Button();
            this.CBAutoLyric = new System.Windows.Forms.CheckBox();
            this.BtnMoreFunction = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TBSeek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSongList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // PPlayerHost
            // 
            this.PPlayerHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PPlayerHost.Location = new System.Drawing.Point(12, 42);
            this.PPlayerHost.Name = "PPlayerHost";
            this.PPlayerHost.Size = new System.Drawing.Size(426, 240);
            this.PPlayerHost.TabIndex = 12;
            // 
            // BtnPlay
            // 
            this.BtnPlay.Location = new System.Drawing.Point(12, 10);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.Size = new System.Drawing.Size(75, 23);
            this.BtnPlay.TabIndex = 1;
            this.BtnPlay.Text = "播放";
            this.BtnPlay.UseVisualStyleBackColor = true;
            this.BtnPlay.Click += new System.EventHandler(this.BtnPlay_Click);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(359, 9);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 5;
            this.BtnStop.Text = "停止播放";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // TBLog
            // 
            this.TBLog.Location = new System.Drawing.Point(12, 399);
            this.TBLog.Multiline = true;
            this.TBLog.Name = "TBLog";
            this.TBLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TBLog.Size = new System.Drawing.Size(426, 158);
            this.TBLog.TabIndex = 24;
            // 
            // PBProgress
            // 
            this.PBProgress.Location = new System.Drawing.Point(12, 302);
            this.PBProgress.Name = "PBProgress";
            this.PBProgress.Size = new System.Drawing.Size(307, 12);
            this.PBProgress.TabIndex = 14;
            // 
            // TBSeek
            // 
            this.TBSeek.Location = new System.Drawing.Point(12, 320);
            this.TBSeek.Name = "TBSeek";
            this.TBSeek.Size = new System.Drawing.Size(307, 45);
            this.TBSeek.TabIndex = 16;
            this.TBSeek.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // BtnClearPlaylist
            // 
            this.BtnClearPlaylist.Location = new System.Drawing.Point(794, 39);
            this.BtnClearPlaylist.Name = "BtnClearPlaylist";
            this.BtnClearPlaylist.Size = new System.Drawing.Size(95, 23);
            this.BtnClearPlaylist.TabIndex = 11;
            this.BtnClearPlaylist.Text = "清除播放清單";
            this.BtnClearPlaylist.UseVisualStyleBackColor = true;
            this.BtnClearPlaylist.Click += new System.EventHandler(this.BtnClearPlaylist_Click);
            // 
            // LDuration
            // 
            this.LDuration.AutoSize = true;
            this.LDuration.Location = new System.Drawing.Point(324, 299);
            this.LDuration.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LDuration.Name = "LDuration";
            this.LDuration.Size = new System.Drawing.Size(114, 15);
            this.LDuration.TabIndex = 15;
            this.LDuration.Text = "00:00:00 / 00:00:00";
            // 
            // TBVolume
            // 
            this.TBVolume.Location = new System.Drawing.Point(325, 319);
            this.TBVolume.Margin = new System.Windows.Forms.Padding(2);
            this.TBVolume.Maximum = 100;
            this.TBVolume.Name = "TBVolume";
            this.TBVolume.Size = new System.Drawing.Size(82, 45);
            this.TBVolume.TabIndex = 17;
            // 
            // LVolume
            // 
            this.LVolume.AutoSize = true;
            this.LVolume.Location = new System.Drawing.Point(410, 330);
            this.LVolume.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LVolume.Name = "LVolume";
            this.LVolume.Size = new System.Drawing.Size(28, 15);
            this.LVolume.TabIndex = 18;
            this.LVolume.Text = "100";
            // 
            // BtnPause
            // 
            this.BtnPause.Location = new System.Drawing.Point(92, 9);
            this.BtnPause.Margin = new System.Windows.Forms.Padding(2);
            this.BtnPause.Name = "BtnPause";
            this.BtnPause.Size = new System.Drawing.Size(100, 23);
            this.BtnPause.TabIndex = 2;
            this.BtnPause.Text = "暫停 / 恢復播放";
            this.BtnPause.UseVisualStyleBackColor = true;
            this.BtnPause.Click += new System.EventHandler(this.BtnPause_Click);
            // 
            // DgvSongList
            // 
            this.DgvSongList.AllowDrop = true;
            this.DgvSongList.AllowUserToOrderColumns = true;
            this.DgvSongList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DgvSongList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DgvSongList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvSongList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VideoID,
            this.SongName,
            this.StartTime,
            this.EndTime,
            this.SubSrc});
            this.DgvSongList.Location = new System.Drawing.Point(443, 67);
            this.DgvSongList.Margin = new System.Windows.Forms.Padding(2);
            this.DgvSongList.Name = "DgvSongList";
            this.DgvSongList.RowHeadersWidth = 51;
            this.DgvSongList.RowTemplate.Height = 29;
            this.DgvSongList.Size = new System.Drawing.Size(446, 490);
            this.DgvSongList.TabIndex = 29;
            this.DgvSongList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSongList_CellDoubleClick);
            this.DgvSongList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DgvSongList_CellFormatting);
            this.DgvSongList.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvSongList_CellValidated);
            this.DgvSongList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DgvSongList_CellValidating);
            this.DgvSongList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DgvSongList_DataError);
            this.DgvSongList.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.DgvSongList_DefaultValuesNeeded);
            this.DgvSongList.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.DgvSongList_UserDeletedRow);
            this.DgvSongList.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.DgvSongList_UserDeletingRow);
            this.DgvSongList.DragDrop += new System.Windows.Forms.DragEventHandler(this.DgvSongList_DragDrop);
            this.DgvSongList.DragEnter += new System.Windows.Forms.DragEventHandler(this.DgvSongList_DragEnter);
            // 
            // VideoID
            // 
            this.VideoID.DataPropertyName = "VideoID";
            this.VideoID.HeaderText = "影片 ID";
            this.VideoID.MinimumWidth = 6;
            this.VideoID.Name = "VideoID";
            this.VideoID.ToolTipText = "影片的 ID 值或連結網址";
            // 
            // SongName
            // 
            this.SongName.DataPropertyName = "Name";
            this.SongName.HeaderText = "名稱";
            this.SongName.MinimumWidth = 6;
            this.SongName.Name = "SongName";
            this.SongName.ToolTipText = "影片的名稱";
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartTime";
            this.StartTime.HeaderText = "開始時間";
            this.StartTime.MinimumWidth = 6;
            this.StartTime.Name = "StartTime";
            this.StartTime.ToolTipText = "影片的開始時間";
            // 
            // EndTime
            // 
            this.EndTime.DataPropertyName = "EndTime";
            this.EndTime.HeaderText = "結束時間";
            this.EndTime.MinimumWidth = 6;
            this.EndTime.Name = "EndTime";
            this.EndTime.ToolTipText = "影片的結束時間";
            // 
            // SubSrc
            // 
            this.SubSrc.DataPropertyName = "SubSrc";
            this.SubSrc.HeaderText = "字幕檔";
            this.SubSrc.Name = "SubSrc";
            this.SubSrc.ToolTipText = "影片的外部字幕檔案連結網址";
            // 
            // BtnNext
            // 
            this.BtnNext.Location = new System.Drawing.Point(278, 9);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(75, 23);
            this.BtnNext.TabIndex = 4;
            this.BtnNext.Text = "下一個";
            this.BtnNext.UseVisualStyleBackColor = true;
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.Location = new System.Drawing.Point(197, 9);
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.Size = new System.Drawing.Size(75, 23);
            this.BtnPrevious.TabIndex = 3;
            this.BtnPrevious.Text = "上一個";
            this.BtnPrevious.UseVisualStyleBackColor = true;
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // LName
            // 
            this.LName.AutoSize = true;
            this.LName.Location = new System.Drawing.Point(12, 284);
            this.LName.Name = "LName";
            this.LName.Size = new System.Drawing.Size(19, 15);
            this.LName.TabIndex = 13;
            this.LName.Text = "無";
            // 
            // BtnClearLog
            // 
            this.BtnClearLog.Location = new System.Drawing.Point(273, 563);
            this.BtnClearLog.Name = "BtnClearLog";
            this.BtnClearLog.Size = new System.Drawing.Size(75, 23);
            this.BtnClearLog.TabIndex = 27;
            this.BtnClearLog.Text = "清除紀錄";
            this.BtnClearLog.UseVisualStyleBackColor = true;
            this.BtnClearLog.Click += new System.EventHandler(this.BtnClearLog_Click);
            // 
            // BtnLoadPlaylist
            // 
            this.BtnLoadPlaylist.Location = new System.Drawing.Point(443, 39);
            this.BtnLoadPlaylist.Name = "BtnLoadPlaylist";
            this.BtnLoadPlaylist.Size = new System.Drawing.Size(95, 23);
            this.BtnLoadPlaylist.TabIndex = 8;
            this.BtnLoadPlaylist.Text = "載入播放清單";
            this.BtnLoadPlaylist.UseVisualStyleBackColor = true;
            this.BtnLoadPlaylist.Click += new System.EventHandler(this.BtnLoadPlaylist_Click);
            // 
            // BtnSavePlaylist
            // 
            this.BtnSavePlaylist.Location = new System.Drawing.Point(544, 39);
            this.BtnSavePlaylist.Name = "BtnSavePlaylist";
            this.BtnSavePlaylist.Size = new System.Drawing.Size(95, 23);
            this.BtnSavePlaylist.TabIndex = 9;
            this.BtnSavePlaylist.Text = "儲存播放清單";
            this.BtnSavePlaylist.UseVisualStyleBackColor = true;
            this.BtnSavePlaylist.Click += new System.EventHandler(this.BtnSavePlaylist_Click);
            // 
            // NUDSpeed
            // 
            this.NUDSpeed.DecimalPlaces = 2;
            this.NUDSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NUDSpeed.Location = new System.Drawing.Point(190, 372);
            this.NUDSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NUDSpeed.Name = "NUDSpeed";
            this.NUDSpeed.Size = new System.Drawing.Size(60, 23);
            this.NUDSpeed.TabIndex = 21;
            this.NUDSpeed.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUDSpeed.ValueChanged += new System.EventHandler(this.NUDSpeed_ValueChanged);
            // 
            // CBQuality
            // 
            this.CBQuality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBQuality.FormattingEnabled = true;
            this.CBQuality.Items.AddRange(new object[] {
            "最高",
            "1080P",
            "720P",
            "480P",
            "360P",
            "240P",
            "144P"});
            this.CBQuality.Location = new System.Drawing.Point(104, 372);
            this.CBQuality.Name = "CBQuality";
            this.CBQuality.Size = new System.Drawing.Size(80, 23);
            this.CBQuality.TabIndex = 20;
            this.CBQuality.SelectedIndexChanged += new System.EventHandler(this.CBQuality_SelectedIndexChanged);
            // 
            // LSpeed
            // 
            this.LSpeed.AutoSize = true;
            this.LSpeed.Location = new System.Drawing.Point(256, 377);
            this.LSpeed.Name = "LSpeed";
            this.LSpeed.Size = new System.Drawing.Size(20, 15);
            this.LSpeed.TabIndex = 22;
            this.LSpeed.Text = "1x";
            this.LSpeed.DoubleClick += new System.EventHandler(this.LSpeed_DoubleClick);
            // 
            // BtnMute
            // 
            this.BtnMute.Location = new System.Drawing.Point(338, 370);
            this.BtnMute.Name = "BtnMute";
            this.BtnMute.Size = new System.Drawing.Size(100, 23);
            this.BtnMute.TabIndex = 23;
            this.BtnMute.Text = "靜音／取消靜音";
            this.BtnMute.UseVisualStyleBackColor = true;
            this.BtnMute.Click += new System.EventHandler(this.BtnMute_Click);
            // 
            // BtnRandomPlay
            // 
            this.BtnRandomPlay.Location = new System.Drawing.Point(733, 562);
            this.BtnRandomPlay.Name = "BtnRandomPlay";
            this.BtnRandomPlay.Size = new System.Drawing.Size(75, 23);
            this.BtnRandomPlay.TabIndex = 32;
            this.BtnRandomPlay.Text = "隨機播放";
            this.BtnRandomPlay.UseVisualStyleBackColor = true;
            this.BtnRandomPlay.Click += new System.EventHandler(this.BtnRandomPlay_Click);
            // 
            // BtnRandomPlaylist
            // 
            this.BtnRandomPlaylist.Location = new System.Drawing.Point(653, 562);
            this.BtnRandomPlaylist.Margin = new System.Windows.Forms.Padding(2);
            this.BtnRandomPlaylist.Name = "BtnRandomPlaylist";
            this.BtnRandomPlaylist.Size = new System.Drawing.Size(75, 23);
            this.BtnRandomPlaylist.TabIndex = 31;
            this.BtnRandomPlaylist.Text = "隨機排序";
            this.BtnRandomPlaylist.UseVisualStyleBackColor = true;
            this.BtnRandomPlaylist.Click += new System.EventHandler(this.BtnRandomPlaylist_Click);
            // 
            // BtnExportLog
            // 
            this.BtnExportLog.Location = new System.Drawing.Point(192, 563);
            this.BtnExportLog.Name = "BtnExportLog";
            this.BtnExportLog.Size = new System.Drawing.Size(75, 23);
            this.BtnExportLog.TabIndex = 26;
            this.BtnExportLog.Text = "匯出紀錄";
            this.BtnExportLog.UseVisualStyleBackColor = true;
            this.BtnExportLog.Click += new System.EventHandler(this.BtnExportLog_Click);
            // 
            // CBNotShowVideo
            // 
            this.CBNotShowVideo.AutoSize = true;
            this.CBNotShowVideo.Location = new System.Drawing.Point(12, 374);
            this.CBNotShowVideo.Name = "CBNotShowVideo";
            this.CBNotShowVideo.Size = new System.Drawing.Size(86, 19);
            this.CBNotShowVideo.TabIndex = 19;
            this.CBNotShowVideo.Text = "不顯示影像";
            this.CBNotShowVideo.UseVisualStyleBackColor = true;
            this.CBNotShowVideo.CheckedChanged += new System.EventHandler(this.CBNotShowVideo_CheckedChanged);
            // 
            // LVersion
            // 
            this.LVersion.AutoSize = true;
            this.LVersion.Location = new System.Drawing.Point(12, 566);
            this.LVersion.Name = "LVersion";
            this.LVersion.Size = new System.Drawing.Size(43, 15);
            this.LVersion.TabIndex = 25;
            this.LVersion.Text = "版本：";
            // 
            // LYtDlpVersion
            // 
            this.LYtDlpVersion.AutoSize = true;
            this.LYtDlpVersion.Location = new System.Drawing.Point(444, 566);
            this.LYtDlpVersion.Name = "LYtDlpVersion";
            this.LYtDlpVersion.Size = new System.Drawing.Size(80, 15);
            this.LYtDlpVersion.TabIndex = 30;
            this.LYtDlpVersion.Text = "yt-dlp 版本：";
            // 
            // BtnUpdateYtDlp
            // 
            this.BtnUpdateYtDlp.Location = new System.Drawing.Point(354, 563);
            this.BtnUpdateYtDlp.Name = "BtnUpdateYtDlp";
            this.BtnUpdateYtDlp.Size = new System.Drawing.Size(85, 23);
            this.BtnUpdateYtDlp.TabIndex = 28;
            this.BtnUpdateYtDlp.Text = "更新 yt-dlp";
            this.BtnUpdateYtDlp.UseVisualStyleBackColor = true;
            this.BtnUpdateYtDlp.Click += new System.EventHandler(this.BtnUpdateYtDlp_Click);
            // 
            // CBNetPlaylists
            // 
            this.CBNetPlaylists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBNetPlaylists.FormattingEnabled = true;
            this.CBNetPlaylists.Location = new System.Drawing.Point(443, 10);
            this.CBNetPlaylists.Name = "CBNetPlaylists";
            this.CBNetPlaylists.Size = new System.Drawing.Size(366, 23);
            this.CBNetPlaylists.TabIndex = 6;
            // 
            // BtnRefreshNetPlaylists
            // 
            this.BtnRefreshNetPlaylists.Location = new System.Drawing.Point(815, 10);
            this.BtnRefreshNetPlaylists.Name = "BtnRefreshNetPlaylists";
            this.BtnRefreshNetPlaylists.Size = new System.Drawing.Size(74, 23);
            this.BtnRefreshNetPlaylists.TabIndex = 7;
            this.BtnRefreshNetPlaylists.Text = "重新整理";
            this.BtnRefreshNetPlaylists.UseVisualStyleBackColor = true;
            this.BtnRefreshNetPlaylists.Click += new System.EventHandler(this.BtnRefreshNetPlaylists_Click);
            // 
            // CBAutoLyric
            // 
            this.CBAutoLyric.AutoSize = true;
            this.CBAutoLyric.Location = new System.Drawing.Point(714, 43);
            this.CBAutoLyric.Name = "CBAutoLyric";
            this.CBAutoLyric.Size = new System.Drawing.Size(74, 19);
            this.CBAutoLyric.TabIndex = 10;
            this.CBAutoLyric.Text = "自動歌詞";
            this.CBAutoLyric.UseVisualStyleBackColor = true;
            this.CBAutoLyric.CheckedChanged += new System.EventHandler(this.CBAutoLyric_CheckedChanged);
            // 
            // BtnMoreFunction
            // 
            this.BtnMoreFunction.Location = new System.Drawing.Point(814, 562);
            this.BtnMoreFunction.Name = "BtnMoreFunction";
            this.BtnMoreFunction.Size = new System.Drawing.Size(75, 23);
            this.BtnMoreFunction.TabIndex = 33;
            this.BtnMoreFunction.Text = "更多功能";
            this.BtnMoreFunction.UseVisualStyleBackColor = true;
            this.BtnMoreFunction.Click += new System.EventHandler(this.BtnMoreFunction_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(904, 601);
            this.Controls.Add(this.BtnMoreFunction);
            this.Controls.Add(this.CBAutoLyric);
            this.Controls.Add(this.BtnRefreshNetPlaylists);
            this.Controls.Add(this.CBNetPlaylists);
            this.Controls.Add(this.BtnUpdateYtDlp);
            this.Controls.Add(this.LYtDlpVersion);
            this.Controls.Add(this.LVersion);
            this.Controls.Add(this.CBNotShowVideo);
            this.Controls.Add(this.BtnExportLog);
            this.Controls.Add(this.BtnRandomPlaylist);
            this.Controls.Add(this.BtnRandomPlay);
            this.Controls.Add(this.BtnMute);
            this.Controls.Add(this.LSpeed);
            this.Controls.Add(this.NUDSpeed);
            this.Controls.Add(this.BtnSavePlaylist);
            this.Controls.Add(this.BtnLoadPlaylist);
            this.Controls.Add(this.CBQuality);
            this.Controls.Add(this.BtnClearLog);
            this.Controls.Add(this.LName);
            this.Controls.Add(this.BtnPrevious);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.DgvSongList);
            this.Controls.Add(this.BtnPause);
            this.Controls.Add(this.LVolume);
            this.Controls.Add(this.TBVolume);
            this.Controls.Add(this.LDuration);
            this.Controls.Add(this.BtnClearPlaylist);
            this.Controls.Add(this.TBSeek);
            this.Controls.Add(this.PBProgress);
            this.Controls.Add(this.TBLog);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.BtnPlay);
            this.Controls.Add(this.PPlayerHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "CustomYTPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.TBSeek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DgvSongList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUDSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel PPlayerHost;
        private Button BtnPlay;
        private Button BtnStop;
        private TextBox TBLog;
        private ProgressBar PBProgress;
        private TrackBar TBSeek;
        private Button BtnClearPlaylist;
        private Label LDuration;
        private TrackBar TBVolume;
        private Label LVolume;
        private Button BtnPause;
        private DataGridView DgvSongList;
        private Button BtnNext;
        private Button BtnPrevious;
        private Label LName;
        private Button BtnClearLog;
        private Button BtnLoadPlaylist;
        private Button BtnSavePlaylist;
        private NumericUpDown NUDSpeed;
        private ComboBox CBQuality;
        private Label LSpeed;
        private Button BtnMute;
        private Button BtnRandomPlay;
        private Button BtnRandomPlaylist;
        private Button BtnExportLog;
        private CheckBox CBNotShowVideo;
        private Label LVersion;
        private Label LYtDlpVersion;
        private Button BtnUpdateYtDlp;
        private ComboBox CBNetPlaylists;
        private Button BtnRefreshNetPlaylists;
        private CheckBox CBAutoLyric;
        private DataGridViewTextBoxColumn VideoID;
        private DataGridViewTextBoxColumn SongName;
        private DataGridViewTextBoxColumn StartTime;
        private DataGridViewTextBoxColumn EndTime;
        private DataGridViewTextBoxColumn SubSrc;
        private Button BtnMoreFunction;
    }
}