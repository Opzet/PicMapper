// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.Platform.Storage;
using MMKiwi.PicMapper.ViewModels.Services;
using Avalonia.Media.Imaging;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Directory = MetadataExtractor.Directory;

namespace MMKiwi.PicMapper.Gui.Avalonia.Services;
public abstract class AvaloniaBitmapProvider : IBitmapProvider
{
    public static async Task<AvaloniaBitmapProvider> LoadAsync(IStorageFile file)
    {
        await using Stream imageStream = await file.OpenReadAsync();
        (var mimeType, var thumbnail, var location) = Load(imageStream);

        string filePath = file.TryGetLocalPath() ?? file.Path.ToString();

        return new StorageFileProvider
        {
            StorageFile = file,
            UniqueId = filePath,
            FileName = Path.GetFileName(filePath),
            Height = 0,
            Width = 0,
            MimeType = mimeType,
            Thumbnail = thumbnail,
            X = location?.Longitude,
            Y = location?.Latitude
        };
    }

    public static async Task<AvaloniaBitmapProvider> LoadAsync(string filePath)
    {
        await using Stream imageStream = File.OpenRead(filePath);
        (var mimeType, var thumbnail, var location) = Load(imageStream);

        return new LocalFileProvider
        {
            FilePath = filePath,
            UniqueId = filePath,
            FileName = Path.GetFileName(filePath),
            Height = 0,
            Width = 0,
            MimeType = mimeType,
            Thumbnail = thumbnail,
            X = location?.Longitude,
            Y = location?.Latitude
        };
    }

    private static (string MimeType, Bitmap Thumbnail, GeoLocation? location) Load(Stream imageStream)
    {
        var mimeType = DetectMimeType(imageStream);
        imageStream.Seek(0, SeekOrigin.Begin);
        var thumbnail = Bitmap.DecodeToHeight(imageStream, 100);
        imageStream.Seek(0, SeekOrigin.Begin);

        IReadOnlyList<Directory> metadata = ImageMetadataReader.ReadMetadata(imageStream);

        GpsDirectory? gps = metadata.OfType<GpsDirectory>().FirstOrDefault();
        var location = gps?.GetGeoLocation();

        return (mimeType, thumbnail, location);
    }

    private class StorageFileProvider : AvaloniaBitmapProvider
    {
        public required IStorageFile StorageFile { get; init; }

        protected override Task<Stream> OpenReadAsync() => StorageFile.OpenReadAsync();

        protected override Task<Stream> OpenWriteAsync() => StorageFile.OpenWriteAsync();
    }

    private class LocalFileProvider : AvaloniaBitmapProvider
    {
        public required string FilePath { get; init; }

        protected override Task<Stream> OpenReadAsync() => Task.FromResult<Stream>(File.OpenRead(FilePath));

        protected override Task<Stream> OpenWriteAsync() => Task.FromResult<Stream>(File.OpenWrite(FilePath));
    }

    public required double Width { get; init; }

    public required double Height { get; init; }

    public required string FileName { get; init; }

    object IBitmapProvider.Thumbnail => Thumbnail;

    public required Bitmap Thumbnail { get; init; }

    public required double? X { get;init; }

    public required double? Y { get; init; }
    public required string MimeType { get; init;  }
    public required string UniqueId { get; init; }
    public virtual string ThumbnailMimeType => "image/png";

    public override string ToString() => $"Image: {FileName}";
    public async Task<Bitmap> GetImageAsync()
    {
        Stream imageStream = await OpenReadAsync();
        return new(imageStream);
    }

    async Task<object> IBitmapProvider.GetImageAsync() => await GetImageAsync();

    public byte[] GetThumbnail()
    {
        using MemoryStream ms = new();
        Thumbnail.Save(ms);
        return ms.ToArray();
    }

    public async Task CopyImageAsync(Stream destination)
    {
        await using Stream imageStream = await OpenReadAsync();
        await imageStream.CopyToAsync(destination);
    }

    private static string DetectMimeType(Stream imageFile)
    {
        Span<byte> header = stackalloc byte[16];
        imageFile.ReadExactly(header);

        return header switch
        {
            var h when h[..2].SequenceEqual(FileSignatures.Jpeg) => "image/jpg",
            var h when h.Slice(4, 12).SequenceEqual(FileSignatures.Heic) => "image/heic",
            var h when h[..8].SequenceEqual(FileSignatures.Png) => "image/png",
            var h when h[..4].SequenceEqual(FileSignatures.WebPStart)
                    && h.Slice(8, 4).SequenceEqual(FileSignatures.WebPEnd) => "image/webp",
            _ => throw new InvalidDataException("Unsupported image format. Image must be PNG, JPEG, HEIC, or WEBP")
        };
    }

    private static class FileSignatures
    {
        public static ReadOnlySpan<byte> Png => new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        public static ReadOnlySpan<byte> Jpeg => new byte[] { 0xFF, 0xD8 };
        public static ReadOnlySpan<byte> Heic => "ftypheic"u8;
        public static ReadOnlySpan<byte> WebPStart => "RIFF"u8;
        public static ReadOnlySpan<byte> WebPEnd => "WEBP"u8;
    }

    protected abstract Task<Stream> OpenReadAsync();
    protected abstract Task<Stream> OpenWriteAsync();
}


