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
        PhotoList.Selection.SelectionChanged += Selection_SelectionChanged;
        MapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer("PicMapper"));

        ImageLayer = new(GetFeature)
        {
            Style = CreateBitmapStyle()
        };

        MapView.Map.Layers.Add(ImageLayer);

        this.WhenActivated(d =>
        {
            if (ViewModel == null)
            {
                throw new InvalidOperationException();
            }

            ImageLayer.ObservableCollection = ViewModel.Images;
            ViewModel.Images.CollectionChanged += (s, e) =>
            {
                MapView.Map.Navigator.ZoomToBox(ImageLayer.Extent);
                MapView.Map.Refresh();
            };
        });
    }

    PointFeature GetFeature(IBitmapProvider imageInfo)
    {
        (double x, double y) = SphericalMercator.FromLonLat(imageInfo.X ?? 0, imageInfo.Y ?? 0);
        return new PointFeature(x, y);
    }

    public ObservableMemoryLayer<IBitmapProvider> ImageLayer { get; }

    private static SymbolStyle CreateBitmapStyle()
    {
        System.IO.Stream assets = AssetLoader.Open(new Uri("avares://MMKiwi.PicMapper.Gui.Avalonia/Assets/placemark.png"));

        int bitmapId = BitmapRegistry.Instance.Register(assets);

        double bitmapHeight = 30;
        return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 1, SymbolOffset = new Offset(0, bitmapHeight * 0.5) };
    }

    private void Selection_SelectionChanged(object? sender, SelectionModelSelectionChangedEventArgs e)
    {
        ViewModel!.SelectedImages = e.SelectedItems.Cast<IBitmapProvider>();
    }
}