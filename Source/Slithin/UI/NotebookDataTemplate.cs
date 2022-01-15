using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Material.Styles;
using Slithin.Core;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;

namespace Slithin.UI;

public class NotebookDataTemplate : IDataTemplate
{
    public IControl Build(object param)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var notebooksDir = ServiceLocator.Container.Resolve<IPathManager>().NotebooksDir;
        var cache = ServiceLocator.Container.Resolve<ICacheService>();
        var contextProvider = ServiceLocator.Container.Resolve<IContextMenuProvider>();

        if (param is not Metadata md)
        {
            return null;
        }

        var stackPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            MaxHeight = 275
        };

        var img = new Image
        {
            MinWidth = 25,
            MinHeight = 25,
            MaxHeight = 150,
            HorizontalAlignment = HorizontalAlignment.Center
        };

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
                        img.Source = cache.GetObject(thumbnail, new Bitmap(File.OpenRead(thumbnail)));
                    }
                    else
                    {
                        img.Source = cache.GetObject("notebook-" + md.Content.FileType,
                            new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{md.Content.FileType}.png"))));
                    }
                }
            }
            else
            {
                img.Source = cache.GetObject("notebook-" + md.Content.FileType,
                    new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{md.Content.FileType}.png"))));
            }
        }
        else if (md.ID == "trash")
        {
            img.Source = cache.GetObject("trash",
                new Bitmap(assets.Open(new Uri("avares://Slithin/Resources/trash.png"))));
        }
        else
        {
            img.Source = cache.GetObject("folder",
                new Bitmap(assets.Open(new Uri("avares://Slithin/Resources/folder.png"))));
        }

        stackPanel.Children.Add(img);

        var title = new TextBlock { [!TextBlock.TextProperty] = new Binding("VisibleName") };

        title.TextAlignment = TextAlignment.Center;
        title.TextWrapping = TextWrapping.Wrap;

        stackPanel.Children.Add(title);

        var card = new Card { Content = stackPanel };

        card.Initialized += (s, e) =>
        {
            card.ContextMenu = contextProvider.BuildMenu(UIContext.Notebook, md, card.Parent.Parent.DataContext);
        };

        return card;
    }

    public bool Match(object data)
    {
        return data is Metadata;
    }
}
