using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;

namespace Slithin.ViewModels.Pages
{
    public class ToolsPageViewModel : BaseViewModel
    {
        private readonly ToolInvoker _invoker;
        private ITool _selectedScript;

        public ToolsPageViewModel(ToolInvoker invoker)
        {
            ConfigurateScriptCommand = new DelegateCommand(_ => DialogService.Open(SelectedScript.GetModal()), _ => _ is ITool tool && tool.IsConfigurable);
            RemoveScriptCommand = new DelegateCommand(_ =>
            {
                Items.Remove(((ITool)_));
            }, _ => false);

            ExecuteScriptCommand = new DelegateCommand(_ =>
            {
                ((ITool)_).Invoke(_);
            }, _ => _ is not null);

            _invoker = invoker;
        }

        public ICommand ConfigurateScriptCommand { get; set; }

        public ICommand ExecuteScriptCommand { get; set; }

        public ObservableCollection<ITool> Items { get; set; } = new();

        public ICommand RemoveScriptCommand { get; set; }

        public ITool SelectedScript
        {
            get => _selectedScript;
            set => SetValue(ref _selectedScript, value);
        }

        public override void OnLoad()
        {
            base.OnLoad();

            foreach (var tool in _invoker.Tools)
            {
                Items.Add(tool.Value);
            }
        }
    }
}
