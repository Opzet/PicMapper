using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MMKiwi.PicMapper.ViewModels;
using MMKiwi.PicMapper.Gui.Avalonia.Services;
using MMKiwi.PicMapper.Gui.Avalonia.Views;
using MMKiwi.PicMapper.Models.Services;
using Splat;
using MMKiwi.PicMapper.ViewModels.Services;

namespace MMKiwi.PicMapper.Gui.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    DesktopSettingsProvider SettingsProvider { get; } = new();
    AvaloniaFileLoader FileLoader { get; } = new();

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MainWindowViewModel vm = new(FileLoader, SettingsProvider);
            DesktopSettingsProvider.LoadMainWindowSettings(vm);

            MainWindow = new MainWindow
            {
                DataContext = vm,
            };
            desktop.MainWindow = MainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    public MainWindow? MainWindow { get; private set; }
}