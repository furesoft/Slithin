using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Controls;
using Slithin.Core;
using Slithin.UI.Tools;

namespace Slithin.Tools
{
    public class NotebookCreationTool : ITool
    {
        public IImage Image
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/pdf.png"))); ;
            }
        }

        public ScriptInfo Info => new ScriptInfo("Notebook Creator", "Internal", "Build PDF Notebooks");

        public bool IsConfigurable => false;

        public Control GetModal()
        {
            return null;
        }

        public void Invoke(object data)
        {
            var modal = new CreateNotebookModal();
            DialogService.Open(modal);
        }
    }
}
