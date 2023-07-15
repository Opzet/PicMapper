// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using System.Reactive.Disposables;

namespace MMKiwi.PicMapper.ViewModels;

public partial class MainWindowViewModel
{
    public record class Settings(OutputFormat SelectedOutputFormat)
    {
        public void LoadSettings(MainWindowViewModel viewModel)
        {
            viewModel.SelectedOutputFormat = SelectedOutputFormat;
        }
    }
    public override Settings SaveSettings() => new(SelectedOutputFormat);

    private void HookupSettings(CompositeDisposable d)
    {
        this.WhenAnyValue(vm => vm.SelectedOutputFormat).Subscribe(_ => SettingsProvider.SaveMainWindowSettings(this)).DisposeWith(d);
    }
}
