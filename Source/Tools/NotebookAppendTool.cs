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
    public class NotebookAppendTool : ITool
    {
        public IImage Image
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/pdf_append.png")));
            }
        }

        public ScriptInfo Info => new("Notebook Appendor", "PDF", "Append Pages To PDF");

        public bool IsConfigurable => false;

        public Control GetModal()
        {
            return null;
        }

        public void Invoke(object data)
        {
            var modal = new AppendNotebookModal();

            DialogService.Open(modal, ServiceLocator.Container.Resolve<AppendNotebookModalViewModel>());
        }
    }
}
