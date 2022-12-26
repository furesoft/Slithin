using System.Collections.ObjectModel;
using Slithin.Core.ImportExport;
using Slithin.Core.MVVM;
using Slithin.Core.Services;
using Slithin.Entities.Remarkable;

namespace Slithin.Modules.Export;

public class ExportModalViewModel : BaseViewModel
{
    private IExportProvider _selectedFormat;

    public ExportModalViewModel(Metadata md, IExportProviderFactory exportProviderFactory)
    {
        Notebook = md;
        Formats = new ObservableCollection<IExportProvider>(exportProviderFactory.GetAvailableProviders(md));
        SelectedFormat = Formats.First();
        ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public string ExportPath { get; set; }
    public ObservableCollection<IExportProvider> Formats { get; set; }
    public bool IsEpubSelected { get; set; }
    public Metadata Notebook { get; set; }

    public string PagesSelector { get; set; } = "1-";

    public IExportProvider SelectedFormat
    {
        get { return _selectedFormat; }
        set
        {
            SetValue(ref _selectedFormat, value);
            IsEpubSelected = _selectedFormat.Title.Contains("EPUB");
        }
    }
}
