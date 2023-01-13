using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;

public class ContextualButton : ContextualElement
{
    public ContextualButton(string title, string iconName, ICommand command)
    {
        Title = title;
        Command = command;

        Icon = (GeometryDrawing)Application.Current.FindResource(iconName);
    }

    public ICommand Command { get; set; }
    public string? Hint { get; set; }
    public string Title { get; set; }
    public object? CommandParameter { get; set; }
    public GeometryDrawing Icon { get; set; }
}
