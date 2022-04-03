using Avalonia;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Remarkable.Models;

namespace Slithin.Commands.ContextCommands.Notebooks;

[Context(UIContext.Notebook)]
public class CopyIDContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;

    public CopyIDContextCommand(ILocalisationService localisationService)
    {
        _localisationService = localisationService;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Copy ID");

    public bool CanHandle(object data)
    {
        return data != null
                && data is Metadata md
                && md.VisibleName != _localisationService.GetString("Quick sheets")
                && md.VisibleName != _localisationService.GetString("Up ..")
                && md.VisibleName != _localisationService.GetString("Trash");
    }

    public async void Invoke(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        await Application.Current.Clipboard.SetTextAsync(md.ID);
    }
}
