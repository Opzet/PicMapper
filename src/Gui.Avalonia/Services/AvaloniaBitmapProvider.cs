// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.Platform.Storage;
using MMKiwi.PicMapper.Models.Services;
using Avalonia.Media.Imaging;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Directory = MetadataExtractor.Directory;

namespace MMKiwi.PicMapper.Gui.Avalonia.Services;
public class AvaloniaBitmapProvider : IBitmapProvider
{
    private AvaloniaBitmapProvider(IStorageFile file, Bitmap thumbnail, double width, double height, string fileName, double? x, double? y, string mimeType)
    {
        StorageFile = file;
        Thumbnail = thumbnail;
        Width = width;
        Height = height;
        FileName = fileName;
        X = x;
        Y = y;
        MimeType = mimeType;
    }

    public static async Task<AvaloniaBitmapProvider> LoadAsync(IStorageFile file)
    {
        await using Stream imageStream = await file.OpenReadAsync();

        string mimeType = DetectMimeType(imageStream);

        imageStream.Seek(0, SeekOrigin.Begin);
        var thumbnail = Bitmap.DecodeToHeight(imageStream, 100);

        imageStream.Seek(0, SeekOrigin.Begin);

        IReadOnlyList<Directory> metadata = ImageMetadataReader.ReadMetadata(imageStream);

        GpsDirectory? gps = metadata.OfType<GpsDirectory>().FirstOrDefault();
        GeoLocation? location = gps?.GetGeoLocation();

        return new(file, thumbnail, 0, 0, file.Path.Segments.Last(), location?.Longitude, location?.Latitude, mimeType);
    }
    public double Width { get; }

    public double Height { get; }

    public string FileName { get; }

    public IStorageFile StorageFile { get; }

    object IBitmapProvider.Thumbnail => Thumbnail;

    public Bitmap Thumbnail { get; }

    public double? X { get; }

    public double? Y { get; }
    public string MimeType { get; }

    public string ThumbnailMimeType => "image/png";

    public async Task<Bitmap> GetImageAsync()
    {
        Stream imageStream = await StorageFile.OpenReadAsync();
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
        await using Stream imageStream = await StorageFile.OpenReadAsync();
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
}


