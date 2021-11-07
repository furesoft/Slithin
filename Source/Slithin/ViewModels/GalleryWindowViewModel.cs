using System.Collections.ObjectModel;
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

        public ObservableCollection<object> Slides { get; set; } = new();
        public ICommand StartCommand { get; set; }
    }
}
