using ReactiveUI;
using System.Reactive;
using Splat;
using MMKiwi.PicMapper.Models.Services;
using MMKiwi.PicMapper.Models;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace MMKiwi.PicMapper.ViewModels;

public class MainWindowViewModel : ViewModelBase
{


    public MainWindowViewModel()
    {
        LoadImages = ReactiveCommand.CreateFromTask(LoadImagesAsync);
        IObservable<bool> hasSelected = this.WhenAnyValue(vm => vm.SelectedImages).Select(coll => coll != null && coll.Any());
        _hasSelected = hasSelected.ToProperty(this, vm => vm.HasSelected);
        RemoveImages = ReactiveCommand.Create(RemoveImagesImpl, hasSelected);
    }

    private async Task LoadImagesAsync()
    {
        IFileLoader fileLoader = Locator.Current.GetService<IFileLoader>() ?? throw new InvalidOperationException("Missing IFileLoader<TImage>");
        IAsyncEnumerable<IBitmapProvider> files = fileLoader.LoadImageAsync();
        await foreach (IBitmapProvider file in files)
        {
            Images.Add(file);
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
                Images.Remove(selectedImage);
            }
        }
    }

    public ObservableCollection<IBitmapProvider> Images { get; } = new();

    private IEnumerable<IBitmapProvider>? _selectedImages;
    public IEnumerable<IBitmapProvider>? SelectedImages {
        get => _selectedImages;
        set {
            _selectedImages = value;
            this.RaisePropertyChanged();
        }
    }

    public ObservableAsPropertyHelper<bool> _hasSelected;

    public ReactiveCommand<Unit, Unit> RemoveImages { get; }

    public bool HasSelected => _hasSelected.Value;

    public ReactiveCommand<Unit, Unit> LoadImages { get; }
}
