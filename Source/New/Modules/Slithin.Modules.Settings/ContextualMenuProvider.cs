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
        var supportCommand = ServiceContainer.Current.Resolve<ShowDonationButtonsCommand>();
        var aboutCommand = ServiceContainer.Current.Resolve<AboutCommand>();

        registrar.RegisterFor(UIContext.Settings,
            new ContextualButton("Support Slithin", "support", supportCommand));

        registrar.RegisterFor(UIContext.Settings,
            new ContextualButton("About", "about", aboutCommand));
    }
}
