using System.Windows.Input;
using ReactiveUI.Fody.Helpers;
using Slithin.Core;

namespace Slithin.ViewModels.Modals
{
    public class MessageBoxModalViewModel : BaseViewModel
    {
        public ICommand AcceptCommand { get; set; }

        [Reactive]
        public string Message { get; set; }
    }
}
