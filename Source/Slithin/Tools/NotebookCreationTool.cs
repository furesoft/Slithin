using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Models;
using Slithin.UI.Tools;
using Slithin.ViewModels.Modals;

namespace Slithin.Tools
{
    public class NotebookCreationTool : ITool
    {
        public IImage Image
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/pdf.png")));
            }
        }

        public ScriptInfo Info => new("pdf_creator", "Notebook Creator", "PDF", "Build PDF Notebooks", true);

        public bool IsConfigurable => false;

        public Control GetModal()
        {
            return null;
        }

        public void Invoke(object data)
        {
            var vm = ServiceLocator.Container.Resolve<CreateNotebookModalViewModel>();

            if (data is ToolProperties props)
            {
                vm.Title = props["title"].ToString();
                vm.CoverFilename = props["coverFilename"].ToString();
                vm.Pages = new ObservableCollection<object>((IEnumerable<object>)props["pages"]);
                vm.RenderName = (bool)props["renderName"];

                vm.OKCommand.Execute(null);
            }
            else
            {
                var modal = new CreateNotebookModal();

                DialogService.Open(modal, vm);
            }
        }
    }
}
