using System.Windows.Input;

namespace Slithin.Modules.Menu.Models.ContextualMenu.ContextualElements;

public class ContextualButton : ContextualElement
{
    public ContextualButton(string title, string iconName, ICommand command)
    {
        Title = title;
        IconName = iconName;
        Command = command;
    }

    public ICommand Command { get; set; }
    public string IconName { get; set; }
    public string? Hint { get; set; }
    public string Title { get; set; }
}
