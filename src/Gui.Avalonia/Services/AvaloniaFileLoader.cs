using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using MMKiwi.PicMapper.Models.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
