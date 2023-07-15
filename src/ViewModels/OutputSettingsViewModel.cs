// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using ReactiveUI;
using System.Collections.Immutable;
using System.Reactive;

namespace MMKiwi.PicMapper.ViewModels;

public partial class OutputSettingsViewModel : ViewModelBase<MainWindowViewModel.Settings>, IRoutableViewModel
{
    public OutputSettingsViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;
        Back = ReactiveCommand.CreateFromObservable(() => MainWindow.Router.Navigate.Execute(new ImageSelectorViewModel(MainWindow)));
    }

    public ReactiveCommand<Unit, IRoutableViewModel> Back { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> Next { get; }


    public MainWindowViewModel MainWindow { get; }

    string? IRoutableViewModel.UrlPathSegment => nameof(OutputSettingsViewModel);
    IScreen IRoutableViewModel.HostScreen => MainWindow;
    public override MainWindowViewModel.Settings SaveSettings() => MainWindow.SaveSettings();

    public FormatInfo[] Formats { get; } = { new(OutputFormat.KML, "KML/KMZ", new string[] { "kml", "kmz" }) };
}

public record class FormatInfo(OutputFormat OutputFormat, string DisplayName, string[] Extensions)
{
    public override string ToString() => $"{DisplayName} ({string.Join(", ", Extensions)})";
}