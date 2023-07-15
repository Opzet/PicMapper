// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
using System.Transactions;

namespace MMKiwi.PicMapper.ViewModels;

[JsonSourceGenerationOptions(WriteIndented = true, IgnoreReadOnlyFields = true, IgnoreReadOnlyProperties = true)]
[JsonSerializable(typeof(MainWindowViewModel.Settings), TypeInfoPropertyName = "MainWindowViewModel")]
public partial class ViewModelSerializer : JsonSerializerContext { }