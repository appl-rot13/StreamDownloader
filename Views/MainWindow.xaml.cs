
namespace StreamDownloader.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        this.InitializeComponent();

        // 右クリックでのコンテキストメニュー表示を無効化
        this.DownloadOptionButton.ContextMenuOpening += (sender, e) => e.Handled = true;

        // トグルボタンのチェックでコンテキストメニューを表示
        this.DownloadOptionButton.Checked += (sender, e) =>
        {
            this.DownloadOptionButton.ContextMenu.PlacementTarget = this.DownloadButton;
            this.DownloadOptionButton.ContextMenu.IsOpen = true;
        };

        // コンテキストメニューのクローズでトグルボタンのチェックを解除
        this.DownloadOptionButton.ContextMenu.Closed += (sender, e) =>
        {
            this.DownloadOptionButton.IsChecked = false;
        };
    }
}
