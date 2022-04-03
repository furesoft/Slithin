using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.UI.Modals;
using Slithin.ViewModels.Modals;
using Slithin.Core.MVVM;
using Slithin.Core.Tools;

namespace Slithin.ViewModels.Pages;

public class ToolsPageViewModel : BaseViewModel
{
    private readonly ToolInvoker _invoker;
    private ITool _selectedScript;

    public ToolsPageViewModel(ToolInvoker invoker)
    {
        ConfigurateScriptCommand = new DelegateCommand(ShowConfigModal,
            _ => _ is ITool tool && tool.IsConfigurable);
        NewScriptCommand =
            new DelegateCommand(_ => DialogService.Open(new NewScriptModal(), new NewScriptModalViewModel()));

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

        SyncService.ToolsFilter.AllTools = _invoker.Tools.Values.Where(_ => _.Info.IsListed).ToList();
        SyncService.ToolsFilter.Tools = new ObservableCollection<ITool>(SyncService.ToolsFilter.AllTools);

        var categories = _invoker.Tools.Where(_ => _.Value.Info.IsListed).Select(_ => _.Value.Info.Category);

        SyncService.ToolsFilter.Categories = new ObservableCollection<string>(categories.Distinct());
    }

    private static void ShowConfigModal(object _)
    {
        var tool = ((ITool)_);
        var content = tool.GetModal();

        var dc = new DialogControl();
        dc.Header = "Config " + tool.Info.Name;
        dc.Content = content;
        dc.IsCancelEnabled = true;
        dc.CommandText = "OK";
        dc.MinHeight = 300;
        dc.MaxWidth = 500;

        var vm = new ModalBaseViewModel();

        DialogService.Open(dc, vm);
    }
}
