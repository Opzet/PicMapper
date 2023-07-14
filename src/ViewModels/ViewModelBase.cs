using ReactiveUI;
using System.Text.Json;
using static MMKiwi.PicMapper.ViewModels.MainWindowViewModel;

namespace MMKiwi.PicMapper.ViewModels;

public abstract class ViewModelBase<TSettings> : ViewModelBase
{
    public abstract TSettings SaveSettings();

}

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    ViewModelActivator IActivatableViewModel.Activator { get; } = new();
}