// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using MMKiwi.PicMapper.Models.Services;
using MMKiwi.PicMapper.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace MMKiwi.PicMapper.Gui.Avalonia.Views;

public partial class OutputSettings : ReactiveUserControl<OutputSettingsViewModel>
{
    public OutputSettings()
    {
        InitializeComponent();
    }
}