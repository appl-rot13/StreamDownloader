
namespace StreamDownloader.Downloader;

using StreamDownloader.Configuration.Models;
using StreamDownloader.Extensions;

using System.Diagnostics;

public class Downloader(DownloaderSettings settings) : IDownloader
{
    private readonly DownloaderSettings settings = settings;

    public void Download(IReadOnlyList<string> urls)
    {
        var command = this.CreateCommand(urls) + " & echo ExitCode: %ERRORLEVEL%" + " & pause";
        Process.Start("cmd.exe", $"/c {command.DoubleQuoted()}");
    }

    public string CreateCommand(IReadOnlyList<string> urls)
    {
        ArgumentNullException.ThrowIfNull(urls, nameof(urls));

        if (urls.Count == 0)
        {
            throw new ArgumentException("At least one element is required.", nameof(urls));
        }

        var values = new List<string> { this.settings.FilePath.DoubleQuoted() };
        foreach (var option in this.settings.Options)
        {
            if (option.Enabled)
            {
                values.Add(option.Value);
            }
        }

        values.Add("--");
        foreach (var url in urls)
        {
            values.Add(url.DoubleQuoted());
        }

        return string.Join(' ', values);
    }
}
