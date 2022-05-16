using System;

namespace Slithin.Core.MVVM;

public abstract class BaseViewModel : NotifyObject
{
    public event Action OnRequestClose;

    public bool IsLoaded { get; set; }

    public void Load()
    {
        if (!IsLoaded)
        {
            OnLoad();
        }
    }

    public virtual void OnLoad()
    {
        IsLoaded = true;
    }

    protected void RequestClose()
    {
        OnRequestClose?.Invoke();
    }
}
