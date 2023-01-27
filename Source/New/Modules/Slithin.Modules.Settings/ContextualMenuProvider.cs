using AuroraModularis.Core;
using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Settings.Commands;

namespace Slithin.Modules.Settings;

internal class ContextualMenuProvider : IContextualMenuProvider
{
    public void RegisterContextualMenuElements(ContextualRegistrar registrar)
    {
        var supportCommand = Container.Current.Resolve<ShowDonationButtonsCommand>();
        
        registrar.RegisterFor(UIContext.Settings,
            new ContextualButton("Support Slithin", "support",  supportCommand));
    }
}
