
namespace StreamDownloader.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using StreamDownloader.Configuration;
using StreamDownloader.Configuration.Models;
using StreamDownloader.Extensions;

using System.Collections.ObjectModel;
using System.Diagnostics;

public partial class MainViewModel : ObservableObject
{
    private readonly AppSettings appSettings;

    public MainViewModel()
        : this(Configuration.Load<AppSettings>("appsettings.json"))
    {
    }

    public MainViewModel(AppSettings appSettings)
    {
        this.appSettings = appSettings;
        this.appSettings.Validate();

        this.Options = new ObservableCollection<DownloaderOption>(this.appSettings.DownloaderSettings.Options);
        this.Urls.CollectionChanged += (sender, e) =>
        {
            this.AddCommand.NotifyCanExecuteChanged();
            this.DownloadCommand.NotifyCanExecuteChanged();
        };
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCommand))]
    public partial string Url { get; set; } = string.Empty;

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

    [RelayCommand(CanExecute = nameof(CanDownload))]
    private void Download()
    {
        if (!this.CanDownload())
        {
            return;
        }

        var downloaderCommand = this.appSettings.DownloaderSettings.CreateCommand(this.Urls);
        var command = downloaderCommand + " & echo ExitCode: %ERRORLEVEL%" + " & pause";

        Process.Start("cmd.exe", $"/c {command.DoubleQuoted()}");
        this.Urls.Clear();
    }

    private bool CanDownload()
    {
        return this.Urls.Any();
    }
}
