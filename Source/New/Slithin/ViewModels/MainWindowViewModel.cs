using System.Collections.ObjectModel;
using System.Reflection;
using Avalonia.Controls;
using Slithin.Core.Menu;
using Slithin.Core.MVVM;
using Slithin.Entities;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.UI.ContextualMenus;

namespace Slithin.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly ILocalisationService _localisationService;
    private object _contextualMenu;
    private Page _selectedTab;

    private string _title;

    public MainWindowViewModel(IVersionService versionService,
                               ILoginService loginService,
                               ILocalisationService localisationService)
    {
        _localisationService = localisationService;
        Title = $"Slithin {versionService.GetSlithinVersion()} - {loginService.GetCurrentCredential().Name} -";

        LoadMenu();
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
        var toRearrange = new List<(int index, Page page, Control view)>();

        foreach (var type in typeof(App).Assembly.GetTypes())
        {
            if (!typeof(IPage).IsAssignableFrom(type) || type.IsInterface)

                continue;

            var instance = Activator.CreateInstance(type);
            var preserveIndexAttribute = type.GetCustomAttribute<PreserveIndexAttribute>();
            var pageIconAttribute = type.GetCustomAttribute<PageIconAttribute>();

            if (instance is not IPage pageInstance || !pageInstance.IsEnabled() || instance is not Control controlInstance)
                continue;

            var header = _localisationService.GetString(pageInstance?.Title);
            var page = new Page
            {
                Header = header,
                DataContext = controlInstance.DataContext,
            };

            page.Icon = App.Current.FindResource(pageIconAttribute == null ? "Material.Refresh" : pageIconAttribute.Key);

            if (pageInstance.UseContextualMenu())
            {
                page.Tag = pageInstance.GetContextualMenu();
            }
            else
            {
                page.Tag = new EmptyContextualMenu() { DataContext = header };
            }

            toRearrange.Add((preserveIndexAttribute != null ? preserveIndexAttribute.Index : toRearrange.Count, page, controlInstance));
        }

        foreach (var page in toRearrange.OrderBy(_ => _.index))
        {
            Menu.Add(page.page);
            Tabs.Add(page.view);
        }
    }

    private void Refresh()
    {
        if (SelectedTab?.Tag is not Control context)
        {
            ContextualMenu = null;
            return;
        }

        if (SelectedTab?.DataContext is BaseViewModel pl)
        {
            pl.Load();
        }

        if (SelectedTab?.DataContext is null || SelectedTab.Tag is EmptyContextualMenu)
        {
            SelectedTab.DataContext = SelectedTab.Header;
        }

        context.DataContext = SelectedTab?.DataContext;
        ContextualMenu = context;
    }
}
