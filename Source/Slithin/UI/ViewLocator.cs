using System;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NodeEditor.ViewModels;
using Slithin.Core;
using Slithin.VPL.Components.Views;
using Slithin.Core.VPLNodeBuilding;

namespace Slithin.UI;

public class ViewLocator : IDataTemplate
{
    public IControl Build(object data)
    {
        if (data is NodeViewModel vm)
        {
            var viewAttribute = vm.GetType().GetCustomAttribute<NodeViewAttribute>();

            if (viewAttribute != null)
            {
                return (Control)Activator.CreateInstance(viewAttribute.Type);
            }
            else
            {
                var name = vm.Content.GetType().FullName!.Replace("ViewModel", "View");
                var type = Type.GetType(name);

                if (type != null)
                {
                    return (Control)Activator.CreateInstance(type)!;
                }
                else
                {
                    return Activator.CreateInstance<DefaultView>();
                }
            }
        }
        else
        {
            var viewAttribute = data.GetType().GetCustomAttribute<NodeViewAttribute>();

            if (viewAttribute != null)
            {
                return (Control)Activator.CreateInstance(viewAttribute.Type);
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
                    return Activator.CreateInstance<DefaultView>();
                }
            }
        }

        return new TextBlock { Text = "VM Not Found: " };
    }

    public bool Match(object data)
    {
        return data is BaseViewModel;
    }
}
