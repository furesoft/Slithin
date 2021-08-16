using System.Windows.Input;
using Slithin.Core;

namespace Slithin.ViewModels
{
    public class GalleryWindowViewModel : BaseViewModel
    {
        public GalleryWindowViewModel()
        {
            StartCommand = new DelegateCommand(_ =>
            {
                RequestClose();
            });
        }

        public ICommand StartCommand { get; set; }
    }
}
