
namespace StreamDownloader.Configuration.Models;

using System.IO;

public class DownloaderSettings
{
    public required string FilePath { get; init; }

    public required DownloaderOption[] Options { get; init; } = [];

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
}
