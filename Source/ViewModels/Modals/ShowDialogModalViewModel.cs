using System.Windows.Input;
using ReactiveUI.Fody.Helpers;
using Slithin.Core;

namespace Slithin.ViewModels.Modals
{
    public class ShowDialogModalViewModel : BaseViewModel
    {
        public ICommand AcceptCommand { get; set; }

        [Reactive]
        public object Content { get; set; }

        [Reactive]
        public string Title { get; set; }
    }
}
