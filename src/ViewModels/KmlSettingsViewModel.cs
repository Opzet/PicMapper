// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using MMKiwi.PicMapper.ViewModels.Services;

using ReactiveUI;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

using static MMKiwi.PicMapper.ViewModels.KmlSettingsViewModel;

namespace MMKiwi.PicMapper.ViewModels;

public partial class KmlSettingsViewModel : ViewModelBase<Settings>, IOutputSettingsViewModel, IFormatViewModel
{
    private IconInfo? _icon;
    private bool _embedThumbnail = true;
    private bool _embedFullImage = false;

    public KmlSettingsViewModel(OutputSettingsViewModel outputSettingsViewModel)
    {
        ParentViewModel = outputSettingsViewModel;

        // Creates the validation for the Name property.
        _ = this.ValidationRule(viewModel => viewModel.Icon,
                                icon => !string.IsNullOrEmpty(icon?.Key),
                                "You must select an icon");

        this.WhenActivated(d => HookupSettings(d));
    }

    public OutputSettingsViewModel ParentViewModel { get; }

    public bool EmbedThumbnail
    {
        get => _embedThumbnail;
        set => this.RaiseAndSetIfChanged(ref _embedThumbnail, value);
    }
    public bool EmbedFullImage
    {
        get => _embedFullImage;
        set => this.RaiseAndSetIfChanged(ref _embedFullImage, value);

    }

    public IconInfo? Icon
    {
        get => _icon;
        set => this.RaiseAndSetIfChanged(ref _icon, value);
    }

    public IFormatProcessor CreateProcessor()
    {
        throw new NotImplementedException();
    }
}