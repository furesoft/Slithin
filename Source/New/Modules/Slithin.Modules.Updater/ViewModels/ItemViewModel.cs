using Slithin.Core.MVVM;

namespace Slithin.Modules.Updater.ViewModels;

internal class ItemViewModel : BaseViewModel
{
    private int _progress;
    private string _name;

    private bool _isDone;

    public int Progress
    {
        get { return _progress; }
        set { this.SetValue(ref _progress, value); IsDone = _progress >= 100; }
    }

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public bool IsDone
    {
        get { return _isDone; }
        set { this.SetValue(ref _isDone, value); }
    }
}
