using System.Reflection;
using Avalonia.Controls;
using Slithin.Core;
using Slithin.Modules.Device.UI;
using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Menu.Models.Menu;
using Slithin.Modules.Menu.Views;

namespace Slithin.Modules.Menu;

public class ContextualMenuBuilderImpl : IContextualMenuBuilder
{
    private readonly Dictionary<string, ContextualRegistrar> _registrars = new();

    public UserControl BuildContextualMenu(string id)
    {
        var control = new DefaultContextualMenu();
        control.DataContext = _registrars[id].GetAllElements();

        if (_registrars[id].GetAllElements().Any())
        {
            return new EmptyContextualMenu();
        }
        
        return control;
    }

    public void Init()
    {
        foreach (var page in Utils.Find<IPage>())
        {
            var contextAttribute = page.GetType().GetCustomAttribute<ContextAttribute>();

            if (contextAttribute is null) continue;
            
            if (!_registrars.ContainsKey(contextAttribute.Context))
            {
                _registrars.Add(contextAttribute.Context, new());
            }
        }

        //Todo: refactor to make it more performant, O(n³) is not ok!!
        foreach (var provider in Utils.Find<IContextualMenuProvider>())
        {
            var tmpRegistrar = new ContextualRegistrar();
            provider.RegisterContextualMenuElements(tmpRegistrar);

            foreach (var bag in tmpRegistrar.GetAllElements())
            {
                foreach (var element in bag.Value)
                {
                    _registrars[bag.Key].RegisterFor(bag.Key, element);
                }
            }
        }
    }
}
