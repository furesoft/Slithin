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

namespace Slithin.UI
{
    public class NotebookDataTemplate : IDataTemplate
    {
        public IControl Build(object param)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            var container = new StackPanel();

            var img = new Image();
            img.MinWidth = 25;
            img.MinHeight = 25;

            if (param is Metadata md)
            {
                if (md.Type == "DocumentType")
                {
                    if (Directory.Exists(Path.Combine(ServiceLocator.NotebooksDir, md.ID + ".thumbnails")))
                    {
                        string filename = "";
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
                            img.Source = new Bitmap(File.OpenRead(Path.Combine(ServiceLocator.NotebooksDir, md.ID + ".thumbnails", filename + ".jpg")));
                        }

                        if (md.Pinned)
                        {
                            var pinnedImg = new Image();
                            pinnedImg.Source = new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/star.png")));
                            pinnedImg.Width = 25;
                            pinnedImg.Height = 25;

                            //container.Children.Add(pinnedImg);
                        }
                    }
                    else
                    {
                        img.Source = new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{md.Content.FileType}.png")));
                    }
                }
                else
                {
                    img.Source = new Bitmap(assets.Open(new Uri("avares://Slithin/Resources/folder.png")));
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
