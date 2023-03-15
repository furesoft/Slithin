﻿using Avalonia.Controls;
using Slithin.Core.MVVM;

namespace Slithin.Modules.Menu.Models.ItemContext;

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
        return _command.CanExecute(obj);
    }

    public ICollection<MenuItem> GetMenu(object obj)
    {
        _command.ParentViewModel = ParentViewModel;

        return new List<MenuItem>(
            new MenuItem[] {
                new() {
                    Header = _command.Title,
                    Command = new DelegateCommand(_ => _command.Execute(obj)) } }
            );
    }
}
