// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using System.Reactive.Disposables;

namespace MMKiwi.PicMapper.ViewModels;

public partial class OutputSettingsViewModel
{
    public override Settings SaveSettings() => new(SelectedFormat?.OutputFormat);

    public override void LoadSettings(Settings? settings)
    {
        if (settings != null)
        {
            SelectedFormat = Formats.First(f => f.OutputFormat == settings.OutputFormat);
        }
    }
    private void HookupSettings(CompositeDisposable d)
    {
        this.WhenAnyValue(vm => vm.SelectedFormat).Subscribe(_ => MainWindow.SettingsProvider.SaveOutputSettings(this)).DisposeWith(d);
    }
    public record class Settings(OutputFormat? OutputFormat);
}
