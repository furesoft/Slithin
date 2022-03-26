using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;

namespace Slithin.Core.Commands.ContextCommands.Notebooks;

[Context(UIContext.Notebook)]
public class UnPinContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;

    public UnPinContextCommand(ILocalisationService localisationService)
    {
        _localisationService = localisationService;
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

        md.IsPinned = false;

        ServiceLocator.SyncService.NotebooksFilter.Documents.Remove(md);
        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);

        ServiceLocator.SyncService.NotebooksFilter.SortByFolder();

        md.Upload();
    }
}
