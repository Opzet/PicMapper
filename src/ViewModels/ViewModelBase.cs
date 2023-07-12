using ReactiveUI;

namespace MMKiwi.PicMapper.ViewModels;

public class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    ViewModelActivator IActivatableViewModel.Activator { get; } = new();
}