using Avalonia.Threading;
using Slithin.Core.MVVM;
using Slithin.Modules.UI;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Notifications;

internal class StatusController : IStatusController
{
    private StatusViewModel _viewModel = new();

    public StatusController()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var modal = new StatusModal();
            modal.DataContext = _viewModel;

            UI.Models.DialogHost.Open(modal);
        });
    }

    ~StatusController()
    {
        Finish();
    }

    public StatusViewModel ViewModel => _viewModel;

    public void Finish()
    {
        Dispatcher.UIThread.InvokeAsync(UI.Models.DialogHost.Close);
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
