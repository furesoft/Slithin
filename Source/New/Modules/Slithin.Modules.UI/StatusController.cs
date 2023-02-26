using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using Slithin.Core.MVVM;
using Slithin.Modules.UI.Modals;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.UI;

internal class StatusController : IStatusController
{
    private readonly bool _showInNewWindow;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private StatusViewModel _viewModel = new();
    private Window _window;

    public StatusController(bool showInNewWindow)
    {
        _cancellationTokenSource = new();
        _cancellationTokenSource.Token.Register(Finish);

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var modal = new StatusModal();
            _viewModel.CancellationTokenSource = _cancellationTokenSource;

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
                _window.Closing += (s, e) =>
                {
                    Cancel();
                };
                _window.Show();
            }
            else
            {
                Models.DialogHost.Open(modal);
            }
        });
        _showInNewWindow = showInNewWindow;
    }

    ~StatusController()
    {
        Finish();
    }

    public StatusViewModel ViewModel => _viewModel;

    public CancellationToken Token => _cancellationTokenSource.Token;

    public void Cancel()
    {
        _cancellationTokenSource.Cancel();

        Finish();
    }

    public void Finish()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (_showInNewWindow)
            {
                _window?.Close();
                return;
            }

            Models.DialogHost.Close();
        }
        );
    }

    public void Step(string message)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            _viewModel.Message = message;
        });
    }

    public class StatusViewModel : NotifyObject
    {
        private string _message;

        public StatusViewModel()
        {
            CancelCommand = new DelegateCommand(Cancel);
        }

        public string Message
        {
            get { return _message; }
            set { SetValue(ref _message, value); }
        }

        public ICommand CancelCommand { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }

        private void Cancel(object obj)
        {
            CancellationTokenSource.Cancel();
        }
    }
}
