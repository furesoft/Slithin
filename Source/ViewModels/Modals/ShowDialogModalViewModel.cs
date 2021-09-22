using System.Windows.Input;
using Slithin.Core;

namespace Slithin.ViewModels.Modals
{
    public class ShowDialogModalViewModel : BaseViewModel
    {
        private object _content;
        private string _title;
        public ICommand AcceptCommand { get; set; }

        public object Content
        {
            get => _content;
            set => SetValue(ref _content, value);
        }

        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }
    }
}
