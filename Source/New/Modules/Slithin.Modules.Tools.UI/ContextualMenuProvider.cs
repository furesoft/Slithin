using Slithin.Modules.Menu.Models.ContextualMenu;
using Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Tools.UI.ViewModels;

namespace Slithin.Modules.Tools.UI;

internal class ContextualMenuProvider : IContextualMenuProvider
{
    private readonly ToolsPageViewModel _viewModel;

    public ContextualMenuProvider(ToolsPageViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public void RegisterContextualMenuElements(ContextualRegistrar registrar)
    {
        registrar.RegisterFor(UIContext.Tools, new ContextualButton("Execute", "Material.HexagonMultiple", _viewModel.ExecuteScriptCommand));
        registrar.RegisterFor(UIContext.Tools, new ContextualButton("Config", "Vaadin.CogOutline", _viewModel.ConfigurateScriptCommand));
    }
}
