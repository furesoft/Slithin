using AuroraModularis.Core;
using Avalonia.Controls;

namespace Slithin.Core.MVVM;

public abstract class BaseViewModel : NotifyObject
{
    public event Action OnRequestClose;

    public bool IsLoaded { get; set; }

    public static void ApplyViewModel<T>(Control control, T vm)
        where T : BaseViewModel
    {
        if (control is Window win)
        {
            vm.OnRequestClose += () =>
            {
                win.Close();
            };
        }

        vm.Load();
        
        if (!Design.IsDesignMode)
        {
            control.DataContext = vm;
        }
    }
    public static void ApplyViewModel<T>(Control control)
        where T : BaseViewModel
    {
        var vm = ServiceContainer.Current.Resolve<T>();

        ApplyViewModel<T>(control, vm);
    }

    public void Load()
    {
        if (!IsLoaded)
        {
            IsLoaded = true;

            OnLoad();
        }
    }

    protected virtual void OnLoad()
    {
    }

    protected void RequestClose()
    {
        OnRequestClose?.Invoke();
    }
}
