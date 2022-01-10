namespace CustomYTPlayer.Extensions;

/// <summary>
/// TimeSpan Extension
/// </summary>
public static class TimeSpanExtension
{
    /// <summary>
    /// 移除毫秒
    /// <para>來源：https://stackoverflow.com/a/35750677 </para>
    /// </summary>
    /// <param name="timeSpan">TimeSpan</param>
    /// <returns>TimeSpan</returns>
    public static TimeSpan StripMilliseconds(this TimeSpan timeSpan)
    {
        // 當有毫秒時，秒直接加 1 秒。
        int seconds = timeSpan.Seconds;

        if (timeSpan.Milliseconds > 0)
        {
            seconds++;
        }

        return new TimeSpan(timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, seconds);
    }

    /// <summary>
    /// 轉換成 FFmpeg 的時間格式字串
    /// </summary>
    /// <param name="timeSpan">TimeSpan</param>
    /// <param name="allowDecimalPoint">布林值，是否允許小數點，預設值為 true</param>
    /// <returns>字串</returns>
    public static string ToTimestamp(this TimeSpan timeSpan, bool allowDecimalPoint = true)
    {
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;
        int milliseconds = timeSpan.Milliseconds;

        string decimalPoint = string.Empty;

        if (allowDecimalPoint == true)
        {
            decimalPoint = $".{milliseconds.ToString().PadLeft(3, '0')}";
        }
        else
        {
            // 對毫秒進行四捨五入，當值大於等於 500 毫秒加 1 秒。
            if (milliseconds >= 500)
            {
                seconds++;
            }
        }

        return $"{hours.ToString().PadLeft(2, '0')}:" +
            $"{minutes.ToString().PadLeft(2, '0')}:" +
            $"{seconds.ToString().PadLeft(2, '0')}{decimalPoint}";
    }

    /// <summary>
    /// 轉換成 CUE 指令碼的時間格式字串
    /// </summary>
    /// <param name="timeSpan">TimeSpan</param>
    /// <param name="allowDecimalPoint">布林值，是否允許小數點，預設值為 true</param>
    /// <returns>字串</returns>
    public static string ToCueTimestamp(this TimeSpan timeSpan, bool allowDecimalPoint = true)
    {
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes + (hours * 60);
        int seconds = timeSpan.Seconds;
        int milliseconds = timeSpan.Milliseconds;

        string decimalPoint = string.Empty;

        if (allowDecimalPoint == true)
        {
            // 強制將 ".PadLeft(3, '0')" 改為 ".PadLeft(2, '0')"。
            decimalPoint = $".{milliseconds.ToString().PadLeft(2, '0')}";
        }
        else
        {
            // 對毫秒進行四捨五入，當值大於等於 500 毫秒加 1 秒。
            if (milliseconds >= 500)
            {
                seconds++;
            }
        }

        return $"{minutes.ToString().PadLeft(2, '0')}:" +
            $"{seconds.ToString().PadLeft(2, '0')}{decimalPoint}";
    }

    /// <summary>
    /// 轉換成 YouTube 留言的時間格式字串
    /// </summary>
    /// <param name="timeSpan">TimeSpan</param>
    /// <param name="formated">布林值，用於判斷是否輸出格式化（hh:mm:ss）後的字串，預設值為 false</param>
    /// <returns>字串</returns>
    public static string ToYtTimestamp(this TimeSpan timeSpan, bool formated = false)
    {
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;
        int milliseconds = timeSpan.Milliseconds;

        // 對毫秒進行四捨五入，當值大於等於 500 毫秒加 1 秒。
        if (milliseconds >= 500)
        {
            seconds++;
        }

        if (formated)
        {
            return $"{hours.ToString().PadLeft(2, '0')}:" +
                $"{minutes.ToString().PadLeft(2, '0')}:" +
                $"{seconds.ToString().PadLeft(2, '0')}";
        }
        else
        {
            return $"{(hours > 0 ? $"{hours}:" : string.Empty)}" +
                $"{(hours > 0 ? minutes.ToString().PadLeft(2, '0') : minutes)}:" +
                $"{seconds.ToString().PadLeft(2, '0')}";
        }
    }

    /// <summary>
    /// 轉換成 Twitch 網址的時間格式字串
    /// </summary>
    /// <param name="timeSpan">TimeSpan</param>
    /// <returns>字串</returns>
    public static string ToTwitchTimestamp(this TimeSpan timeSpan)
    {
        int hours = timeSpan.Hours;
        int minutes = timeSpan.Minutes;
        int seconds = timeSpan.Seconds;
        int milliseconds = timeSpan.Milliseconds;

        // 對毫秒進行四捨五入，當值大於等於 500 毫秒加 1 秒。
        if (milliseconds >= 500)
        {
            seconds++;
        }

        return $"{(hours > 0 ? $"{hours}h" : string.Empty)}" +
            $"{(minutes > 0 ? $"{minutes}m" : string.Empty)}{seconds}s";
    }

    /// <summary>
    /// 轉換成綜合時間標記字串
    /// </summary>
    /// <param name="timeSpan">TimeSpan</param>
    /// <returns>字串</returns>
    public static string ToTimestampString(this TimeSpan timeSpan)
    {
        return $"{timeSpan.ToTimestamp()}｜{timeSpan.ToYtTimestamp()}｜" +
            $"{timeSpan.TotalSeconds}｜{timeSpan.ToTwitchTimestamp()}";
    }
}