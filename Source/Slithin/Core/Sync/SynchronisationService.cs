using System.Collections.ObjectModel;
using LiteDB;
using Slithin.Models;
using Slithin.Commands;
using Slithin.Core.MVVM;

namespace Slithin.Core.Sync;

public class SynchronisationService : NotifyObject
{
    public SynchronisationService(LiteDatabase db)
    {
        TemplateFilter = new();
        NotebooksFilter = new();
        ToolsFilter = new();

        SynchronizeCommand = ServiceLocator.Container.Resolve<SynchronizeCommand>();
    }

    public ObservableCollection<CustomScreen> CustomScreens { get; set; } = new();

    public NotebooksFilter NotebooksFilter { get; set; }
    public SynchronizeCommand SynchronizeCommand { get; set; }
    public TemplateFilter TemplateFilter { get; set; }
    public ToolsFilter ToolsFilter { get; set; }
}
