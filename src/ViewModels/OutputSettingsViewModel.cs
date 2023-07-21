// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using MMKiwi.PicMapper.ViewModels.Services;

using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MMKiwi.PicMapper.ViewModels;

public partial class OutputSettingsViewModel : ViewModelBase<OutputSettingsViewModel.Settings>, IRoutableViewModel
{
    public OutputSettingsViewModel(MainWindowViewModel mainWindow)
    {
        MainWindow = mainWindow;

        KmlSettingsViewModel kml = new(this);
        MainWindow.SettingsProvider.LoadKmlSettings(kml);

        Formats = new FormatInfo[] {
            new FormatInfo(OutputFormat.KML, "KMZ/KML", new string[] { "*.kmz", "*.kml" }, kml)
        };


        Back = ReactiveCommand.CreateFromObservable(() => MainWindow.Router.Navigate.Execute(new ImageSelectorViewModel(MainWindow)));

        _formatViewModel = this.WhenAnyValue(vm => vm.SelectedFormat).Select(sf => sf?.ViewModel).ToProperty(this, vm => vm.FormatViewModel);

        ValidationHelper selectedValid = this.ValidationRule(vm => vm.SelectedFormat, sf => sf is not null, "Please select and output format");
        _ = this.ValidationRule(vm => vm.OutputPath, outPath => outPath != null, "Please select an output path");
        _ = this.ValidationRule(this.WhenAnyValue(vm => vm.SelectedFormat).Select(sf => sf?.ViewModel.ValidationContext.Valid ?? Observable.Return(false)).Merge(), "The format settings are invalid");

        Next = ReactiveCommand.CreateFromObservable(() => MainWindow.Router.Navigate.Execute(new ProcessorViewModel(MainWindow, SelectedFormat!.ViewModel.CreateProcessor())), this.IsValid());

        this.WhenActivated(d =>
        {
            HookupSettings(d);
        });

        Browse = ReactiveCommand.CreateFromTask(BrowseImpl, selectedValid.ValidationChanged.Select(v => v.IsValid));
    }

    public ReactiveCommand<Unit, IRoutableViewModel> Back { get; }
    public ReactiveCommand<Unit, IRoutableViewModel> Next { get; }

    public ReactiveCommand<Unit, Unit> Browse { get; }
    private async Task BrowseImpl()
    {
        if (SelectedFormat != null)
        {
            IFileHandle? fileHandle = await MainWindow.FileLoader.BrowseForFileAsync(SelectedFormat.DisplayName, SelectedFormat.Extensions);
            if (fileHandle != null)
                OutputPath = fileHandle;
        }
    }

    private FormatInfo? _selectedFormat;

    public FormatInfo? SelectedFormat
    {
        get => _selectedFormat;
        set => this.RaiseAndSetIfChanged(ref _selectedFormat, value);
    }
    private IFileHandle? _outputPath;
    public IFileHandle? OutputPath
    {
        get => _outputPath;
        set => this.RaiseAndSetIfChanged(ref _outputPath, value);
    }

    public MainWindowViewModel MainWindow { get; }

    readonly ObservableAsPropertyHelper<IOutputSettingsViewModel?> _formatViewModel;
    public IOutputSettingsViewModel? FormatViewModel => _formatViewModel.Value;

    string? IRoutableViewModel.UrlPathSegment => nameof(OutputSettingsViewModel);
    IScreen IRoutableViewModel.HostScreen => MainWindow;

    public FormatInfo[] Formats { get; }
}
