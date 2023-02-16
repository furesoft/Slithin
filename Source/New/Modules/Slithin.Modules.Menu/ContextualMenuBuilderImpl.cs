using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Views;

namespace Slithin.Modules.Menu;

public class ContextualMenuBuilderImpl : IContextualMenuBuilder
{
    private readonly ContextualRegistrar _registrar = new();

    public UserControl BuildContextualMenu(string id)
    {
        var control = new DefaultContextualMenu();
        var elements = _registrar.GetAllElements(id);

        var contextualElements = elements.ToArray();
        control.FindControl<ItemsPresenter>("presenter").DataContext = contextualElements;

        if (!contextualElements.Any())
        {
            return new EmptyContextualMenu();
        }

        return control;
    }

    public void Init()
    {
        var typeFinder = ServiceContainer.Current.Resolve<ITypeFinder>();

        foreach (var provider in typeFinder.FindAndResolveTypes<IContextualMenuProvider>())
        {
            provider.RegisterContextualMenuElements(_registrar);
        }
    }
}
