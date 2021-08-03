using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Tools;

namespace Slithin.ViewModels
{
    public class ToolsPageViewModel : BaseViewModel
    {
        public ToolsPageViewModel()
        {
            Items.Add(new NotebookCreationTool());

            ConfigurateScriptCommand = new DelegateCommand(_ => DialogService.Open(SelectedScript.GetModal()));
            RemoveScriptCommand = new DelegateCommand(_ =>
            {
                Items.Remove(SelectedScript);
            }, _ => SelectedScript is not null);

            ExecuteScriptCommand = new DelegateCommand(_ =>
            {
                SelectedScript.Invoke(_);
            }, _ => SelectedScript is not null);
        }

        public ICommand ConfigurateScriptCommand { get; set; }
        public ICommand ExecuteScriptCommand { get; set; }
        public ObservableCollection<ITool> Items { get; set; } = new();
        public ICommand RemoveScriptCommand { get; set; }

        public ITool SelectedScript { get; set; }
    }
}
