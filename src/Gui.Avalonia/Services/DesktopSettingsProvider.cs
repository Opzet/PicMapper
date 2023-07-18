// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.Platform.Storage;

using MMKiwi.PicMapper.ViewModels;
using MMKiwi.PicMapper.ViewModels.Services;

using Nito.AsyncEx;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

using XmpCore.Options;

namespace MMKiwi.PicMapper.Gui.Avalonia.Services;
public class DesktopSettingsProvider : ISettingsProvider
{
    static private readonly SemaphoreSlim _syncRoot = new(1);

    static AsyncLazy<ViewModelSerializer.ConfigurationRoot> RootConfig { get; } = new(LoadCurrentConfigurationAsync);


    public static async Task SaveOutputSettings(OutputSettingsViewModel viewModel)
    {
        await _syncRoot.WaitAsync();
        try
        {
            ViewModelSerializer.ConfigurationRoot rootConfig = await RootConfig.Task;

            rootConfig.OutputSettings = viewModel.SaveSettings();

            await using var jsonOut = File.Open(DataPath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(jsonOut, rootConfig, ViewModelSerializer.Default.Root);
        }
        finally
        {
            _ = _syncRoot.Release();
        }
    }

    public static async Task LoadOutputSettings(OutputSettingsViewModel viewModel)
    {
        await _syncRoot.WaitAsync();
        try
        {
            ViewModelSerializer.ConfigurationRoot rootConfig = await RootConfig.Task;

            if (rootConfig?.OutputSettings != null)
                viewModel.LoadSettings(rootConfig.OutputSettings);
        }
        finally
        {
            _ = _syncRoot.Release();
        }
    }


    public static async Task SaveKmlSettings(KmlSettingsViewModel viewModel)
    {
        await _syncRoot.WaitAsync();
        try
        {
            ViewModelSerializer.ConfigurationRoot rootConfig = await RootConfig.Task;

            rootConfig.Kml = viewModel.SaveSettings();

            await using var jsonOut = File.Open(DataPath, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(jsonOut, rootConfig, ViewModelSerializer.Default.Root);
        }
        finally
        {
            _ =_syncRoot.Release();
        }
    }

    public static async Task LoadKmlSettings(KmlSettingsViewModel viewModel)
    {
        await _syncRoot.WaitAsync();
        try
        {
            ViewModelSerializer.ConfigurationRoot rootConfig = await RootConfig.Task;

            if (rootConfig?.Kml != null)
                viewModel.LoadSettings(rootConfig.Kml);
        }
        finally
        {
            _syncRoot.Release();
        }
    }

    private static async Task<ViewModelSerializer.ConfigurationRoot> LoadCurrentConfigurationAsync()
    {
        if (!File.Exists(DataPath)) return new();

        ViewModelSerializer.ConfigurationRoot? rootConfig = null;

        await using FileStream jsonIn = File.Open(DataPath, FileMode.Open);
        if (jsonIn.Length > 0)
        {
            try
            {
                rootConfig = await JsonSerializer.DeserializeAsync(jsonIn, ViewModelSerializer.Default.Root);
            }
            catch
            {
#warning TODO warn that we could not load previous settings
            }
        }
        return rootConfig ?? new();
    }

    Task ISettingsProvider.SaveOutputSettings(OutputSettingsViewModel viewModel) => SaveOutputSettings(viewModel);
    Task ISettingsProvider.LoadOutputSettings(OutputSettingsViewModel viewModel) => LoadOutputSettings(viewModel);
    Task ISettingsProvider.SaveKmlSettings(KmlSettingsViewModel viewModel) => SaveKmlSettings(viewModel);
    Task ISettingsProvider.LoadKmlSettings(KmlSettingsViewModel viewModel) => LoadKmlSettings(viewModel);

    private static string DataPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PicMapper.settings.json");
}
