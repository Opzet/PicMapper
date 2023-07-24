﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using NetEscapades.EnumGenerators;

using System.Text.Json.Serialization;

namespace MMKiwi.PicMapper.ViewModels;

[EnumExtensions]
[JsonConverter(typeof(OutputFormatConverter))]
public enum OutputFormat
{
    KML
}
