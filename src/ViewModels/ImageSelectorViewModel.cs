// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using System.Reactive;
using MMKiwi.PicMapper.ViewModels.Services;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using System.Collections.ObjectModel;
using DynamicData;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using DynamicData.Binding;

namespace MMKiwi.PicMapper.ViewModels;
public partial class ImageSelectorViewModel : ViewModelBase, IRoutableViewModel
{
    public ImageSelectorViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;

        _ = this.ValidationRule(vm => vm.MainWindow.Images.Count, c => c > 0, "At least one image is required.");

        Next = ReactiveCommand.CreateFromTask(async () =>
        {
            OutputSettingsViewModel vm = new(MainWindow);
            await MainWindow.SettingsProvider.LoadOutputSettings(vm);
            return await MainWindow.Router.Navigate.Execute(vm);
        }, this.IsValid());

        var disposable = _selectedImagesSource.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(out _selectedImages).Subscribe();
        _selectedImage = SelectedImages.ObserveCollectionChanges().Select(_ => SelectedImages.FirstOrDefault()).ToProperty(this, vm => vm.SelectedImage);
        IObservable<bool> hasSelected = this.WhenAnyValue(vm => vm.SelectedImages.Count).Select(c => c > 0);
        _hasSelected = hasSelected.ToProperty(this, vm => vm.HasSelected);

        this.WhenActivated((CompositeDisposable d) =>
        {
            d.Add(disposable);
        });

        LoadImages = ReactiveCommand.CreateFromTask(LoadImagesAsync);
        RemoveImages = ReactiveCommand.Create(RemoveImagesImpl, hasSelected);

    }

    public MainWindowViewModel MainWindow { get; }

    private readonly ObservableAsPropertyHelper<IBitmapProvider?> _selectedImage;
    public IBitmapProvider? SelectedImage => _selectedImage.Value;

    private async Task LoadImagesAsync()
    {
        IAsyncEnumerable<IBitmapProvider> files = MainWindow.FileLoader.LoadImageAsync();
        await foreach (IBitmapProvider file in files)
        {
            MainWindow.AddImage(file);
        }
    }

    private void RemoveImagesImpl()
    {
        if (SelectedImages == null)
        {
            return;
        }

        MainWindow.RemoveImages(SelectedImages);
    }

    public void AddSelectedImages(IEnumerable<IBitmapProvider> images) => _selectedImagesSource.AddOrUpdate(images);
    public void RemoveSelectedImages(IEnumerable<IBitmapProvider> images) => _selectedImagesSource.Remove(images);

    private readonly SourceCache<IBitmapProvider, string> _selectedImagesSource = new(i => i.UniqueId);

    private ReadOnlyObservableCollection<IBitmapProvider> _selectedImages;
    public ReadOnlyObservableCollection<IBitmapProvider> SelectedImages => _selectedImages;

    public IObservable<IChangeSet<IBitmapProvider, string>> ConnectSelectedImages() => _selectedImagesSource.Connect();

    public ObservableAsPropertyHelper<bool> _hasSelected;
    public bool HasSelected => _hasSelected.Value;

    public ReactiveCommand<Unit, Unit> RemoveImages { get; }
    public ReactiveCommand<Unit, Unit> LoadImages { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> Next { get; }

    string? IRoutableViewModel.UrlPathSegment => nameof(ImageSelectorViewModel);

    IScreen IRoutableViewModel.HostScreen => MainWindow;

}
