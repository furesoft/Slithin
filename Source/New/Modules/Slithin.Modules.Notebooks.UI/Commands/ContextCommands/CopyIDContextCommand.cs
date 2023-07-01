using Avalonia;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;

namespace Slithin.Modules.Notebooks.UI.Commands.ContextCommands;

[Context(UIContext.Notebook)]
public class CopyIDContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;

    public CopyIDContextCommand(ILocalisationService localisationService)
    {
        _localisationService = localisationService;
    }

    public object ParentViewModel { get; set; }
    public TranslatedString Title => "Copy ID";

    public bool CanExecute(object data)
    {
        return data is Metadata md
                && md.VisibleName != _localisationService.GetString("Quick sheets")
                && md.VisibleName != _localisationService.GetString("Up ..")
                && md.VisibleName != _localisationService.GetString("Trash");
    }

    public async void Execute(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        await Application.Current.Clipboard.SetTextAsync(md.ID);
    }
}
