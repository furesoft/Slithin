using System.Windows.Input;
using Slithin.Core;

namespace Slithin.Modules.Settings.UI.Commands;

public class BuyCoffeeCommand : ICommand
{
    public bool CanExecute(object? parameter) => true;
    public void Execute(object? parameter)
    {
        Utils.OpenUrl("https://www.buymeacoffee.com/furesoft");
    }

    public event EventHandler? CanExecuteChanged;
}
