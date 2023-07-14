using System.Text.Json.Serialization;
using System.Transactions;

namespace MMKiwi.PicMapper.ViewModels;

[JsonSourceGenerationOptions(WriteIndented = true, IgnoreReadOnlyFields = true, IgnoreReadOnlyProperties = true)]
[JsonSerializable(typeof(MainWindowViewModel.Settings), TypeInfoPropertyName = "MainWindowViewModel")]
public partial class ViewModelSerializer : JsonSerializerContext { }