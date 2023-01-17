using System.Windows.Input;
using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;

public class ContextualButton : ContextualElement
{
    private object? _commandParameter;

    public ContextualButton(string title, string iconName, ICommand command)
    {
        var localisationService = Container.Current.Resolve<ILocalisationService>();

        Title = localisationService.GetString(title);
        Command = command;

        Icon = (GeometryDrawing)Application.Current.FindResource(iconName);
    }

    public ICommand Command { get; set; }
    public string? Hint { get; set; }
    public string Title { get; set; }

    public GeometryDrawing Icon { get; set; }
}
