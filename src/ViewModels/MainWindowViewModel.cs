// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using MMKiwi.PicMapper.Models.Services;
using System.Collections.ObjectModel;
using MMKiwi.PicMapper.ViewModels.Services;
using System.Reactive;
using System.Reactive.Disposables;

namespace MMKiwi.PicMapper.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IScreen
{
    public MainWindowViewModel(IFileLoader fileLoader, ISettingsProvider settingsProvider)
    {
        FileLoader = fileLoader;
        SettingsProvider = settingsProvider;
        this.WhenActivated((CompositeDisposable d) => Router.Navigate.Execute(new ImageSelectorViewModel(this)));
    }

    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; } = new RoutingState();

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack => Router.NavigateBack;

    public ObservableCollection<IBitmapProvider> Images { get; } = new();

    public IFileLoader FileLoader { get; }
    public ISettingsProvider SettingsProvider { get; }
}
