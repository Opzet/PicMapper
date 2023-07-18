// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Avalonia.Threading;

using DynamicData;
using DynamicData.Binding;
using ExCSS;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers.Wms;
using Mapsui.Styles;

using MMKiwi.PicMapper.Gui.Avalonia.Services;
using MMKiwi.PicMapper.Models.Services;
using MMKiwi.PicMapper.ViewModels;
using ReactiveUI;

using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MMKiwi.PicMapper.Gui.Avalonia.Views;

public partial class ImageSelector : ReactiveUserControl<ImageSelectorViewModel>
{
    public ImageSelector()
    {
        InitializeComponent();
        PhotoList.Selection.SelectionChanged += UpdateSelectedImage;

        _ = this.WhenActivated(d =>
        {
            PhotoList.AddHandler(DragDrop.DropEvent, DropFiles);
            PhotoList.AddHandler(DragDrop.DragEnterEvent, StartDrop);
            PhotoList.AddHandler(DragDrop.DragLeaveEvent, EndDropDrop);

            MapView.Map.Layers.Add(Mapsui.Tiling.OpenStreetMap.CreateTileLayer("PicMapper"));

            if (ViewModel == null)
            {
                throw new InvalidOperationException("ViewModel is null");
            }

            ImageLayer = ReactiveObservableLayer.Create(GetFeature, ViewModel.MainWindow.ConnectImages(), "CurrentImages");
            ImageLayer.Style = CreateBitmapStyle();

            SelectedImageLayer = ReactiveObservableLayer.Create(GetFeature, ViewModel.ConnectSelectedImages(), "SelectedImages");
            SelectedImageLayer.Style = CreateSelectedBitmapStyle();

            MapView.Map.Layers.Add(ImageLayer);
            MapView.Map.Layers.Add(SelectedImageLayer);


            ViewModel.MainWindow.Images.ToObservableChangeSet().Subscribe((_) =>
            {
                ZoomExtents();
                MapView.Map.Refresh();
            });

            _ = ViewModel.WhenAnyValue(vm => vm.SelectedImage).Select(GetImage).Merge().BindTo(this, v => v.ImagePreview.Source).DisposeWith(d);
        });
    }

    public void ZoomIn() => MapView.Map.Navigator.ZoomIn();
    public void ZoomOut() => MapView.Map.Navigator.ZoomOut();
    public void ZoomExtents()
    {
        if (ImageLayer is not null)
        {
            MapView.Map.Navigator.ZoomToBox(ImageLayer.Extent);
            MapView.Map.Navigator.ZoomOut();
        }
    }

    public void StartDrop(object? sender, DragEventArgs eventArgs)
    {
        PhotoList.Classes.Add("DropHover");
    }

    public void EndDropDrop(object? sender, DragEventArgs eventArgs)
    {
        PhotoList.Classes.Remove("DropHover");
    }

    public async void DropFiles(object? sender, DragEventArgs eventArgs)
    {
        PhotoList.Classes.Remove("DropHover");

        IEnumerable<IStorageItem>? files = eventArgs.Data.GetFiles();

        if (files != null && ViewModel != null)
        {
            await Parallel.ForEachAsync(files, async (file, ct) =>
            {
                string? path = file.TryGetLocalPath();
                if (path != null)
                {
                    AvaloniaBitmapProvider bitmap = await AvaloniaBitmapProvider.LoadAsync(path);
                    Dispatcher.UIThread.Post(() => ViewModel.MainWindow.AddImage(bitmap));
                }
            });
        }
    }

    IObservable<object?> GetImage(IBitmapProvider? image) => image == null ? Observable.Return<object?>(null) : Observable.FromAsync(image.GetImageAsync);

    static PointFeature GetFeature(IBitmapProvider imageInfo)
    {
        (double x, double y) = SphericalMercator.FromLonLat(imageInfo.X ?? 0, imageInfo.Y ?? 0);
        return new PointFeature(x, y);
    }

    public ReactiveObservableLayer<IBitmapProvider>? ImageLayer { get; private set; }
    public ReactiveObservableLayer<IBitmapProvider>? SelectedImageLayer { get; private set; }

    private static SymbolStyle CreateBitmapStyle()
    {
        System.IO.Stream assets = AssetLoader.Open(new Uri("avares://MMKiwi.PicMapper.Gui.Avalonia/Assets/placemark.png"));

        int bitmapId = BitmapRegistry.Instance.Register(assets);

        double bitmapHeight = 30;
        return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 1, SymbolOffset = new Offset(0, bitmapHeight * 0.5) };
    }

    private static SymbolStyle CreateSelectedBitmapStyle()
    {
        System.IO.Stream assets = AssetLoader.Open(new Uri("avares://MMKiwi.PicMapper.Gui.Avalonia/Assets/yellowPlacemark.png"));

        int bitmapId = BitmapRegistry.Instance.Register(assets);

        double bitmapHeight = 30;
        return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 1, SymbolOffset = new Offset(0, bitmapHeight * 0.5) };
    }

    private void UpdateSelectedImage(object? sender, SelectionModelSelectionChangedEventArgs e)
    {
        if (ViewModel != null)
        {
            ViewModel.AddSelectedImages(e.SelectedItems.Cast<IBitmapProvider>());
            ViewModel.RemoveSelectedImages(e.DeselectedItems.Cast<IBitmapProvider>());
        }
    }
}