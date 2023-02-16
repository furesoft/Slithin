using System.Windows.Input;
using Slithin.Modules.Settings.UI.Modals;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Settings.UI.Commands;

internal class ShowDonationButtonsCommand : ICommand
{
    private readonly IDialogService _dialogService;

    public ShowDonationButtonsCommand(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        await _dialogService.Show("Support", new SupportModal { DataContext = parameter });
    }

    public event EventHandler? CanExecuteChanged;
}
