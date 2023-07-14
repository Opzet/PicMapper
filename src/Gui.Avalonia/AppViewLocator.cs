using MMKiwi.PicMapper.Gui.Avalonia.Views;
using MMKiwi.PicMapper.ViewModels;
using ReactiveUI;

namespace MMKiwi.PicMapper.Gui.Avalonia;

public class AppViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        ImageSelectorViewModel context => new ImageSelector { ViewModel = context },
        OutputSettingsViewModel context => new OutputSettings { ViewModel = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}