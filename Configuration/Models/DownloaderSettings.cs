
namespace StreamDownloader.Configuration.Models;

using StreamDownloader.Extensions;

using System.IO;

public class DownloaderSettings
{
    public required string FilePath { get; init; }

    public required DownloaderOption[] Options { get; init; }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(this.FilePath))
        {
            throw new InvalidOperationException("Downloader file path is not configured.");
        }

        if (!File.Exists(this.FilePath))
        {
            throw new FileNotFoundException($"Downloader '{this.FilePath}' is not found.");
        }
    }

    public string CreateCommand(IEnumerable<string> urls)
    {
        var filePath = this.FilePath.DoubleQuoted();
        var options = string.Join(' ', this.Options.Where(o => o.Enabled).Select(o => o.Value));
        var args = string.Join(' ', urls.Select(u => u.DoubleQuoted()));

        return $"{filePath} {options} -- {args}";
    }
}
