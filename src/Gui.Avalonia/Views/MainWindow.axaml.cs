using Avalonia.ReactiveUI;
using ReactiveUI;
using MMKiwi.PicMapper.ViewModels;
using System.Reactive.Linq;
using Avalonia.Controls.Selection;
using Mapsui.Layers;
using Mapsui.Styles;
using Avalonia.Platform;
using Mapsui.Projections;
using MMKiwi.PicMapper.Models.Services;

namespace MMKiwi.PicMapper.Gui.Avalonia.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

    }


}