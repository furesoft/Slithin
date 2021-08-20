using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;

namespace Slithin.UI
{
    public class NotebookDataTemplate : IDataTemplate
    {
        public IControl Build(object param)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var notebooksDir = ServiceLocator.Container.Resolve<IPathManager>().NotebooksDir;
            var cache = ServiceLocator.Container.Resolve<ICacheService>();

            var container = new StackPanel();

            container.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            container.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            container.MaxHeight = 275;

            var img = new Image
            {
                MinWidth = 25,
                MinHeight = 25
            };

            if (param is Metadata md)
            {
                if (md.Type == "DocumentType")
                {
                    if (Directory.Exists(Path.Combine(notebooksDir, md.ID + ".thumbnails")))
                    {
                        var filename = "";
                        if (md?.Content.CoverPageNumber == 0)
                        {
                            // load first page
                            filename = md.Content.Pages[0];
                        }
                        else if (md?.Content.CoverPageNumber == -1)
                        {
                            // load last page opened, set in md.LastOpenedPage
                            filename = md.Content.Pages[md.LastOpenedPage];
                        }

                        if (!string.IsNullOrEmpty(filename))
                        {
                            var thumbnail = Path.Combine(notebooksDir, md.ID + ".thumbnails", filename + ".jpg");

                            if (File.Exists(thumbnail))
                            {
                                img.Source = new Bitmap(File.OpenRead(thumbnail));
                            }
                            else
                            {
                                img.Source = cache.Get("notebook-" + md.Content.FileType, new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{md.Content.FileType}.png"))));
                            }
                        }
                    }
                    else
                    {
                        img.Source = cache.Get("notebook-" + md.Content.FileType, new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{md.Content.FileType}.png"))));
                    }
                }
                else
                {
                    img.Source = cache.Get("folder", new Bitmap(assets.Open(new Uri("avares://Slithin/Resources/folder.png"))));
                }
            }

            container.Children.Add(img);

            var title = new TextBlock
            {
                [!TextBlock.TextProperty] = new Binding("VisibleName")
            };

            title.TextAlignment = Avalonia.Media.TextAlignment.Center;
            title.TextWrapping = Avalonia.Media.TextWrapping.Wrap;

            container.Children.Add(title);

            return container;
        }

        public bool Match(object data)
        {
            return data is Metadata;
        }
    }
}
