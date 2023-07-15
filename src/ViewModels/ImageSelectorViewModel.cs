// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using System.Reactive;
using MMKiwi.PicMapper.Models.Services;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace MMKiwi.PicMapper.ViewModels;
public partial class ImageSelectorViewModel : ViewModelBase<MainWindowViewModel.Settings>, IRoutableViewModel
{
    public ImageSelectorViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;

        LoadImages = ReactiveCommand.CreateFromTask(LoadImagesAsync);

        IObservable<bool> hasSelected = this.WhenAnyValue(vm => vm.SelectedImages).Select(coll => coll != null && coll.Any());
        _hasSelected = hasSelected.ToProperty(this, vm => vm.HasSelected);
        RemoveImages = ReactiveCommand.Create(RemoveImagesImpl, hasSelected);

        _selectedImage = this.WhenAnyValue(vm => vm.SelectedImages).Select(image => image?.FirstOrDefault()).ToProperty(this, vm => vm.SelectedImage);

        IObservable<bool> hasImages = MainWindow.WhenAnyValue(mw => mw.Images.Count).Select(c => c > 0);
        Next = ReactiveCommand.CreateFromObservable(() => MainWindow.Router.Navigate.Execute(new OutputSettingsViewModel(MainWindow)), hasImages);

    }

    public MainWindowViewModel MainWindow { get; }

    public override MainWindowViewModel.Settings SaveSettings() => MainWindow.SaveSettings();


    private readonly ObservableAsPropertyHelper<IBitmapProvider?> _selectedImage;
    public IBitmapProvider? SelectedImage => _selectedImage.Value;

    private async Task LoadImagesAsync()
    {
        IAsyncEnumerable<IBitmapProvider> files = MainWindow.FileLoader.LoadImageAsync();
        await foreach (IBitmapProvider file in files)
        {
            MainWindow.Images.Add(file);
        }
    }

    private void RemoveImagesImpl()
    {
        if (SelectedImages == null)
        {
            return;
        }

        foreach (object selected in SelectedImages)
        {
            if (selected is IBitmapProvider selectedImage)
            {
                MainWindow.Images.Remove(selectedImage);
            }
        }
    }

    private IEnumerable<IBitmapProvider>? _selectedImages;

    public IEnumerable<IBitmapProvider>? SelectedImages {
        get => _selectedImages;
        set {
            _selectedImages = value;
            this.RaisePropertyChanged();
        }
    }

    public ObservableAsPropertyHelper<bool> _hasSelected;
    public bool HasSelected => _hasSelected.Value;

    public ReactiveCommand<Unit, Unit> RemoveImages { get; }
    public ReactiveCommand<Unit, Unit> LoadImages { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> Next { get; }

    string? IRoutableViewModel.UrlPathSegment => nameof(ImageSelectorViewModel);

    IScreen IRoutableViewModel.HostScreen => MainWindow;
}
