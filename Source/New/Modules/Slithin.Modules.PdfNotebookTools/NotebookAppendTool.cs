using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Media;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.PdfNotebookTools.ViewModels;
using Slithin.Modules.PdfNotebookTools.Views;
using Slithin.Modules.Tools.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.PdfNotebookTools;

[Context(UIContext.Notebook)]
public class NotebookAppendTool : ITool, IContextProvider
{
    private readonly IDialogService _dialogService;

    public NotebookAppendTool(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public IImage Image => this.LoadImage("pdf_append.png");

    public ToolInfo Info => new("pdf_append", "PDF Appendor", "PDF", "Append Pages To PDF", true, true);
    public bool IsConfigurable => false;
    public object ParentViewModel { get; set; }

    public bool CanHandle(object obj)
    {
        return obj is FileSystemModel {Tag: Metadata {Content.FileType: "pdf"}};
    }

    public ICollection<MenuItem> GetMenu(object obj)
    {
        var menu = new List<MenuItem>
        {
            new() {Header = "Append Pages", Command = new DelegateCommand(_ => { Invoke(obj); })}
        };

        return menu;
    }

    public Control? GetModal()
    {
        return null;
    }

    public async void Invoke(object data)
    {
        var modal = new AppendNotebookModal();
        var vm = Container.Current.Resolve<AppendNotebookModalViewModel>();

        if (data is ToolProperties props)
        {
            vm.ID = props["id"].ToString() ?? string.Empty;
            vm.Pages = new((IEnumerable<object>)props["pages"]);
            
            vm.OKCommand.Execute(null);
        }
        else
        {
            if (data is Metadata md)
            {
                vm.ID = md.ID; //Pre select the notebook, if it is executed from contextmenu
            }

            if (await _dialogService.Show("", modal))
            {
                vm.OKCommand.Execute(null);
            }
        }
    }
}
