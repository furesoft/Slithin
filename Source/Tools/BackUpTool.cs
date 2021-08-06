using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Core;

namespace Slithin.Tools
{
    public class BackupTool : ITool
    {
        public IImage Image
        {
            get
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/backup.png")));
            }
        }

        public ScriptInfo Info => new ScriptInfo("Backup", "Internal", "Backup all your files");

        public bool IsConfigurable => false;

        public Control GetModal()
        {
            return null;
        }

        public void Invoke(object data)
        {
        }
    }
}
