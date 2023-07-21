// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using MMKiwi.PicMapper.ViewModels.Services;
using MMKiwi.PicMapper.ViewModels.Design;

using ReactiveUI;

using System.Reactive.Disposables;

namespace MMKiwi.PicMapper.ViewModels;

public partial class KmlSettingsViewModel
{
    public override Settings SaveSettings() => new(Icon?.Key, EmbedThumbnail, EmbedFullImage);

    public override void LoadSettings(Settings? settings)
    {
        if (settings != null)
        {
            Icon = settings.Icon is null ? null : new(settings.Icon, ReadOnlyMemory<byte>.Empty);
            EmbedThumbnail = settings.EmbedThumbnail;
            EmbedFullImage = settings.EmbedFullImage;
        }
    }
    private void HookupSettings(CompositeDisposable d)
    {
        this.WhenAnyValue(vm => vm.Icon).Subscribe(_ => ParentViewModel.MainWindow.SettingsProvider.SaveKmlSettings(this)).DisposeWith(d);
        this.WhenAnyValue(vm => vm.EmbedThumbnail).Subscribe(_ => ParentViewModel.MainWindow.SettingsProvider.SaveKmlSettings(this)).DisposeWith(d);
        this.WhenAnyValue(vm => vm.EmbedFullImage).Subscribe(_ => ParentViewModel.MainWindow.SettingsProvider.SaveKmlSettings(this)).DisposeWith(d);
    }

    public record class Settings(string? Icon, bool EmbedThumbnail = true, bool EmbedFullImage = false);
}