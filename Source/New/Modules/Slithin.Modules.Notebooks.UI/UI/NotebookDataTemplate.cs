using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;
using Material.Styles;
using Slithin.Controls;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.Menu.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.Models;

namespace Slithin.Modules.Notebooks.UI.UI;

internal class NotebookDataTemplate : IDataTemplate
{
    public IControl Build(object param)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var notebooksDir = ServiceContainer.Current.Resolve<IPathManager>().NotebooksDir;
        var cache = ServiceContainer.Current.Resolve<ICacheService>();
        var contextProvider = ServiceContainer.Current.Resolve<IContextMenuProvider>();
        var thumbnailLoader = ServiceContainer.Current.Resolve<IThumbnailLoader>();

        if (param is not FileSystemModel fsm)
        {
            return null;
        }

        var stackPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            MaxHeight = 275
        };

        var img = CreateImageControl(fsm);

        var titlePanel = new Grid();

        var title = new TextBlock
        {
            [!TextBlock.TextProperty] = new Binding("VisibleName"),
            TextAlignment = TextAlignment.Center,
            TextWrapping = TextWrapping.Wrap,
            Width = 125,
            VerticalAlignment = VerticalAlignment.Center,
            FontWeight = FontWeight.Bold,
            Height = 50
        };

        var favImage = new Image();

        CreatePinnedStar(fsm, favImage, titlePanel, title);

        titlePanel.Children.Add(favImage);

        LoadThumbnail(fsm, img, thumbnailLoader);

        titlePanel.Children.Add(title);

        stackPanel.Children.Add(titlePanel);
        stackPanel.Children.Add(img);

        var card = new Card
        {
            Content = stackPanel,
            Background = (IBrush)new BrushConverter().ConvertFromString("#e2e2e2")
        };

        card.Initialized += (s, e) =>
        {
            card.ContextMenu = contextProvider.BuildMenu(UIContext.Notebook, fsm, card.Parent.Parent.DataContext);
        };

        return card;
    }

    public bool Match(object data)
    {
        return data is FileSystemModel;
    }

    private static void LoadThumbnail(FileSystemModel fsm, dynamic img, IThumbnailLoader thumbnailLoader)
    {
        if (fsm is FileModel)
        {
            img.Source = thumbnailLoader.LoadImage(fsm);
        }
        else if (fsm is TrashModel)
        {
            img.Margin = new Thickness(10);
            img.Source = new DrawingImage((GeometryDrawing)Application.Current.FindResource("Cool.TrashFull"));
        }
        else
        {
            img.Margin = new Thickness(10);
            img.Source = new DrawingImage((GeometryDrawing)Application.Current.FindResource("Bootstrap.FolderFill"));
        }
    }

    private static void CreatePinnedStar(FileSystemModel fsm, Image favImage, Grid titlePanel, TextBlock title)
    {
        if (fsm.IsPinned)
        {
            favImage.Source = new DrawingImage((GeometryDrawing)Application.Current.FindResource("Entypo+.Star"));
            titlePanel.ColumnDefinitions.Add(new(new GridLength(20)));
            titlePanel.ColumnDefinitions.Add(new(new GridLength(125, GridUnitType.Star)));

            Grid.SetColumn(title, 1);
            Grid.SetColumn(favImage, 0);
        }
        else
        {
            title.HorizontalAlignment = HorizontalAlignment.Center;
        }

        favImage.Width = 20;
        favImage.Height = 20;
        favImage.HorizontalAlignment = HorizontalAlignment.Left;
        favImage.VerticalAlignment = VerticalAlignment.Top;
    }

    private static dynamic CreateImageControl(FileSystemModel fsm)
    {
        dynamic img = new Image()
        {
            MinWidth = 25,
            MinHeight = 25,
            MaxHeight = 135,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom,
        };

        if (fsm is FileModel)
        {
            img = new PreviewImageControl
            {
                MinWidth = 25,
                MinHeight = 25,
                MaxHeight = 135,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
        }

        return img;
    }
}
