// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using MMKiwi.PicMapper.Models.Services;
using MMKiwi.PicMapper.ViewModels.Services;
using System.Linq;

namespace MMKiwi.PicMapper.ViewModels.Design;

public class MainWindowViewModel : ViewModels.MainWindowViewModel
{
    public MainWindowViewModel() : base(new FileLoader(), new SettingsProvider())
    {
    }
}

public class SettingsProvider : ISettingsProvider
{
    public Task SaveMainWindowSettings(ViewModels.MainWindowViewModel viewModel) => Task.CompletedTask;
}

public class FileLoader : IFileLoader
{
    public IAsyncEnumerable<IBitmapProvider> LoadImageAsync() => AsyncEnumerable.Empty<IBitmapProvider>();
}

public class ImageSelectorViewModel : ViewModels.ImageSelectorViewModel
{
    public ImageSelectorViewModel() : base(new MainWindowViewModel()) { }
}

public class OutputSettingsViewModel: ViewModels.OutputSettingsViewModel
{
    public OutputSettingsViewModel() : base(new MainWindowViewModel()) { }
}