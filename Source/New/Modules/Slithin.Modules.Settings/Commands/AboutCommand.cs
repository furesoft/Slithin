using System.Windows.Input;
using Slithin.Modules.Settings.Modals;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Settings.Commands;

public class AboutCommand : ICommand
{
    private readonly IDialogService _dialogService;

    public AboutCommand(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        var modal = new AboutModal();
        
        await _dialogService.Show("About Slithin", modal);
    }

    public event EventHandler? CanExecuteChanged;
}
