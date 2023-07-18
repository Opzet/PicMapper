// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using MMKiwi.PicMapper.Models.Services;

using ReactiveUI.Validation.Abstractions;

namespace MMKiwi.PicMapper.ViewModels;

public interface IOutputSettingsViewModel: IValidatableViewModel
{
    OutputSettingsViewModel ParentViewModel { get; }

    IFormatProcessor CreateProcessor();
}

public interface IFormatProcessor
{
    string OutputPath { get; }
    Task Process(IEnumerable<IBitmapProvider> images);
}