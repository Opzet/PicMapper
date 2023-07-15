// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.Platform.Storage;
using MMKiwi.PicMapper.ViewModels;
using MMKiwi.PicMapper.ViewModels.Services;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using XmpCore.Options;

namespace MMKiwi.PicMapper.Gui.Avalonia.Services;
public class DesktopSettingsProvider : ISettingsProvider
{
    public static async Task SaveMainWindowSettings(MainWindowViewModel viewModel)
    {
        await using FileStream jsonFile = File.OpenWrite(DataPath("main.json"));
        await JsonSerializer.SerializeAsync<MainWindowViewModel.Settings>(jsonFile, viewModel.SaveSettings(), ViewModelSerializer.Default.MainWindowViewModel);
    }

    Task ISettingsProvider.SaveMainWindowSettings(MMKiwi.PicMapper.ViewModels.MainWindowViewModel viewModel) => SaveMainWindowSettings(viewModel);

    public static async void LoadMainWindowSettings(MainWindowViewModel viewModel)
    {
        string jsonPath = DataPath("main.json");
        if (!File.Exists(jsonPath)) return;
        using FileStream jsonFile = File.OpenRead(jsonPath);
        MainWindowViewModel.Settings? settings = await JsonSerializer.DeserializeAsync(jsonFile, ViewModelSerializer.Default.MainWindowViewModel);
        settings?.LoadSettings(viewModel);
    }

    private static string DataPath(string fileName)
    {
        if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PicMapper")))
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PicMapper"));
        }

        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PicMapper", fileName);
    }

}
