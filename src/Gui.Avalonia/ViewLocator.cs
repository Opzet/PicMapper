using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MMKiwi.PicMapper.ViewModels;

namespace MMKiwi.PicMapper.Gui.Avalonia;

public class ViewLocator : IDataTemplate
{
    public Control Build(object? data)
    {
        ArgumentNullException.ThrowIfNull(data);
        string name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        return type != null ? (Control)Activator.CreateInstance(type)! : new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data) => data is ViewModelBase;
}