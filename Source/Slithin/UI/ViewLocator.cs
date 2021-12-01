using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml.Templates;
using NodeEditor.ViewModels;
using Slithin.Core;

namespace Slithin.UI;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        if (data is NodeViewModel vm)
        {
            var name = vm.Content.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
        }

        else
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                return new TextBlock { Text = "VM Not Found: " };
            }
        }

        return new TextBlock { Text = "VM Not Found: " };
    }

    public bool Match(object data)
    {
        return data is BaseViewModel;
    }
}
