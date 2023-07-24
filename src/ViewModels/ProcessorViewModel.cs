// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;

using System.ComponentModel;

namespace MMKiwi.PicMapper.ViewModels;
public class ProcessorViewModel : ViewModelBase, IRoutableViewModel
{
    public ProcessorViewModel(MainWindowViewModel mainWindowViewModel, IFormatProcessor processor)
    {
        MainWindow = mainWindowViewModel;
        Processor = processor;
    }

    public MainWindowViewModel MainWindow { get; }
    public IFormatProcessor Processor { get; }

    string? IRoutableViewModel.UrlPathSegment => nameof(ProcessorViewModel);

    IScreen IRoutableViewModel.HostScreen => MainWindow;
}