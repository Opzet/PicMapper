namespace MMKiwi.PicMapper.ViewModels.Services;
public interface ISettingsProvider
{
    Task SaveMainWindowSettings(MainWindowViewModel viewModel);
}
