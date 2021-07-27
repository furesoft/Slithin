using System.Windows.Input;
using Slithin.Core;

namespace Slithin.ViewModels
{
    public class MakeFolderModalViewModel : BaseViewModel
    {
        private string _name;

        public ICommand MakeFolderCommand { get; set; }

        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }
    }
}
