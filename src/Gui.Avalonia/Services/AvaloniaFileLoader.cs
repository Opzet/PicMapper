// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.Controls;
using Avalonia.Platform.Storage;

using MMKiwi.PicMapper.ViewModels.Services;

namespace MMKiwi.PicMapper.Gui.Avalonia.Services;
internal class AvaloniaFileLoader : IFileLoader
{
    public async Task<IFileHandle?> BrowseForFileAsync(string description, IReadOnlyList<string> extensions)
    {
        TopLevel topLevel = TopLevel.GetTopLevel(((App?)global::Avalonia.Application.Current)!.MainWindow)!;
        IStorageFile? file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Output file",
            FileTypeChoices = new FilePickerFileType[]
            {
                new FilePickerFileType(description)
                {
                    Patterns = extensions
                },
                FilePickerFileTypes.All
            }
        });

        return file != null ? new AvaloniaFileHandle(file) : (IFileHandle?)null;
    }

    public async IAsyncEnumerable<IBitmapProvider> LoadImageAsync()
    {
        TopLevel topLevel = TopLevel.GetTopLevel(((App?)global::Avalonia.Application.Current)!.MainWindow)!;
        IReadOnlyList<IStorageFile> images = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Select an image",
            AllowMultiple = true,
            FileTypeFilter = new FilePickerFileType[] { FilePickerFileTypes.ImageAll, FilePickerFileTypes.All }
        });

        foreach (IStorageFile image in images)
        {
            yield return await AvaloniaBitmapProvider.LoadAsync(image);
        }
    }

    public class LocalFileHandle:IFileHandle
    {
        public string FilePath { get; }

        public LocalFileHandle(string filePath)
        {
            FilePath = filePath;
        }

        public ValueTask<Stream> GetWriteStream() => ValueTask.FromResult(File.OpenWrite(FilePath) as Stream);
    }

    /// <summary>
    /// Not currently used; will need to use this for non-desktop environments.
    /// </summary>
    public class AvaloniaFileHandle : IFileHandle
    {
        public IStorageFile StorageFile { get; }

        public string FilePath => StorageFile.TryGetLocalPath() ?? StorageFile.Path.ToString();

        public AvaloniaFileHandle(IStorageFile storageFile)
        {
            StorageFile = storageFile;
        }

        public async ValueTask<Stream> GetWriteStream() => await StorageFile.OpenWriteAsync();
    }
}
