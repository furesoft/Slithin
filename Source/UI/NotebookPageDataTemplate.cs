using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Slithin.Tools;

namespace Slithin.UI
{
    public class NotebookPageDataTemplate : IDataTemplate
    {
        public IControl Build(object param)
        {
            var grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            TextBlock title;
            if (param is NotebookPage)
            {
                title = new TextBlock
                {
                    [!TextBlock.TextProperty] = new Binding("Template.Name")
                };
            }
            else
            {
                title = new TextBlock
                {
                    [!TextBlock.TextProperty] = new Binding("ShortName")
                };
            }

            title.TextWrapping = Avalonia.Media.TextWrapping.Wrap;
            Grid.SetColumn(title, 0);

            grid.Children.Add(title);

            var count = new TextBlock
            {
                [!TextBlock.TextProperty] = new Binding("Count"),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
            };

            Grid.SetColumn(count, 1);

            grid.Children.Add(count);

            return grid;
        }

        public bool Match(object data)
        {
            return data is NotebookPage || data is NotebookCustomPage;
        }
    }
}
