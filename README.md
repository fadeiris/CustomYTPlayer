# 自定義 YouTube 影片播放器

基於 `libmpv` 與 `yt-dlp` 的`自定義 YouTube 影片播放器`，支援簡易的多媒體播放、播放清單等功能。

## 簡易說明

1. 有關 `yt-dlp` 的設定，請調整 `.\lib\yt-dlp.conf` 檔案。
2. 有關 `libmpv` 的設定，請調整 `.\lib\mpv.conf` 檔案。
3. 有關本應用程式的其他設定，請調整 `CustomYTPlayer.dll.config` 檔案。
4. 將播放清單檔案放置於 `.\Playlists` 資料夾內，即可在本應用程式開啟時自動載入。
5. 用滑鼠雙擊影片播放區塊，可以開關彈出視窗以檢視影片內容。

### 注意事項

若有需求時，請自行調整 `.\lib\yt-dlp.conf` 檔案的內容以及其所使用的文字編碼。

## 快捷鍵

- `Q 鍵`：結束應用程式。
- `W 鍵`：讓彈出視窗恢復成預設大小（720p）。
- `E 鍵`：開關彈出視窗。
- `R 鍵`：開關彈出視窗的全螢幕顯示。
- `A 鍵`：播放。
- `S 鍵`：暫停 / 恢復播放。
- `D 鍵`：上一個。
- `F 鍵`：下一個。
- `G 鍵`：停止播放。
- `Z 鍵`：隨機播放。
- `X 鍵`：隨機排序播放清單。
- `C 鍵`：不顯示影像。
- `V 鍵`：靜音／取消靜音。

※僅會在應用程式的主畫面顯示且有焦點時才會有效。

## 相關連結

### 函式庫

- [Mpv.NET (lib)](https://github.com/hudec117/Mpv.NET-lib-)
  - [Mpv.NET.WinFormsExample](https://github.com/hudec117/Mpv.NET-lib-/tree/master/src/Mpv.NET.WinFormsExample) 
- [SevenZipExtractor](https://github.com/adoconnection/SevenZipExtractor)
- [Discord Rich Presence](https://github.com/Lachee/discord-rpc-csharp)

### 相依性檔案、使用手冊以及外部應用程式

- mpv
    - [libmpv](https://sourceforge.net/projects/mpv-player-windows/files/libmpv/)
      - 版本：`mpv-dev-x86_64-20211212-git-0e76372.7z`
      - ※從 2021-12-15 之後，mpv-1.dll 已變更名稱為 mpv-2.dll，需等待 Mpv.NET (lib) 函式庫支援後才可以使用。
  - [ytdl_hook.lua](https://github.com/mpv-player/mpv/blob/master/player/lua/ytdl_hook.lua)
    - 請將`第 11 行`的 `ytdl_path = "",` 替換成 `ytdl_path = "lib\\yt-dlp",`。
  - [mpv Reference Manual](https://mpv.io/manual/master/)
    - mpv 使用手冊。
- [yt-dlp](https://github.com/yt-dlp/yt-dlp)
  - [yt-dlp/FFmpeg-Builds](https://github.com/yt-dlp/FFmpeg-Builds)

### 其他
- [Youtube 影片截選播放清單](https://github.com/YoutubeClipPlaylist/YoutubeClipPlaylist)
  - [Playlists Repo](https://github.com/YoutubeClipPlaylist/Playlists)
    - 網路播放清單檔案來源。
  - [Lyrics Repo](https://github.com/YoutubeClipPlaylist/Lyrics)
    - 自動歌詞檔案來源。
- [自定義播放清單](https://github.com/fadeiris/CustomPlaylist)
