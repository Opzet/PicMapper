// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MMKiwi.PicMapper.Models.Services;

namespace MMKiwi.PicMapper.Gui.Avalonia.Services;
internal class AvaloniaFileLoader : IFileLoader
{
    public async IAsyncEnumerable<IBitmapProvider> LoadImageAsync()
    {
        TopLevel topLevel = TopLevel.GetTopLevel(((App?)global::Avalonia.Application.Current)!.MainWindow)!;
        IReadOnlyList<IStorageFile> images = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select an image",
            AllowMultiple = true,
            FileTypeFilter = new List<FilePickerFileType> { FilePickerFileTypes.ImageAll, FilePickerFileTypes.All }
        });

        foreach (IStorageFile image in images)
        {
            yield return await AvaloniaBitmapProvider.LoadAsync(image);
        }
    }
}
