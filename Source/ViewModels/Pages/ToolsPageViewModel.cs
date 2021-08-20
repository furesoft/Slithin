using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Scripting;

namespace Slithin.ViewModels.Pages
{
    public class ToolsPageViewModel : BaseViewModel
    {
        private ITool _selectedScript;

        public ToolsPageViewModel()
        {
            foreach (var tool in Utils.Find<ITool>())
            {
                Items.Add(tool);
            }

            ConfigurateScriptCommand = new DelegateCommand(_ => DialogService.Open(SelectedScript.GetModal()), _ => _ is ITool tool && tool.IsConfigurable);
            RemoveScriptCommand = new DelegateCommand(_ =>
            {
                Items.Remove(((ITool)_));
            }, _ => false);

            ExecuteScriptCommand = new DelegateCommand(_ =>
            {
                ((ITool)_).Invoke(_);
            }, _ => _ is not null);
        }

        public ICommand ConfigurateScriptCommand { get; set; }
        public ICommand ExecuteScriptCommand { get; set; }
        public ObservableCollection<ITool> Items { get; set; } = new();
        public ICommand RemoveScriptCommand { get; set; }

        public ITool SelectedScript
        {
            get { return _selectedScript; }
            set { SetValue(ref _selectedScript, value); }
        }
    }
}
