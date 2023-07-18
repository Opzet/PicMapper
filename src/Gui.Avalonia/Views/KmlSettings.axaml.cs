// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using Avalonia.ReactiveUI;

using MMKiwi.PicMapper.ViewModels;
using System.Reactive.Disposables;

namespace MMKiwi.PicMapper.Gui.Avalonia.Views;
public partial class KmlSettings : ReactiveUserControl<KmlSettingsViewModel>
{
    public KmlSettings()
    {
        InitializeComponent();

        _ = this.WhenActivated(d =>
        {
            _ = this.Bind(ViewModel, vm => vm.Icon, v => v.SelectedIcon.SelectedItem,
                vmProp => SelectedIcon.Items.Cast<KmlIcon>().FirstOrDefault(key => key.Key == vmProp?.Key),
                vProp => vProp is KmlIcon icon ? new(icon.Key ?? "", icon.ToDataUri()) : null).DisposeWith(d);
        });
    }
}
