﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace MMKiwi.PicMapper.Models.Services;

public interface IFileLoader
{
    IAsyncEnumerable<IBitmapProvider> LoadImageAsync();

    Task<IFileHandle?> BrowseForFileAsync(string description, IReadOnlyList<string> extensions);
}