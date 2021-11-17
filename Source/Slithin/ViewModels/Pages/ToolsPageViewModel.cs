using System.Linq;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.UI.Modals;
using Slithin.ViewModels.Modals;

namespace Slithin.ViewModels.Pages;

public class ToolsPageViewModel : BaseViewModel
{
    private readonly ToolInvoker _invoker;
    private ITool _selectedScript;

    public ToolsPageViewModel(ToolInvoker invoker)
    {
        ConfigurateScriptCommand = new DelegateCommand(_ => DialogService.Open(SelectedScript.GetModal()), _ => _ is ITool tool && tool.IsConfigurable);
        NewScriptCommand = new DelegateCommand(_ => DialogService.Open(new NewScriptModal(), new NewScriptModalViewModel()));

        ExecuteScriptCommand = new DelegateCommand(_ =>
        {
            ((ITool)_).Invoke(_);
        }, _ => _ is not null);

        _invoker = invoker;
    }

    public ICommand ConfigurateScriptCommand { get; set; }
    public ICommand ExecuteScriptCommand { get; set; }
    public ICommand NewScriptCommand { get; set; }

    public ITool SelectedScript
    {
        get => _selectedScript;
        set => SetValue(ref _selectedScript, value);
    }

    public override void OnLoad()
    {
        base.OnLoad();

        SyncService.ToolsFilter.AllTools = _invoker.Tools.Values.ToList();
        SyncService.ToolsFilter.Tools = new(SyncService.ToolsFilter.AllTools);

        var categories = _invoker.Tools.Select(_ => _.Value.Info.Category);

        SyncService.ToolsFilter.Categories = new(categories.Distinct());
    }
}