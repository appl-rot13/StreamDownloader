namespace StreamDownloaderTest.Downloader;

using Shouldly;
using StreamDownloader.Configuration.Models;
using StreamDownloader.Downloader;

[TestClass]
public class DownloaderTest
{
    [TestMethod]
    public void CreateCommand_NoOptions()
    {
        var downloader = CreateDownloader(@".\yt-dlp.exe");
        var command = downloader.CreateCommand(["https://www.youtube.com/watch?v=VideoID-1", "https://www.youtube.com/watch?v=VideoID-2"]);

        command.ShouldBe(@""".\yt-dlp.exe"" -- ""https://www.youtube.com/watch?v=VideoID-1"" ""https://www.youtube.com/watch?v=VideoID-2""");
    }

    [TestMethod]
    public void CreateCommand_IgnoresDisabledOptions()
    {
        var downloader = CreateDownloader(
            @".\yt-dlp.exe",
            new DownloaderOption { Enabled = false, Value = "--verbose" },
            new DownloaderOption { Enabled = true, Value = "--wait-for-video 30" },
            new DownloaderOption { Enabled = true, Value = @"--cookies ""cookies.txt""" });
        var command = downloader.CreateCommand(["VideoID-1", "VideoID-2"]);

        command.ShouldBe(@""".\yt-dlp.exe"" --wait-for-video 30 --cookies ""cookies.txt"" -- ""VideoID-1"" ""VideoID-2""");
    }

    [TestMethod]
    public void CreateCommand_AllOptionsDisabled()
    {
        var downloader = CreateDownloader(
            @".\yt-dlp.exe",
            new DownloaderOption { Enabled = false, Value = "--verbose" },
            new DownloaderOption { Enabled = false, Value = "--wait-for-video 30" },
            new DownloaderOption { Enabled = false, Value = @"--cookies ""cookies.txt""" });
        var command = downloader.CreateCommand(["VideoID-1"]);

        command.ShouldBe(@""".\yt-dlp.exe"" -- ""VideoID-1""");
    }

    [TestMethod]
    public void CreateCommand_NullUrls_ThrowsNullArgumentException()
    {
        var downloader = CreateDownloader(@".\yt-dlp.exe");
        Should.Throw<ArgumentNullException>(() => downloader.CreateCommand(null!));
    }

    [TestMethod]
    public void CreateCommand_EmptyUrls_ThrowsArgumentException()
    {
        var downloader = CreateDownloader(@".\yt-dlp.exe");
        Should.Throw<ArgumentException>(() => downloader.CreateCommand([]));
    }

    private static Downloader CreateDownloader(string filePath, params DownloaderOption[] options)
    {
        return new Downloader(new DownloaderSettings { FilePath = filePath, Options = options });
    }
}
