using System.Windows.Input;
using Slithin.Core.MVVM;
using Slithin.Modules.Tools.Models;

namespace Slithin.Modules.Tools.UI;

public class ToolsPageViewModel : BaseViewModel
{
    private readonly IToolInvokerService _invoker;
    private ITool _selectedScript;

    public ToolsPageViewModel(IToolInvokerService invoker)
    {
        //ConfigurateScriptCommand = new DelegateCommand(ShowConfigModal,
        //   _ => _ is ITool tool && tool.IsConfigurable);

        ExecuteScriptCommand = new DelegateCommand(_ =>
        {
            ((ITool)_).Invoke(_);
        }, _ => _ is not null);

        _invoker = invoker;
    }

    public ICommand ConfigurateScriptCommand { get; set; }

    public ICommand ExecuteScriptCommand { get; set; }

    public ITool SelectedScript
    {
        get => _selectedScript;
        set => SetValue(ref _selectedScript, value);
    }

    /*

    public override void OnLoad()
    {
        base.OnLoad();

        ToolsFilter.AllTools = _invoker.Tools.Values.Where(_ => _.Info.IsListed).ToList();
        ToolsFilter.Tools = new ObservableCollection<ITool>(ToolsFilter.AllTools);

        var categories = _invoker.Tools.Where(_ => _.Value.Info.IsListed).Select(_ => _.Value.Info.Category);

        ToolsFilter.Categories = new ObservableCollection<string>(categories.Distinct());
    }

    private static void ShowConfigModal(object _)
    {
        var tool = (ITool)_;
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
    */
}
