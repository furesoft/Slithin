﻿using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Media;
using Slithin.Modules.PdfNotebookTools.ViewModels;
using Slithin.Modules.PdfNotebookTools.Views;
using Slithin.Modules.Tools.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.PdfNotebookTools;

public class NotebookCreationTool : ITool
{
    private readonly IDialogService _dialogService;

    public NotebookCreationTool(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public IImage Image => this.LoadImage("pdf.png");

    public ToolInfo Info => new("pdf_creator", "Notebook Creator", "PDF", "Build PDF Notebooks", true, true);

    public bool IsConfigurable => false;

    public Control? GetModal()
    {
        return null;
    }

    public async void Invoke(object data)
    {
        var vm = ServiceContainer.Current.Resolve<CreateNotebookModalViewModel>();

        if (data is ToolProperties props)
        {
            vm.Title = props["title"].ToString() ?? string.Empty;
            vm.CoverFilename = props["coverFilename"].ToString() ?? string.Empty;
            vm.Pages = new((IEnumerable<object>)props["pages"]);
            vm.RenderName = (bool)props["renderName"];

            vm.OKCommand.Execute(null);
        }
        else
        {
            var modal = new CreateNotebookModal { DataContext = vm };

            if (await _dialogService.Show("", modal))
            {
                vm.OKCommand.Execute(null);
            }
        }
    }
}
