// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.ReactiveUI;
using ReactiveUI;
using MMKiwi.PicMapper.ViewModels;
using System.Reactive.Linq;
using Avalonia.Controls.Selection;
using Mapsui.Layers;
using Mapsui.Styles;
using Avalonia.Platform;
using Mapsui.Projections;
using MMKiwi.PicMapper.Models.Services;

namespace MMKiwi.PicMapper.Gui.Avalonia.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

    }


}