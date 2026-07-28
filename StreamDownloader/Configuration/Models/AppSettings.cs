
namespace StreamDownloader.Configuration.Models;

public class AppSettings
{
    public required DownloaderSettings DownloaderSettings { get; init; }

    public void Validate()
    {
#if !DEBUG
        this.DownloaderSettings.Validate();
#endif
    }
}
