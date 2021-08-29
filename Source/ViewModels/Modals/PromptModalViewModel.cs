using System.Windows.Input;
using Slithin.Core;

namespace Slithin.ViewModels.Modals
{
    public class PromptModalViewModel : BaseViewModel
    {
        private string _header;
        private string _input;
        private string _watermark;
        public ICommand AcceptCommand { get; set; }

        public string Header
        {
            get { return _header; }
            set { SetValue(ref _header, value); }
        }

        public string Input
        {
            get { return _input; }
            set { _input = value; }
        }

        public string Watermark
        {
            get { return _watermark; }
            set { SetValue(ref _watermark, value); }
        }
    }
}
