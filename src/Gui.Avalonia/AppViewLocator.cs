// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using MMKiwi.PicMapper.Gui.Avalonia.Views;
using MMKiwi.PicMapper.ViewModels;
using ReactiveUI;

namespace MMKiwi.PicMapper.Gui.Avalonia;

public class AppViewLocator : IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        ImageSelectorViewModel context => new ImageSelector { ViewModel = context },
        OutputSettingsViewModel context => new OutputSettings { ViewModel = context },
        KmlSettingsViewModel context => new KmlSettings { ViewModel = context },
        ProcessorViewModel context => new Processor { ViewModel = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
    };
}