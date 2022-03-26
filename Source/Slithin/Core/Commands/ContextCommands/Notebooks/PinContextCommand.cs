using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;

namespace Slithin.Core.Commands.ContextCommands.Notebooks;

[Context(UIContext.Notebook)]
public class PinContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly Xochitl _xochitl;

    public PinContextCommand(ILocalisationService localisationService, Xochitl xochitl)
    {
        _localisationService = localisationService;
        _xochitl = xochitl;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Pin");

    public bool CanHandle(object data)
    {
        return data is Metadata md && !md.IsPinned
            && md.VisibleName != _localisationService.GetString("Quick sheets")
            && md.VisibleName != _localisationService.GetString("Trash");
    }

    public void Invoke(object data)
    {
        if (data is not Metadata md)
        {
            return;
        }

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);

        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();

        md.IsPinned = true;
        md.Version++;
        md.Save();

        md.Upload();

        _xochitl.ReloadDevice();
    }
}
