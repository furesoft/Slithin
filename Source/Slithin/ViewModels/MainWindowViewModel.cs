using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.Models;
using Slithin.UI.ContextualMenus;

namespace Slithin.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private object _contextualMenu;
    private Page _selectedTab;

    private string _title;

    public MainWindowViewModel(IVersionService versionService)
    {
        LoadMenu();

        Title = $"Slithin {versionService.GetSlithinVersion()}";
    }

    public object ContextualMenu
    {
        get => _contextualMenu;
        set => SetValue(ref _contextualMenu, value);
    }

    public ObservableCollection<Page> Menu { get; set; } = new();

    public Page SelectedTab
    {
        get => _selectedTab;
        set
        {
            SetValue(ref _selectedTab, value);
            Refresh();
        }
    }

    public ObservableCollection<object> Tabs { get; set; } = new();

    public string Title
    {
        get => _title;
        set => SetValue(ref _title, value);
    }

    private void LoadMenu()
    {
        foreach (var type in typeof(App).Assembly.GetTypes())
        {
            if (!typeof(IPage).IsAssignableFrom(type) || type.IsInterface)

                continue;

            var instance = Activator.CreateInstance(type);

            if (instance is not IPage pageInstance || !pageInstance.IsEnabled() || instance is not Control controlInstance)
                continue;

            var page = new Page
            {
                Header = pageInstance?.Title,
                DataContext = controlInstance.DataContext
            };

            if (pageInstance.UseContextualMenu())
            {
                page.Tag = pageInstance.GetContextualMenu();
            }
            else
            {
                page.Tag = new EmptyContextualMenu() { DataContext = pageInstance?.Title };
            }

            Tabs.Add(controlInstance);

            Menu.Add(page);
        }
    }

    private void Refresh()
    {
        if (SelectedTab.Tag is not Control context)
        {
            ContextualMenu = null;
            return;
        }

        if (SelectedTab.DataContext is BaseViewModel pl)
        {
            pl.Load();
        }

        if (SelectedTab.DataContext is null || SelectedTab.Tag is EmptyContextualMenu)
        {
            SelectedTab.DataContext = SelectedTab.Header;
        }

        context.DataContext = SelectedTab.DataContext;
        ContextualMenu = context;
    }
}
