using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Controls;
using Slithin.Core;

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

        public bool IsConfigurable => true;

        public Control GetModal()
        {
            return null;
        }

        public async void Invoke(object data)
        {
            await DialogService.ShowDialog("PDF generated");
        }
    }
}
