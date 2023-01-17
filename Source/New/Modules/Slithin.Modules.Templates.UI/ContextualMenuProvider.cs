using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Templates.UI.ViewModels;

namespace Slithin.Modules.Templates.UI;

internal class ContextualMenuProvider : IContextualMenuProvider
{
    private readonly TemplatesPageViewModel _viewModel;

    public ContextualMenuProvider(TemplatesPageViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public void RegisterContextualMenuElements(ContextualRegistrar registrar)
    {
        registrar.RegisterFor(UIContext.Templates, new ContextualButton("Add", "Ionicons.AddiOS", _viewModel.OpenAddModalCommand));
        registrar.RegisterFor(UIContext.Templates, new ContextualButton("Remove", "BoxIcons.SolidTrash", _viewModel.RemoveTemplateCommand));
        
        registrar.RegisterFor(UIContext.Templates,
            new ContextualDropDownButton("Filter", "Material.FilterMenu", new FilterPopup()));
    }
}
