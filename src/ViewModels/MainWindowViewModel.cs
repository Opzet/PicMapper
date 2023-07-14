using ReactiveUI;
using MMKiwi.PicMapper.Models.Services;
using System.Collections.ObjectModel;
using MMKiwi.PicMapper.ViewModels.Services;
using System.Reactive;

namespace MMKiwi.PicMapper.ViewModels;

public partial class MainWindowViewModel : ViewModelBase<MainWindowViewModel.Settings>, IScreen
{
    public MainWindowViewModel(IFileLoader fileLoader, ISettingsProvider settingsProvider)
    {
        FileLoader = fileLoader;
        SettingsProvider = settingsProvider;
        this.WhenActivated(d =>
        {
            Router.Navigate.Execute(new ImageSelectorViewModel(this));
            HookupSettings(d);
        });
    }

    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; } = new RoutingState();


    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack => Router.NavigateBack;


    public ObservableCollection<IBitmapProvider> Images { get; } = new();

    public OutputFormat SelectedOutputFormat {
        get => _selectedOutputFormat;
        set => this.RaiseAndSetIfChanged(ref _selectedOutputFormat, value);
    }
    private OutputFormat _selectedOutputFormat;

    public IFileLoader FileLoader { get; }
    public ISettingsProvider SettingsProvider { get; }
}
