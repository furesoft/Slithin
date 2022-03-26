using System.Collections.Generic;
using Avalonia.Controls;
using Slithin.Core;
using Slithin.Core.ItemContext;

namespace Slithin.ContextMenus;

public class CommandBasedContextMenu : IContextProvider
{
    private readonly IContextCommand _command;

    public CommandBasedContextMenu(IContextCommand command)
    {
        _command = command;
    }

    public object ParentViewModel { get; set; }

    public bool CanHandle(object obj)
    {
        return _command.CanHandle(obj);
    }

    public ICollection<MenuItem> GetMenu(object obj)
    {
        return new List<MenuItem>(
            new MenuItem[] {
                new() {
                    Header = _command.Titel,
                    Command = new DelegateCommand(_ => _command.Invoke(obj)) } }
            );
    }
}
