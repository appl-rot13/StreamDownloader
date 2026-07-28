
namespace StreamDownloaderTest.ViewModels;

using NSubstitute;

using Shouldly;

using StreamDownloader.Configuration.Models;
using StreamDownloader.Downloader;
using StreamDownloader.ViewModels;

[TestClass]
public class MainViewModelTest
{
    [TestMethod]
    public void Constructor_InitializesProperties()
    {
        var viewModel = CreateMainViewModel(out var settings, out _);

        viewModel.Url.ShouldBe(string.Empty);
        viewModel.IsKeepSources.ShouldBeTrue();

        viewModel.Urls.ShouldBeEmpty();
        viewModel.Options.ShouldBe(settings.Options);
    }

    [TestMethod]
    [DataRow(" ")]
    public void AddCommand_WhiteSpaceUrl_DoesNotAdd(string url)
    {
        var viewModel = CreateMainViewModel();

        viewModel.Url.ShouldBe(string.Empty);
        viewModel.Urls.ShouldBeEmpty();
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();

        viewModel.Url = url;
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();
        viewModel.AddCommand.Execute(null);

        viewModel.Url.ShouldBe(url);
        viewModel.Urls.ShouldBeEmpty();
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();
    }

    [TestMethod]
    [DataRow("https://www.youtube.com/watch?v=VideoID-1")]
    [DataRow(" https://www.youtube.com/watch?v=VideoID-2 ")]
    public void AddCommand_NonWhiteSpaceUrl_AddsTrimmedUrl(string url)
    {
        var viewModel = CreateMainViewModel();

        viewModel.Url.ShouldBe(string.Empty);
        viewModel.Urls.ShouldBeEmpty();
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();

        viewModel.Url = url;
        viewModel.AddCommand.CanExecute(null).ShouldBeTrue();
        viewModel.AddCommand.Execute(null);

        viewModel.Url.ShouldBe(string.Empty);
        viewModel.Urls.ShouldBe([url.Trim()]);
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();
    }

    [TestMethod]
    [DataRow("https://www.youtube.com/watch?v=VideoID-1")]
    [DataRow(" https://www.youtube.com/watch?v=VideoID-2 ")]
    public void AddCommand_DuplicateUrl_DoesNotAdd(string url)
    {
        var viewModel = CreateMainViewModel(url);

        viewModel.Url.ShouldBe(string.Empty);
        viewModel.Urls.ShouldNotBeEmpty();
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();

        viewModel.Url = url;
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();
        viewModel.AddCommand.Execute(null);

        viewModel.Url.ShouldBe(url);
        viewModel.Urls.ShouldBe([url.Trim()]);
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();
    }

    [TestMethod]
    public void ClearCommand_ClearsUrls()
    {
        var viewModel = CreateMainViewModel("VideoID-1");

        viewModel.Urls.ShouldNotBeEmpty();
        viewModel.ClearCommand.CanExecute(null).ShouldBeTrue();

        viewModel.ClearCommand.Execute(null);

        viewModel.Urls.ShouldBeEmpty();
        viewModel.ClearCommand.CanExecute(null).ShouldBeTrue();
    }

    [TestMethod]
    public void ClearCommand_EnablesAddCommand()
    {
        var url = "VideoID-1";
        var viewModel = CreateMainViewModel(url);

        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();

        viewModel.Url = url;
        viewModel.AddCommand.CanExecute(null).ShouldBeFalse();

        viewModel.ClearCommand.Execute(null);
        viewModel.AddCommand.CanExecute(null).ShouldBeTrue();
    }

    [TestMethod]
    public void ClearCommand_DisablesDownloadCommand()
    {
        var viewModel = CreateMainViewModel();

        viewModel.Url = "VideoID-1";
        viewModel.DownloadCommand.CanExecute(null).ShouldBeFalse();

        viewModel.AddCommand.Execute(null);
        viewModel.DownloadCommand.CanExecute(null).ShouldBeTrue();

        viewModel.ClearCommand.Execute(null);
        viewModel.DownloadCommand.CanExecute(null).ShouldBeFalse();
    }

    [TestMethod]
    public void ClearCommand_DisablesParallelDownloadCommand()
    {
        var viewModel = CreateMainViewModel();

        viewModel.Url = "VideoID-1";
        viewModel.ParallelDownloadCommand.CanExecute(null).ShouldBeFalse();

        viewModel.AddCommand.Execute(null);
        viewModel.ParallelDownloadCommand.CanExecute(null).ShouldBeTrue();

        viewModel.ClearCommand.Execute(null);
        viewModel.ParallelDownloadCommand.CanExecute(null).ShouldBeFalse();
    }

    [TestMethod]
    public void DownloadCommand_NoUrls_DoesNotDownload()
    {
        var viewModel = CreateMainViewModel(out var downloader);

        viewModel.Urls.ShouldBeEmpty();
        viewModel.DownloadCommand.CanExecute(null).ShouldBeFalse();

        viewModel.DownloadCommand.Execute(null);
        downloader.DidNotReceive().Download(Arg.Any<IReadOnlyList<string>>());

        viewModel.Urls.ShouldBeEmpty();
        viewModel.DownloadCommand.CanExecute(null).ShouldBeFalse();
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void DownloadCommand_WithUrls_Downloads(bool isKeepSources)
    {
        var urls = new[] { "VideoID-1", "VideoID-2", "VideoID-3" };
        var viewModel = CreateMainViewModel(out var downloader, urls);
        viewModel.IsKeepSources = isKeepSources;

        viewModel.Urls.ShouldBe(urls);
        viewModel.DownloadCommand.CanExecute(null).ShouldBeTrue();

        viewModel.DownloadCommand.Execute(null);
        downloader.Received(1).Download(Arg.Any<IReadOnlyList<string>>());
        downloader.Received(1).Download(Arg.Is<IReadOnlyList<string>>(arg => arg != null && arg.SequenceEqual(urls)));

        if (isKeepSources)
        {
            viewModel.Urls.ShouldBe(urls);
            viewModel.DownloadCommand.CanExecute(null).ShouldBeTrue();
        }
        else
        {
            viewModel.Urls.ShouldBeEmpty();
            viewModel.DownloadCommand.CanExecute(null).ShouldBeFalse();
        }
    }

    [TestMethod]
    public void ParallelDownloadCommand_NoUrls_DoesNotDownload()
    {
        var viewModel = CreateMainViewModel(out var downloader);

        viewModel.Urls.ShouldBeEmpty();
        viewModel.ParallelDownloadCommand.CanExecute(null).ShouldBeFalse();

        viewModel.ParallelDownloadCommand.Execute(null);
        downloader.DidNotReceive().Download(Arg.Any<IReadOnlyList<string>>());

        viewModel.Urls.ShouldBeEmpty();
        viewModel.ParallelDownloadCommand.CanExecute(null).ShouldBeFalse();
    }

    [TestMethod]
    [DataRow(true)]
    [DataRow(false)]
    public void ParallelDownloadCommand_WithUrls_Downloads(bool isKeepSources)
    {
        var urls = new[] { "VideoID-1", "VideoID-2", "VideoID-3" };
        var viewModel = CreateMainViewModel(out var downloader, urls);
        viewModel.IsKeepSources = isKeepSources;

        viewModel.Urls.ShouldBe(urls);
        viewModel.ParallelDownloadCommand.CanExecute(null).ShouldBeTrue();

        viewModel.ParallelDownloadCommand.Execute(null);
        downloader.Received(urls.Length).Download(Arg.Any<IReadOnlyList<string>>());
        foreach (var url in urls)
        {
            // 並列ダウンロードのため、呼び出し順はテストしない
            downloader.Received(1).Download(Arg.Is<IReadOnlyList<string>>(arg => arg != null && arg.Count == 1 && arg[0] == url));
        }

        if (isKeepSources)
        {
            viewModel.Urls.ShouldBe(urls);
            viewModel.ParallelDownloadCommand.CanExecute(null).ShouldBeTrue();
        }
        else
        {
            viewModel.Urls.ShouldBeEmpty();
            viewModel.ParallelDownloadCommand.CanExecute(null).ShouldBeFalse();
        }
    }

    private static MainViewModel CreateMainViewModel(params string[] urls)
    {
        return CreateMainViewModel(out _, urls);
    }

    private static MainViewModel CreateMainViewModel(out IDownloader downloader, params string[] urls)
    {
        return CreateMainViewModel(out _, out downloader, urls);
    }

    private static MainViewModel CreateMainViewModel(out DownloaderSettings settings, out IDownloader downloader, params string[] urls)
    {
        settings = new DownloaderSettings
        {
            FilePath = @".\yt-dlp.exe",
            Options = [
                new DownloaderOption { Enabled = false, Value = "--verbose" },
                new DownloaderOption { Enabled = true, Value = "--wait-for-video 30" },
                new DownloaderOption { Enabled = true, Value = @"--cookies ""cookies.txt""" },
            ],
        };

        var viewModel = new MainViewModel(settings, downloader = Substitute.For<IDownloader>());
        foreach (var url in urls)
        {
            viewModel.Url = url;
            viewModel.AddCommand.Execute(null);
        }

        return viewModel;
    }
}
