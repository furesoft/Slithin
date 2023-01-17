namespace Slithin.Modules.Menu.Models.ContextualMenu;

public abstract class ContextualElement : Slithin.Core.MVVM.NotifyObject
{
    private object? _dataContext;

    public object? DataContext
    {
        get { return _dataContext; }
        set { SetValue(ref _dataContext, value); }
    }
}
