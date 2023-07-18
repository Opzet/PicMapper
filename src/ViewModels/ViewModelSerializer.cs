// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using MMKiwi.PicMapper.ViewModels.Design;

using System.Text.Json.Serialization;
using System.Transactions;

namespace MMKiwi.PicMapper.ViewModels;

[JsonSourceGenerationOptions(WriteIndented = true, IgnoreReadOnlyFields = true, IgnoreReadOnlyProperties = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(OutputSettingsViewModel.Settings), TypeInfoPropertyName = "OutputSettingsViewModel")]
[JsonSerializable(typeof(KmlSettingsViewModel.Settings), TypeInfoPropertyName = "KmlSettingsViewModel")]
[JsonSerializable(typeof(ConfigurationRoot), TypeInfoPropertyName = "Root")]
public partial class ViewModelSerializer : JsonSerializerContext
{
    public class ConfigurationRoot
    {
        public OutputSettingsViewModel.Settings? OutputSettings { get; set; }
        public KmlSettingsViewModel.Settings? Kml { get; set; }
    }
}