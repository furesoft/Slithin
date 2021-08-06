using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace Slithin.Core
{
    public static class PageSelector
    {
        private static Grid? s_conextualMenu;
        private static TabControl? s_container;

        public static void CollectPages()
        {
            var pages = new List<TabItem>();
            foreach (var type in typeof(App).Assembly.GetTypes())
            {
                if (typeof(IPage).IsAssignableFrom(type) && !type.IsInterface)
                {
                    var instance = Activator.CreateInstance(type);
                    var pageInstance = instance as IPage;
                    var controlInstance = instance as Control;

                    if (pageInstance.IsEnabled())
                    {
                        var page = new TabItem
                        {
                            Header = pageInstance?.Title,
                            DataContext = controlInstance.DataContext
                        };

                        if (pageInstance.UseContextualMenu())
                        {
                            page.Tag = pageInstance.GetContextualMenu();
                        }

                        page.Content = controlInstance;

                        pages.Add(page);
                    }
                }
            }

            s_container.Items = pages;
            s_container.SelectedIndex = 0;
        }

        public static bool GetIsContextualContainer(Control control)
        {
            return s_conextualMenu == control;
        }

        public static bool GetIsPageContainer(Control control)
        {
            return s_container == control;
        }

        public static void SetIsContextualContainer(Grid control, bool value)
        {
            s_conextualMenu = control;
        }

        public static void SetIsPageContainer(TabControl control, bool value)
        {
            s_container = control;

            s_container.SelectionChanged += (object sender, SelectionChangedEventArgs e) =>
            {
                if (e.AddedItems.Count > 0)
                {
                    if (e.AddedItems[0] is TabItem tab)
                    {
                        if (s_conextualMenu is not null && tab.Tag is Control context && control is not null)
                        {
                            s_conextualMenu.Children.Clear();

                            context.DataContext = tab.DataContext;
                            s_conextualMenu.Children.Add(context);
                        }
                        else
                        {
                            s_conextualMenu?.Children.Clear();
                        }
                    }
                }
            };

            CollectPages();
        }
    }
}
