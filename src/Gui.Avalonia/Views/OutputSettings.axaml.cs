using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using MMKiwi.PicMapper.Models.Services;
using MMKiwi.PicMapper.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MMKiwi.PicMapper.Gui.Avalonia.Views;

public partial class OutputSettings : ReactiveUserControl<OutputSettingsViewModel>
{
    public OutputSettings()
    {
        InitializeComponent();
    }
}