using AuroraModularis.Core;
using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Settings.UI.Commands;

namespace Slithin.Modules.Settings.UI;

internal class ContextualMenuProvider : IContextualMenuProvider
{
    public void RegisterContextualMenuElements(ContextualRegistrar registrar)
    {
        var supportCommand = ServiceContainer.Current.Resolve<ShowDonationButtonsCommand>();
        var aboutCommand = ServiceContainer.Current.Resolve<AboutCommand>();
        var feedbackCommand = ServiceContainer.Current.Resolve<FeedbackCommand>();
        var buyCoffeeCommand = ServiceContainer.Current.Resolve<BuyCoffeeCommand>();

        registrar.RegisterFor(UIContext.Settings,
            new ContextualButton("Support Slithin", "support", supportCommand));

        registrar.RegisterFor(UIContext.Settings,
            new ContextualButton("About", "about", aboutCommand));
        
        registrar.RegisterFor(UIContext.Settings, 
            new ContextualButton("Leave a feedback", "VSImageLib2019.FeedbackSmile_grey_16x", feedbackCommand));
        
        registrar.RegisterFor(UIContext.Settings, new ContextualButton("Buy me a coffee", "BuyMeaCoffee", buyCoffeeCommand));
    }
}
