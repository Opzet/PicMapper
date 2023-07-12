﻿namespace MMKiwi.PicMapper.Models.Services;
public interface IBitmapProvider
{
    object Thumbnail { get; }
    Task<object> GetImageAsync();
    double Width { get; }
    double Height { get; }
    string FileName { get; }
    double? X { get; }
    double? Y { get; }

    byte[] GetThumbnail();
    string ThumbnailMimeType { get; }
    string MimeType { get; }
    Task CopyImageAsync(Stream destination);
}