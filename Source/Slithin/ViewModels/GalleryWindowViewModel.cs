using System.Collections.ObjectModel;
using System.Windows.Input;
using Slithin.Core;
using Slithin.Core.MVVM;

namespace Slithin.ViewModels;

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