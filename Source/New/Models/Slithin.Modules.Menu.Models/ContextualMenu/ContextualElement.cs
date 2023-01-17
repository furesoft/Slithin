namespace Slithin.Modules.Menu.Models.ContextualMenu;

public abstract class ContextualElement : Slithin.Core.MVVM.NotifyObject
{
    private object? _dataContext;
    private bool _isVisible = true;

    public object? DataContext
    {
        get { return _dataContext; }
        set { SetValue(ref _dataContext, value); }
    }


    public bool IsVisible
    {
        get
        {
            return _isVisible;
        }
        set
        {
            this.SetValue(ref _isVisible, value);
        }
    }
}
