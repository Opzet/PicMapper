using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MMKiwi.PicMapper.ViewModels;
using MMKiwi.PicMapper.Gui.Avalonia.Services;
using MMKiwi.PicMapper.Gui.Avalonia.Views;
using MMKiwi.PicMapper.Models.Services;
using Splat;

namespace MMKiwi.PicMapper.Gui.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        Locator.CurrentMutable.RegisterLazySingleton<IFileLoader>(() => new AvaloniaFileLoader());

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            desktop.MainWindow = MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    public MainWindow? MainWindow { get; private set; }
}