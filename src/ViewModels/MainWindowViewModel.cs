// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using MMKiwi.PicMapper.Models.Services;
using System.Collections.ObjectModel;
using MMKiwi.PicMapper.ViewModels.Services;
using System.Reactive;
using System.Reactive.Disposables;
using DynamicData;
using DynamicData.Binding;
using System.Reactive.Linq;

namespace MMKiwi.PicMapper.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IScreen
{
    public MainWindowViewModel(IFileLoader fileLoader, ISettingsProvider settingsProvider)
    {
        FileLoader = fileLoader;
        SettingsProvider = settingsProvider;
        var disposable = _imageSource.Connect().ObserveOn(RxApp.MainThreadScheduler).Bind(out _images).Subscribe();

        this.WhenActivated((CompositeDisposable d) =>
        {
            Router.Navigate.Execute(new ImageSelectorViewModel(this));
            d.Add(disposable);
        });
    }

    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; } = new RoutingState();

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack => Router.NavigateBack;

    private readonly SourceCache<IBitmapProvider, string> _imageSource = new(image => image.UniqueId);
    private readonly ReadOnlyObservableCollection<IBitmapProvider> _images;

    public void AddImage(IBitmapProvider image) => _imageSource.AddOrUpdate(image);
    public void AddImages(IEnumerable<IBitmapProvider> image) => _imageSource.Edit(c => _imageSource.AddOrUpdate(image));
    public void RemoveImage(IBitmapProvider image) => _imageSource.Remove(image);
    public void RemoveImages(IEnumerable<IBitmapProvider> image) => _imageSource.Edit(c=>c.Remove(image));

    public IObservable<IChangeSet<IBitmapProvider, string>> ConnectImages() => _imageSource.Connect();

    public ReadOnlyObservableCollection<IBitmapProvider> Images => _images;
    public IFileLoader FileLoader { get; }
    public ISettingsProvider SettingsProvider { get; }
}
