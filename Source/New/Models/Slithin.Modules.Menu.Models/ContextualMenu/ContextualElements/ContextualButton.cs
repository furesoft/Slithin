using System.Windows.Input;
using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;

public class ContextualButton : ContextualElement
{
    private object? _commandParameter;

    public ContextualButton(string title, string iconName, ICommand command)
    {
        var localisationService = ServiceContainer.Current.Resolve<ILocalisationService>();

        Title = localisationService.GetString(title);
        Command = command;

        Icon = Application.Current.FindResource(iconName);
    }

    public ICommand Command { get; }
    public string? Hint { get; set; }
    public string Title { get; set; }

    public object Icon { get; set; }
}
