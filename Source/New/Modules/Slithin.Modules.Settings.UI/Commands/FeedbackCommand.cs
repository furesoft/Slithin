using System.Windows.Input;

namespace Slithin.Modules.Settings.UI.Commands;

public class FeedbackCommand : ICommand
{
    public bool CanExecute(object? parameter) => true;
    public void Execute(object? parameter)
    {
        var feedbackWindow = new FeedbackWindow();
        feedbackWindow.Show();
    }

    public event EventHandler? CanExecuteChanged;
}
