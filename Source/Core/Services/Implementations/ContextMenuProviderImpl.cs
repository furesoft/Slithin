using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Slithin.Core.Scripting;
using Slithin.Core.ItemContext;

namespace Slithin.Core.Services.Implementations
{
    public class ContextMenuProviderImpl : IContextMenuProvider
    {
        private readonly Dictionary<UIContext, List<IContextProvider>> _providers = new();

        public void Add(IContextProvider provider)
        {
            var attrs = provider.GetType().GetCustomAttributes<ContextAttribute>();

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

        public ContextMenu BuildMenu<T>(UIContext context, T item, object parent = null)
        {
            if (!_providers.ContainsKey(context))
                return null;

            var providersForContext = _providers[context];
            var availableContexts = providersForContext.Where(p => p.CanHandle(item));

            if (!availableContexts.Any())
                return null;

            var menu = new ContextMenu();

            menu.Items = availableContexts.SelectMany(c =>
            {
                c.ParentViewModel = parent;

                return c.GetMenu(item);
            });

            return menu;
        }

        public void Init()
        {
            var providers = Utils.Find<IContextProvider>();

            foreach (var provider in providers)
            {
                Add(provider);
            }
        }
    }
}
