using System.Collections.Generic;
using Avalonia.Controls;
using Slithin.Core;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.ViewModels.Pages;

namespace Slithin.ContextMenus;

[Context(UIContext.Template)]
public class TemplatesContextMenu : IContextProvider
{
    private readonly ILocalisationService _localisationService;

    public TemplatesContextMenu(ILocalisationService localisationService)
    {
        _localisationService = localisationService;
    }

    public object ParentViewModel { get; set; }

    public bool CanHandle(object obj)
    {
        return obj is Template;
    }

    public ICollection<MenuItem> GetMenu(object obj)
    {
        var menu = new List<MenuItem>();
        if (ParentViewModel is not TemplatesPageViewModel St)
            return menu;

        menu.Add(new MenuItem { Header = _localisationService.GetString("Remove"), Command = new DelegateCommand(_ => St.RemoveTemplateCommand.Execute(obj)) });
        return menu;
    }
}
