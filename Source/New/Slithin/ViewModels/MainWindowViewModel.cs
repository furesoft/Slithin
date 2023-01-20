using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Media;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Entities;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Diagnostics.Sentry.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Menu.Models.Menu;
using Slithin.Modules.Menu.Views;
using Slithin.Modules.Repository.Models;

namespace Slithin.ViewModels;

public class MainWindowViewModel : BaseViewModel
{
    private readonly IContextualMenuBuilder _contextualMenuBuilder;
    private readonly IEventService _eventService;
    private readonly IDiagnosticService _diagnosticService;
    private readonly ILocalisationService _localisationService;
    private object _contextualMenu;

    private Page _selectedTab;

    private string _title;

    public MainWindowViewModel(IVersionService versionService,
        ILoginService loginService, IDiagnosticService diagnosticService,
        ILocalisationService localisationService,
        IContextualMenuBuilder contextualMenuBuilder,
        IEventService eventService)
    {
        _diagnosticService = diagnosticService;
        _localisationService = localisationService;
        _contextualMenuBuilder = contextualMenuBuilder;
        _eventService = eventService;
        Title = $"Slithin {versionService.GetSlithinVersion()} - {loginService.GetCurrentCredential().Name} -";

        SynchronizeCommand = new DelegateCommand(_ =>
        {
            //ToDo: implement synchronize command
        });

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
            RefreshPage();
        }
    }

    public double MenuWidth => CalculateMenuWidth();

    public ObservableCollection<object> Tabs { get; set; } = new();

    public ICommand SynchronizeCommand { get; set; }

    public string Title
    {
        get => _title;
        set => SetValue(ref _title, value);
    }

    private static object? GetIcon(PageIconAttribute? pageIconAttribute, Type type)
    {
        return Application.Current.FindResource(pageIconAttribute == null ? "Material.Refresh" : pageIconAttribute.Key);
    }

    private double CalculateMenuWidth()
    {
        var maximumWidth = 0d;

        var textFormat = new FormattedText();
        textFormat.FontSize = 25;
        textFormat.Typeface = new Typeface("Consolas");

        foreach (var page in Menu)
        {
            textFormat.Text = page.Header;

            maximumWidth = Math.Max(maximumWidth, textFormat.Bounds.Width);
        }

        return maximumWidth + 60;
    }

    private void LoadMenu()
    {
        var monitor = _diagnosticService.StartPerformanceMonitoring("Loading", "Menu");

        var toRearrange = new List<(int index, Page page, Control view)>();

        var types = Utils.FindTypes<IPage>();
        foreach (var type in types)
        {
            if (!typeof(IPage).IsAssignableFrom(type) || type.IsInterface)
            {
                continue;
            }

            if (!typeof(Control).IsAssignableFrom(type))
            {
                continue;
            }

            var instance = Activator.CreateInstance(type);
            var preserveIndexAttribute = type.GetCustomAttribute<PreserveIndexAttribute>();
            var pageIconAttribute = type.GetCustomAttribute<PageIconAttribute>();
            var contextAttribute = type.GetCustomAttribute<ContextAttribute>();
            var controlInstance = (Control)instance;

            if (instance is not IPage pageInstance || !pageInstance.IsEnabled())
            {
                continue;
            }

            if (contextAttribute is null)
            {
                continue;
            }

            var header = _localisationService.GetString(pageInstance.Title);
            var page = new Page { Header = header, DataContext = controlInstance.DataContext };

            page.Icon = GetIcon(pageIconAttribute, type);

            var contextMenu = _contextualMenuBuilder.BuildContextualMenu(contextAttribute.Context);
            page.Tag = contextMenu;

            if (contextMenu is EmptyContextualMenu)
            {
                contextMenu.DataContext = header;
            }

            ApplyDataContextToContextualElements(contextMenu, page.DataContext);

            toRearrange.Add((preserveIndexAttribute != null ? preserveIndexAttribute.Index : toRearrange.Count, page,
                controlInstance));
        }

        foreach (var page in toRearrange.OrderBy(_ => _.index))
        {
            Menu.Add(page.page);
            Tabs.Add(page.view);
        }

        monitor.Dispose();
    }

    private void ApplyDataContextToContextualElements(UserControl contextMenu, object? dataContext)
    {
        if (contextMenu is not DefaultContextualMenu)
        {
            return;
        }

        var elements =
            contextMenu.FindControl<ItemsPresenter>("presenter").DataContext as IEnumerable<ContextualElement>;

        if (elements == null)
        {
            return;
        }

        foreach (var element in elements)
        {
            element.DataContext = dataContext;
        }
    }

    private void RefreshPage()
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

        _eventService.Invoke("PageChanged", SelectedTab);
    }
}
