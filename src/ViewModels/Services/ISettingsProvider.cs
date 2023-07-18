// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace MMKiwi.PicMapper.ViewModels.Services;
public interface ISettingsProvider
{
    Task SaveOutputSettings(OutputSettingsViewModel viewModel);
    Task LoadOutputSettings(OutputSettingsViewModel viewModel);
    Task SaveKmlSettings(KmlSettingsViewModel viewModel);
    Task LoadKmlSettings(KmlSettingsViewModel viewModel);

}
