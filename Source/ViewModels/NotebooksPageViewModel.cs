using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;
using Slithin.Messages;

namespace Slithin.ViewModels
{
    public class NotebooksPageViewModel : BaseViewModel
    {
        private Metadata _selectedNotebook;

        public NotebooksPageViewModel()
        {
            //ImportCommand = DialogService.CreateOpenCommand<ImportNotebookModal>(new AddTemplateModalViewModel());
            RemoveTemplateCommand = new RemoveNotebookCommand(this);

            foreach (var md in Directory.GetFiles(ServiceLocator.NotebooksDir, "*.metadata", SearchOption.AllDirectories))
            {
                var mdObj = JsonConvert.DeserializeObject<Metadata>(File.ReadAllText(md));
                mdObj.ID = Path.GetFileNameWithoutExtension(md);
                MetadataStorage.Add(mdObj);
            }

            foreach (var md in MetadataStorage.GetByParent(""))
            {
                SyncService.NotebooksFilter.Documents.Add(md);
            }

            SyncService.NotebooksFilter.SortByFolder();

            ExportCommand = new DelegateCommand(_ => ServiceLocator.Mailbox.Post(new DownloadAllNotebooksMessage(null)));
        }

        public ICommand ExportCommand { get; set; }
        public ICommand ImportCommand { get; set; }
        public ICommand RemoveTemplateCommand { get; set; }

        public Metadata SelectedNotebook
        {
            get { return _selectedNotebook; }
            set { SetValue(ref _selectedNotebook, value); }
        }
    }
}
