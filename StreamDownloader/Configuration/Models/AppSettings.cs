
namespace StreamDownloader.Configuration.Models;

using StreamDownloader.Configuration;

public class AppSettings
{
    public required DownloaderSettings DownloaderSettings { get; init; }

    public void Validate()
    {
        this.DownloaderSettings.Validate();
    }

    public static AppSettings Load(string filePath)
    {
        var settings = Configuration.Load<AppSettings>(filePath);
#if !DEBUG
        settings.Validate();
#endif
        return settings;
    }
}
