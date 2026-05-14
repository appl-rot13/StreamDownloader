
namespace StreamDownloader.Configuration.Models;

public class AppSettings
{
    public required DownloaderSettings DownloaderSettings { get; init; }

    public void Validate()
    {
        this.DownloaderSettings.Validate();
    }
}
