
namespace StreamDownloader;

using System.Windows;
using System.Windows.Threading;

public partial class App : Application
{
    public App()
    {
        this.DispatcherUnhandledException += this.OnUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += this.OnUnhandledException;
        TaskScheduler.UnobservedTaskException += this.OnUnhandledException;
    }

    protected virtual void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ShowException(e.Exception);
        Environment.Exit(-1);
    }

    protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ShowException(e.ExceptionObject as Exception);
        Environment.Exit(-1);
    }

    protected virtual void OnUnhandledException(object? sender, UnobservedTaskExceptionEventArgs e)
    {
        ShowException(e.Exception);
        e.SetObserved();
    }

    private static void ShowException(Exception? exception)
    {
        if (exception == null)
        {
            return;
        }

        MessageBox.Show($"{exception}", "UnhandledException");
    }
}
