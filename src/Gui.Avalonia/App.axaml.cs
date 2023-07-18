// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MMKiwi.PicMapper.ViewModels;
using MMKiwi.PicMapper.Gui.Avalonia.Services;
using MMKiwi.PicMapper.Gui.Avalonia.Views;

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