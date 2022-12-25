using Avalonia.Controls;
using Avalonia.Threading;
using Slithin.Core.MVVM;
using Slithin.Modules.UI;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Notifications;

internal class StatusController : IStatusController
{
    private readonly bool _showInNewWindow;
    private StatusViewModel _viewModel = new();
    private Window _window;

    public StatusController(bool showInNewWindow)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var modal = new StatusModal();
            modal.DataContext = _viewModel;

            if (showInNewWindow)
            {
                _window = new Window();
                _window.Height = 150;
                _window.Width = 350;
                _window.Title = "Slithin Update";
                _window.Content = modal;
                _window.HasSystemDecorations = false;
                _window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _window.Show();
            }
            else
            {
                UI.Models.DialogHost.Open(modal);
            }
        });
        _showInNewWindow = showInNewWindow;
    }

    ~StatusController()
    {
        Finish();
    }

    public StatusViewModel ViewModel => _viewModel;

    public void Finish()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (_showInNewWindow)
            {
                _window?.Close();
                return;
            }

            UI.Models.DialogHost.Close();
        }
        );
    }

    public void Step(string message)
    {
        _viewModel.Message = message;
    }

    public class StatusViewModel : NotifyObject
    {
        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }
    }
}
