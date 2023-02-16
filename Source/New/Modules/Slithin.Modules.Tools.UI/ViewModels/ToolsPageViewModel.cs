using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Core.MVVM;
using Slithin.Modules.Tools.Models;
using Slithin.Modules.Tools.UI.Models;

namespace Slithin.Modules.Tools.UI.ViewModels;

internal class ToolsPageViewModel : BaseViewModel
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

    public ToolsFilter Filter { get; set; } = new();

    public ITool SelectedScript
    {
        get => _selectedScript;
        set => SetValue(ref _selectedScript, value);
    }

    protected override void OnLoad()
    {
        _invoker.Init();

        Filter.AllTools = _invoker.Tools.Values.Where(_ => _.Info.IsListed).ToList();
        Filter.Tools = new ObservableCollection<ITool>(Filter.AllTools);

        var categories = _invoker.Tools.Where(_ => _.Value.Info.IsListed).Select(_ => _.Value.Info.Category);

        Filter.Categories = new ObservableCollection<string>(categories.Distinct());
    }

    /*

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
