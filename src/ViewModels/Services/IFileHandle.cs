﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMKiwi.PicMapper.ViewModels.Services;

/// <summary>
/// File handle to pass around. 
/// </summary>
public interface IFileHandle
{
    string FilePath { get; }
    ValueTask<Stream> GetWriteStream();
}
