// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace MMKiwi.PicMapper.ViewModels.Services;

/// <summary>
/// Abstraction for loading an image. Currently this is implemented only
/// for an Avalonia bitmap. May need to switch to Skia since Avalonia's
/// transformations are pretty limited.
/// </summary>
public interface IBitmapProvider
{
    /// <summary>
    /// Thumbnail object for data binding.
    /// </summary>
    object Thumbnail { get; }
    Task<object> GetImageAsync();
    double Width { get; }
    double Height { get; }
    string FileName { get; }
    double? X { get; }
    double? Y { get; }

    public string UniqueId { get; }
    byte[] GetThumbnail();
    string ThumbnailMimeType { get; }
    string MimeType { get; }
    Task CopyImageAsync(Stream destination);
}
