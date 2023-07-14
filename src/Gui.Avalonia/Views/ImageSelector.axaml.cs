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

public partial class ImageSelector : ReactiveUserControl<ImageSelectorViewModel>
{
    public ImageSelector()
    {
        InitializeComponent();
        PhotoList.Selection.SelectionChanged += Selection_SelectionChanged;

        ImageLayer = new(GetFeature)
        {
            Style = CreateBitmapStyle()
        };

        this.WhenActivated(d =>
        {
            MapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer("PicMapper"));



            MapView.Map.Layers.Add(ImageLayer);

            if (ViewModel == null)
            {
                throw new InvalidOperationException("ViewModel is null");
            }

            ImageLayer.ObservableCollection = ViewModel.MainWindow.Images;
            ViewModel.MainWindow.Images.CollectionChanged += (s, e) =>
            {
                MapView.Map.Navigator.ZoomToBox(ImageLayer.Extent);
                MapView.Map.Refresh();

            };

            ViewModel.WhenAnyValue(vm => vm.SelectedImage).Select(GetImage).Merge().BindTo(this, v => v.ImagePreview.Source).DisposeWith(d);
        });
    }

    IObservable<object?> GetImage(IBitmapProvider? image)
    {
        if (image == null)
        {
            return Observable.Return<object?>(null);
        }
        else
        {
            return Observable.FromAsync(image.GetImageAsync);
        }
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