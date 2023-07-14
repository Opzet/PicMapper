using MMKiwi.PicMapper.Models.Services;
using MMKiwi.PicMapper.ViewModels.Services;
using System.Linq;

namespace MMKiwi.PicMapper.ViewModels.Design;

public class MainWindowViewModel : ViewModels.MainWindowViewModel
{
    public MainWindowViewModel() : base(new FileLoader(), new SettingsProvider())
    {
    }
}

public class SettingsProvider : ISettingsProvider
{
    public Task SaveMainWindowSettings(ViewModels.MainWindowViewModel viewModel) => Task.CompletedTask;
}

public class FileLoader : IFileLoader
{
    public IAsyncEnumerable<IBitmapProvider> LoadImageAsync() => AsyncEnumerable.Empty<IBitmapProvider>();
}

public class ImageSelectorViewModel : ViewModels.ImageSelectorViewModel
{
    public ImageSelectorViewModel() : base(new MainWindowViewModel()) { }
}

public class OutputSettingsViewModel: ViewModels.OutputSettingsViewModel
{
    public OutputSettingsViewModel() : base(new MainWindowViewModel()) { }
}