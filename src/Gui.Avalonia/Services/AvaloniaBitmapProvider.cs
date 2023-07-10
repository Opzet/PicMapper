using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;
using MMKiwi.PicMapper.Models.Services;
using System.IO;
using Avalonia.Media.Imaging;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace MMKiwi.PicMapper.Gui.Avalonia.Services;
public class AvaloniaBitmapProvider : IBitmapProvider
{
    private AvaloniaBitmapProvider(IStorageFile file, Bitmap thumbnail, double width, double height, string fileName, double? x, double? y)
    {
        StorageFile = file;
        Thumbnail = thumbnail;
        Width = width;
        Height = height;
        FileName = fileName;
        X = x;
        Y = y;
    }

    public static async Task<AvaloniaBitmapProvider> LoadAsync(IStorageFile file)
    {
        await using Stream imageStream = await file.OpenReadAsync();

        var thumbnail = Bitmap.DecodeToHeight(imageStream, 100);

        imageStream.Seek(0, SeekOrigin.Begin);

        GpsDirectory? gps = ImageMetadataReader.ReadMetadata(imageStream).OfType<GpsDirectory>().FirstOrDefault();

        GeoLocation? location = gps?.GetGeoLocation();

        return new(file, thumbnail, 0, 0, file.Path.Segments.Last(), location?.Longitude, location?.Latitude);
    }
    public double Width { get; }

    public double Height { get; }

    public string FileName { get; }

    public IStorageFile StorageFile { get; }

    object IBitmapProvider.Thumbnail => Thumbnail;

    public Bitmap Thumbnail { get; }

    public double? X { get; }

    public double? Y { get; }

    public async Task<Bitmap> GetImageAsync()
    {
        Stream imageStream = await StorageFile.OpenReadAsync();
        return new(imageStream);
    }

    async Task<object> IBitmapProvider.GetImageAsync() => await GetImageAsync();
}
