﻿using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Slithin.Core;
using Slithin.Modules.Device.UI;
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

        control.FindControl<ItemsPresenter>("presenter").DataContext = elements.ToArray();

        if (!elements.Any())
        {
            return new EmptyContextualMenu();
        }

        return control;
    }

    public void Init()
    {
        foreach (var providerType in Utils.FindTypes<IContextualMenuProvider>())
        {
            var provider = Container.Current.Resolve<IContextualMenuProvider>(providerType);
            provider.RegisterContextualMenuElements(_registrar);
        }
    }
}
