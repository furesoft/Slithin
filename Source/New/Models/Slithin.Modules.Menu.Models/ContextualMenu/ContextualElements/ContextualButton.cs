using System.Windows.Input;
using AuroraModularis.Core;
using Avalonia;
using Avalonia.Controls;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;

public class ContextualButton : ContextualElement
{
    private object? _commandParameter;

    public ContextualButton(TranslatedString title, string iconName, ICommand command)
    {
        Title = title;
        Command = command;

        Icon = Application.Current.FindResource(iconName);
    }

    public ICommand Command { get; }
    public TranslatedString? Hint { get; set; }
    public TranslatedString Title { get; set; }

    public object Icon { get; set; }
}
