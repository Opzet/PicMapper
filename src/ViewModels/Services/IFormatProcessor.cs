// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using MMKiwi.PicMapper.ViewModels.Services;

namespace MMKiwi.PicMapper.ViewModels;

public interface IFormatProcessor
{
    IFileHandle OutputPath { get; }
    Task Process(IEnumerable<IBitmapProvider> images);
}