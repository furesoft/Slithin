using Slithin.Core.MVVM;

namespace Slithin.ViewModels;

public class FirstStartViewModel : BaseViewModel
{
    private int _index;

    public int Index
    {
        get { return _index; }
        set { SetValue(ref _index, value); }
    }
}
