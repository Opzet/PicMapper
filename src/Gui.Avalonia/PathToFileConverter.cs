// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Avalonia.Data;
using Avalonia.Data.Converters;

using MMKiwi.PicMapper.Gui.Avalonia.Services;
using MMKiwi.PicMapper.Models.Services;

using System.Globalization;

namespace MMKiwi.PicMapper.Gui.Avalonia;

public class PathToFileConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IFileHandle fileHandle)
        {
            return fileHandle.FilePath;
        }
        return new BindingNotification(value);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string path)
        {
            return new AvaloniaFileLoader.LocalFileHandle(path);
        }
        return new BindingNotification(value);

    }
}