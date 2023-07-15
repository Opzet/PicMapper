// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using System.Text.Json;
using static MMKiwi.PicMapper.ViewModels.MainWindowViewModel;

namespace MMKiwi.PicMapper.ViewModels;

public abstract class ViewModelBase<TSettings> : ViewModelBase
{
    public abstract TSettings SaveSettings();

}

public abstract class ViewModelBase : ReactiveObject, IActivatableViewModel
{
    ViewModelActivator IActivatableViewModel.Activator { get; } = new();
}