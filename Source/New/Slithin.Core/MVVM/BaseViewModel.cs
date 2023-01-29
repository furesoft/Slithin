using AuroraModularis.Core;
using Avalonia.Controls;

namespace Slithin.Core.MVVM;

public abstract class BaseViewModel : NotifyObject
{
    public event Action OnRequestClose;

    public bool IsLoaded { get; set; }

    public static void ApplyViewModel<T>(Control control)
        where T : BaseViewModel
    {
        var vm = ServiceContainer.Current.Resolve<T>();

        vm.OnLoad();

        if (control is Window win)
        {
            vm.OnRequestClose += () =>
            {
                win.Close();
            };
        }

        control.DataContext = vm;
    }

    public void Load()
    {
        if (!IsLoaded)
        {
            IsLoaded = true;

            OnLoad();
        }
    }

    public virtual void OnLoad()
    {
    }

    protected void RequestClose()
    {
        OnRequestClose?.Invoke();
    }
}
