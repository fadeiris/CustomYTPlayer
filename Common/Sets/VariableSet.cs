namespace CustomYTPlayer.Common.Sets;

/// <summary>
/// 變數組
/// </summary>
public static class VariableSet
{
    /// <summary>
    /// 執行檔路徑
    /// </summary>
    public static readonly string ExecPath = Path.Combine(AppContext.BaseDirectory, "lib");

    /// <summary>
    /// libmpv 壓縮檔名稱
    /// </summary>
    public static readonly string LibmpvArchiveName = "mpv-dev-x86_64-20211212-git-0e76372.7z";

    /// <summary>
    /// libmpv 壓縮檔路徑
    /// </summary>
    public static readonly string LibmpvArchivePath = Path.Combine(ExecPath, LibmpvArchiveName);

    /// <summary>
    /// libmpv 壓縮檔下載網址
    /// </summary>
    public static readonly string LibmpvArchiveUrl = "https://sourceforge.net/projects/mpv-player-windows/files/libmpv/mpv-dev-x86_64-20211212-git-0e76372.7z/download";

    /// <summary>
    /// libmpv *.dll 名稱
    /// </summary>
    public static readonly string LibmpvDllName = "mpv-1.dll";

    /// <summary>
    /// libmpv *.dll 路徑
    /// </summary>
    public static readonly string LibmpvDllPath = Path.Combine(ExecPath, LibmpvDllName);

    /// <summary>
    /// ytdl_hook.lua 路徑
    /// </summary>
    public static readonly string YtDlHookLuaPath = Path.Combine(ExecPath, "ytdl_hook.lua");

    /// <summary>
    /// ytdl_hook.lua 下載網址
    /// </summary>
    public static readonly string YtDlHookLuaUrl = "https://raw.githubusercontent.com/mpv-player/mpv/master/player/lua/ytdl_hook.lua";

    /// <summary>
    /// yt-dlp.exe 路徑
    /// </summary>
    public static readonly string YtDlpExePath = Path.Combine(ExecPath, "yt-dlp.exe");

    /// <summary>
    /// yt-dlp 下載網址
    /// </summary>
    public static readonly string YtDlpBinaryUrl = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";

    /// <summary>
    /// 播放清單檔案資料夾的路徑
    /// </summary>
    public static readonly string PlaylistsFolderPath = Path.Combine(AppContext.BaseDirectory, "Playlists");

    /// <summary>
    /// YoutubeClipPlaylist 的基礎網址
    /// </summary>
    public static readonly string YouTubeClipPlaylistBaseUrl = "https://raw.githubusercontent.com/YoutubeClipPlaylist/Playlists/minify/";

    /// <summary>
    /// YoutubeClipPlaylist/Playlists 的 Playlists.jsonc 的網址
    /// </summary>
    public static readonly string YouTubeClipPlaylistPlaylistsJsonUrl = $"{YouTubeClipPlaylistBaseUrl}Playlists.jsonc";

    /// <summary>
    /// YoutubeClipPlaylist/Lyrics 的 Lyrics.json 的網址
    /// </summary>
    public static readonly string YouTubeClipPlaylistLyricsJsonUrl = "https://raw.githubusercontent.com/YoutubeClipPlaylist/Lyrics/minify/Lyrics.json";

    /// <summary>
    /// YoutubeClipPlaylist/Lyrics 的 *.lrc 的模版網址
    /// </summary>
    public static readonly string YouTubeClipPlaylistLrcFileTemplateUrl = "https://raw.githubusercontent.com/YoutubeClipPlaylist/Lyrics/minify/Lyrics/[SongId].lrc";

    /// <summary>
    /// fadeiris/CustomPlaylist 的基礎網址
    /// </summary>
    public static readonly string FadeirisCustomPlaylistBaseUrl = "https://raw.githubusercontent.com/fadeiris/CustomPlaylist/main/";

    /// <summary>
    /// fadeiris/CustomPlaylist 的 Playlists.jsonc 的網址
    /// </summary>
    public static readonly string FadeirisCustomPlaylistPlaylistsJsonUrl = $"{FadeirisCustomPlaylistBaseUrl}Playlists.jsonc";

    /// <summary>
    /// fadeiris/CustomPlaylist 的 B23Playlists.jsonc 的網址
    /// </summary>
    public static readonly string FadeirisCustomPlaylistB23PlaylistsJsonUrl = $"{FadeirisCustomPlaylistBaseUrl}B23Playlists.jsonc";

    /// <summary>
    /// 區塊分隔線
    /// </summary>
    public static readonly string BlockSeparator = "-------";

    /// <summary>
    /// 允許的附檔名（播放清單檔案）
    /// </summary>
    public static readonly string[] AllowedExts =
    {
        ".txt",
        ".json",
        ".jsonc"
    };

    /// <summary>
    /// TBSeek 控制項的 Minimum
    /// </summary>
    public static readonly int TBSeekMinimum = 1;

    /// <summary>
    /// 預設播放清單檔案名稱
    /// </summary>
    public static readonly string DefaultPlaylistFileName = "CustomYTPlayer_Playlist_";
}