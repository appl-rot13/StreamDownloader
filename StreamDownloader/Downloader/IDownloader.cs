
namespace StreamDownloader.Downloader;

public interface IDownloader
{
    void Download(IReadOnlyList<string> urls);
}
