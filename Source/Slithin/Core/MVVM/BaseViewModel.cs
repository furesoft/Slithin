using System;
using LiteDB;
using Slithin.Core.Sync;
using Slithin.Core;
using Slithin.Core.MVVM;

namespace Slithin.Core.MVVM;

public abstract class BaseViewModel : NotifyObject
{
    public event Action OnRequestClose;

    public bool IsLoaded { get; set; }

    [BsonIgnore]
    public SynchronisationService SyncService => ServiceLocator.SyncService;

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
