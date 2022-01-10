namespace CustomYTPlayer.Extensions;

/// <summary>
/// HttpClientProgress Extensions
/// <para>來源：https://gist.github.com/dalexsoto/9fd3c5bdbe9f61a717d47c5843384d11 </para>
/// </summary>
public static class HttpClientProgressExtensions
{
    /// <summary>
    /// 非同步下載資料
    /// </summary>
    /// <param name="client">HttpClient</param>
    /// <param name="requestUrl">字串，網址</param>
    /// <param name="destination">Stream</param>
    /// <param name="progress1">IProgress&lt;float&gt;</param>
    /// <param name="progress2">IProgress&lt;string&gt;</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Task</returns>
    public static async Task DownloadDataAsync(
        this HttpClient client,
        string requestUrl,
        Stream destination,
        IProgress<float>? progress1 = null,
        IProgress<string>? progress2 = null,
        CancellationToken cancellationToken = default)
    {
        using (HttpResponseMessage response = await client.GetAsync(
            requestUrl,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken))
        {
            long? contentLength = response.Content.Headers.ContentLength;

            using Stream download = await response.Content.ReadAsStreamAsync(cancellationToken);

            // No progress...no contentLength... very sad.
            if ((progress1 is null && progress2 is null) || !contentLength.HasValue)
            {
                await download.CopyToAsync(destination, cancellationToken);

                return;
            }

            // 2022-03-17 用於減速 progress2?.Report();。
            int tempCount = 0;

            // Such progress and contentLength much reporting Wow!
            Progress<long> progressWrapper = new(totalBytes =>
            {
                progress1?.Report(GetProgressPercentage(totalBytes, contentLength.Value));

                if (tempCount % 100 == 0 || totalBytes == contentLength.Value)
                {
                    progress2?.Report(GetProgressPercentageString(totalBytes, contentLength.Value));
                }

                tempCount++;
            });

            await download.CopyToAsync(destination, 81920, progressWrapper, cancellationToken);
        }

        static float GetProgressPercentage(float totalBytes, float currentBytes) => (totalBytes / currentBytes) * 100f;

        static string GetProgressPercentageString(float totalBytes, float currentBytes) =>
            $"{totalBytes / (1024f * 1024f)}/{currentBytes / (1024f * 1024f)} MB " +
            $"({Math.Round(totalBytes / currentBytes * 100f, MidpointRounding.AwayFromZero)}%)";
    }

    /// <summary>
    /// 非同步複製至
    /// </summary>
    /// <param name="source">Stream</param>
    /// <param name="destination">Stream</param>
    /// <param name="bufferSize">數值</param>
    /// <param name="progress">IProgress&lt;float&gt;</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Task</returns>
    /// <exception cref="ArgumentOutOfRangeException">ArgumentOutOfRangeException</exception>
    /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
    /// <exception cref="InvalidOperationException">InvalidOperationException</exception>
    static async Task CopyToAsync(
        this Stream source,
        Stream destination,
        int bufferSize,
        IProgress<long>? progress = null,
        CancellationToken cancellationToken = default)
    {
        if (bufferSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bufferSize));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (!source.CanRead)
        {
            throw new InvalidOperationException($"'{nameof(source)}' 是不可寫讀取的。");
        }

        if (destination == null)
        {
            throw new ArgumentNullException(nameof(destination));
        }

        if (!destination.CanWrite)
        {
            throw new InvalidOperationException($"'{nameof(destination)}' 是不可寫入的。");
        }

        byte[] buffer = new byte[bufferSize];

        long totalBytesRead = 0;

        int bytesRead;

        while ((bytesRead = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
        {
            await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);

            totalBytesRead += bytesRead;

            progress?.Report(totalBytesRead);
        }
    }
}