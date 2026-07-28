
namespace StreamDownloader.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using StreamDownloader.Configuration.Models;
using StreamDownloader.Downloader;

using System.Collections.ObjectModel;

public partial class MainViewModel : ObservableObject
{
    private readonly IDownloader downloader;

    public MainViewModel()
        : this(AppSettings.Load("appsettings.json"))
    {
    }

    public MainViewModel(AppSettings appSettings)
        : this(appSettings.DownloaderSettings)
    {
    }

    public MainViewModel(DownloaderSettings downloaderSettings)
        : this(downloaderSettings, new Downloader(downloaderSettings))
    {
    }

    public MainViewModel(DownloaderSettings downloaderSettings, IDownloader downloader)
    {
        this.downloader = downloader;

        this.Options = new ObservableCollection<DownloaderOption>(downloaderSettings.Options);
        this.Urls.CollectionChanged += (sender, e) =>
        {
            this.AddCommand.NotifyCanExecuteChanged();
            this.DownloadCommand.NotifyCanExecuteChanged();
        };
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCommand))]
    public partial string Url { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsKeepSources { get; set; } = true;

    public ObservableCollection<string> Urls { get; } = [];
    public ObservableCollection<DownloaderOption> Options { get; }

    [RelayCommand(CanExecute = nameof(CanAdd))]
    private void Add()
    {
        if (!this.CanAdd())
        {
            return;
        }

        this.Urls.Add(this.Url.Trim());
        this.Url = string.Empty;
    }

    private bool CanAdd()
    {
        var url = this.Url.Trim();
        return !string.IsNullOrWhiteSpace(url) && !this.Urls.Contains(url);
    }

    [RelayCommand]
    private void Clear()
    {
        this.Urls.Clear();
    }

    [RelayCommand(CanExecute = nameof(CanDownload))]
    private void Download()
    {
        if (!this.CanDownload())
        {
            return;
        }

        this.StartDownload([this.Urls.ToArray()]);
    }

    [RelayCommand(CanExecute = nameof(CanDownload))]
    private void ParallelDownload()
    {
        if (!this.CanDownload())
        {
            return;
        }

        this.StartDownload(this.Urls.Select(url => (IReadOnlyList<string>)[url]));
    }

    private bool CanDownload()
    {
        return this.Urls.Any();
    }

    private void StartDownload(IEnumerable<IReadOnlyList<string>> urlGroups)
    {
        foreach (var urlGroup in urlGroups)
        {
            this.downloader.Download(urlGroup);
        }

        if (!this.IsKeepSources)
        {
            this.Clear();
        }
    }
}
