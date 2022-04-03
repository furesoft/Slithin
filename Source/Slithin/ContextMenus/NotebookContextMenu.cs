using System.Collections.Generic;
using Avalonia.Controls;
using Slithin.Core;
using Slithin.Core.FeatureToggle;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.ViewModels.Pages;
using Slithin.Commands;
using Slithin.Core.MVVM;
using Slithin.Core.Remarkable.Models;
using Slithin.Features;

namespace Slithin.ContextMenus;

[Context(UIContext.Notebook)]
public class NotebookContextMenu : IContextProvider
{
    private readonly ILocalisationService _localisationService;

    public NotebookContextMenu(ILocalisationService localisationService)
    {
        _localisationService = localisationService;
    }

    public object ParentViewModel { get; set; }

    public bool CanHandle(object obj)
    {
        return obj != null
                && obj is Metadata md
                && md.VisibleName != _localisationService.GetString("Quick sheets")
                && md.VisibleName != _localisationService.GetString("Up ..")
                && md.VisibleName != _localisationService.GetString("Trash");
    }

    public ICollection<MenuItem> GetMenu(object obj)
    {
        List<MenuItem> menu = new();
        if (ParentViewModel is not NotebooksPageViewModel n)
        {
            return menu;
        }
        if (obj is not Metadata md)
        {
            return menu;
        }

        if (Feature<ExportFeature>.IsEnabled)
        {
            var subItems = new List<MenuItem>();

            if (Feature<ExportPdfFeature>.IsEnabled)
            {
                subItems.Add(new()
                {
                    Header = "PDF",
                    Command = new DelegateCommand(_ =>
                        ServiceLocator.Container.Resolve<ExportCommand>().Execute(obj))
                });
            }
            if (Feature<ExportPngFeature>.IsEnabled)
            {
                subItems.Add(new()
                {
                    Header = "PNG",
                    Command = new DelegateCommand(_ =>
                        ServiceLocator.Container.Resolve<ExportCommand>().Execute(obj))
                });
            }

            if (Feature<ExportSvgFeature>.IsEnabled)
            {
                subItems.Add(new()
                {
                    Header = "SVG",
                    Command = new DelegateCommand(_ =>
                        ServiceLocator.Container.Resolve<ExportCommand>().Execute(obj))
                });
            }

            menu.Add(new MenuItem
            {
                Header = _localisationService.GetString("Export"),
                Items = subItems
            });
        }

        return menu;
    }
}
