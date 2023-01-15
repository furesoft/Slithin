using AuroraModularis.Core;
using Slithin.Modules.Export.Commands;
using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;
using Slithin.Modules.Menu.Models.ItemContext;

namespace Slithin.Modules.Export;

internal class ContextualMenuProvider : IContextualMenuProvider
{
    public void RegisterContextualMenuElements(ContextualRegistrar registrar)
    {
        var exportCommand = Container.Current.Resolve<ExportCommand>();

        registrar.RegisterFor(UIContext.Notebook,
            new ContextualButton("Export", "Material.ExportVariant", exportCommand));
    }
}
