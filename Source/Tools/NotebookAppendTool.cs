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
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Models;
using Slithin.UI.Tools;
using Slithin.ViewModels.Modals;

namespace Slithin.Tools
{
    [Context(UIContext.Notebook)]
    public class NotebookAppendTool : ITool, IContextProvider
    {
        public IImage Image
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/pdf_append.png")));
            }
        }

        public ScriptInfo Info => new("pdf_append", "Notebook Appendor", "PDF", "Append Pages To PDF", true);

        public bool IsConfigurable => false;

        public object ParentViewModel { get; set; }

        public bool CanHandle(object obj) => obj is Metadata md && md.Content.FileType == "pdf";

        public ICollection<MenuItem> GetMenu(object obj)
        {
            var menu = new List<MenuItem>()
            {
                new MenuItem{Header = "Append Pages", Command = new DelegateCommand((_) =>{Invoke(obj);})}
            };

            return menu;
        }

        public Control GetModal()
        {
            return null;
        }

        public void Invoke(object data)
        {
            var modal = new AppendNotebookModal();
            var vm = ServiceLocator.Container.Resolve<AppendNotebookModalViewModel>();

            if (data is ToolProperties props)
            {
                vm.ID = props["id"].ToString();
                vm.Pages = new ObservableCollection<object>((IEnumerable<object>)props["pages"]);

                vm.OKCommand.Execute(null);
            }
            else
            {
                if (data is Metadata md)
                {
                    vm.ID = md.ID; //Pre select the notebook, if it is executed from contextmenu
                }

                DialogService.Open(modal, vm);
            }
        }
    }
}
