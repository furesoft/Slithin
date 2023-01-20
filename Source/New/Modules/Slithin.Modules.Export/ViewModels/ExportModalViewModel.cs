using System.Collections.ObjectModel;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models;

namespace Slithin.Modules.Export.ViewModels;

public class ExportModalViewModel : BaseViewModel
{
    private IExportProvider? _selectedFormat;

    public ExportModalViewModel(Metadata md, IExportProviderFactory exportProviderFactory)
    {
        Notebook = md;
        Formats = new ObservableCollection<IExportProvider>(exportProviderFactory.GetAvailableProviders(md));
        SelectedFormat = Formats.FirstOrDefault();
        ExportPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }

    public string ExportPath { get; set; }
    public ObservableCollection<IExportProvider> Formats { get; set; }
    public bool IsEpubSelected { get; set; }
    public Metadata Notebook { get; set; }

    public string PagesSelector { get; set; } = "1-";

    public IExportProvider? SelectedFormat
    {
        get { return _selectedFormat; }
        set
        {
            SetValue(ref _selectedFormat, value);

            if (_selectedFormat == null)
            {
                return;
            }

            IsEpubSelected = _selectedFormat.Title.Contains("EPUB");
        }
    }
}
