﻿using System.Reflection;
using AuroraModularis.Core;
using Avalonia.Controls;
using Slithin.Modules.Menu.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.Models;

namespace Slithin.Modules.Menu;

internal class ContextMenuProviderImpl : IContextMenuProvider
{
    private readonly Dictionary<string, List<IContextProvider>> _providers = new();

    public void AddProvider(IContextProvider provider)
    {
        var attrs = provider.GetType().GetCustomAttributes<ContextAttribute>();

        foreach (var attr in attrs)
        {
            if (_providers.ContainsKey(attr.Context))
            {
                _providers[attr.Context].Add(provider);
                continue;
            }

            var list = new List<IContextProvider> { provider };

            _providers.Add(attr.Context, list);
        }
    }

    public void AddProvider(IContextProvider provider, IContextCommand command)
    {
        var attrs = command.GetType().GetCustomAttributes<ContextAttribute>();

        foreach (var attr in attrs)
        {
            if (_providers.ContainsKey(attr.Context))
            {
                _providers[attr.Context].Add(provider);
                continue;
            }

            var list = new List<IContextProvider>();
            list.Add(provider);

            _providers.Add(attr.Context, list);
        }
    }

    public ContextMenu BuildMenu<T>(string context, T item, object parent = null)
    {
        if (!_providers.ContainsKey(context))
        {
            return null;
        }
        
        var providersForContext = _providers[context];
        var availableContexts = providersForContext.Where(p => p.CanHandle(item));

        var iContextProviders = availableContexts as IContextProvider[] ?? availableContexts.ToArray();
        if (!iContextProviders.Any())
        {
            return null;
        }

        IEnumerable<MenuItem> ContextProviderSelector(IContextProvider c)
        {
            c.ParentViewModel = parent;

            if (item is UpDirectoryModel || item is TrashModel)
            {
                return Array.Empty<MenuItem>();
            }

            return c.GetMenu(item);
        }

        var menu = new ContextMenu { ItemsSource = iContextProviders.SelectMany(ContextProviderSelector) };

        return menu;
    }

    public void Init()
    {
        var typeFinder = ServiceContainer.Current.Resolve<ITypeFinder>();
        var providerTypes = typeFinder.FindTypes<IContextProvider>();

        foreach (var providerType in providerTypes)
        {
            if (providerType.IsAssignableFrom(typeof(CommandBasedContextMenu)))
            {
                continue;
            }

            var provider = ServiceContainer.Current.Resolve<IContextProvider>(providerType);

            AddProvider(provider);
        }

        var commandTypes = typeFinder.FindTypes<IContextCommand>();
        foreach (var commandType in commandTypes)
        {
            var resolvedCommand = ServiceContainer.Current.Resolve<IContextCommand>(commandType);

            AddProvider(new CommandBasedContextMenu(resolvedCommand), resolvedCommand);
        }
    }
}
