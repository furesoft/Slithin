using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.UI.Tools;
using Slithin.ViewModels;

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

        public ScriptInfo Info => new("Notebook Creator", "PDF", "Build PDF Notebooks");

        public bool IsConfigurable => false;

        public Control GetModal()
        {
            var modal = new CreateNotebookModal
            {
                DataContext = new CreateNotebookModalViewModel(ServiceLocator.Container.Resolve<IPathManager>())
            };

            return modal;
        }

        public void Invoke(object data)
        {
            var modal = new CreateNotebookModal
            {
                DataContext = new CreateNotebookModalViewModel(ServiceLocator.Container.Resolve<IPathManager>())
            };

            DialogService.Open(modal);
        }
    }
}
