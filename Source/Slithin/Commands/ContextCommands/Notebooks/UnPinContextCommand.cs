using Slithin.Core;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;

namespace Slithin.Commands.ContextCommands.Notebooks;

[Context(UIContext.Notebook)]
public class UnPinContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly Xochitl _xochitl;

    public UnPinContextCommand(ILocalisationService localisationService, Xochitl xochitl)
    {
        _localisationService = localisationService;
        _xochitl = xochitl;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Unpin");

    public bool CanHandle(object data)
    {
        return data is Metadata md && md.IsPinned;
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

        md.IsPinned = false;
        md.Version++;
        md.Save();

        md.Upload();

        _xochitl.ReloadDevice();
    }
}
