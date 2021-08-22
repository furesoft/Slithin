using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Slithin.Core;

namespace Slithin.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private object _contextualMenu;
        private Page _selectedTab;

        public MainWindowViewModel()
        {
            LoadMenu();
        }

        public object ContextualMenu
        {
            get { return _contextualMenu; }
            set { SetValue(ref _contextualMenu, value); }
        }

        public ObservableCollection<Page> Menu { get; set; } = new();

        public Page SelectedTab
        {
            get { return _selectedTab; }
            set
            {
                SetValue(ref _selectedTab, value);
                Refresh();
            }
        }

        public ObservableCollection<object> Tabs { get; set; } = new();

        private void LoadMenu()
        {
            foreach (var type in typeof(App).Assembly.GetTypes())
            {
                if (typeof(IPage).IsAssignableFrom(type) && !type.IsInterface)
                {
                    var instance = Activator.CreateInstance(type);
                    var pageInstance = instance as IPage;
                    var controlInstance = instance as Control;

                    if (pageInstance.IsEnabled())
                    {
                        var page = new Page
                        {
                            Header = pageInstance?.Title,
                            DataContext = controlInstance.DataContext
                        };

                        if (pageInstance.UseContextualMenu())
                        {
                            page.Tag = pageInstance.GetContextualMenu();
                        }

                        Tabs.Add(controlInstance);

                        Menu.Add(page);
                    }
                }
            }
        }

        private void Refresh()
        {
            if (SelectedTab.Tag is Control context)
            {
                if (SelectedTab.DataContext is BaseViewModel pl)
                {
                    pl.Load();
                }

                context.DataContext = SelectedTab.DataContext;
                ContextualMenu = context;
            }
            else
            {
                ContextualMenu = null;
            }
        }
    }

    public class Page
    {
        public object DataContext { get; set; }
        public string Header { get; set; }
        public object Tag { get; set; }
    }
}
