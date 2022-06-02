using System.Collections.ObjectModel;
using System.Linq;
using Slithin.Core.MVVM;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;

namespace Slithin.ViewModels.Modals;

public class ExportModalViewModel : BaseViewModel
{
    public ExportModalViewModel(Metadata md, IExportProviderFactory exportProviderFactory)
    {
        Notebook = md;
        Formats = new ObservableCollection<string>(exportProviderFactory.GetAvailableProviders(md).Select(_ => _.Title));
    }

    public ObservableCollection<string> Formats { get; set; }
    public Metadata Notebook { get; set; }
}
