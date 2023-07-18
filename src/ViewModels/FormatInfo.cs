// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace MMKiwi.PicMapper.ViewModels;

public record class FormatInfo(OutputFormat OutputFormat, string DisplayName, string[] Extensions, IOutputSettingsViewModel ViewModel)
{
    public override string ToString() => $"{DisplayName} ({string.Join(", ", Extensions)})";
}