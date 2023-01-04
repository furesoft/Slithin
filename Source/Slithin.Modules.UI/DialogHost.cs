using Avalonia.Controls;
using Slithin.Controls;
using Slithin.Core.MVVM;

namespace Slithin.Modules.UI.Models;

public static class DialogHost
{
    private static ContentDialog _host;

    public static void Close()
    {
        if (_host != null)
        {
            _host.IsOpened = false;
        }
    }

    public static bool GetIsHost(ContentDialog target)
    {
        return ReferenceEquals(_host, target);
    }

    public static void Open(object content)
    {
        if (_host == null)
        {
            return;
        }

        _host.DialogContent = content;
        _host.IsOpened = true;
    }

    public static void Open(Control content, BaseViewModel viewModel)
    {
        content.DataContext = viewModel;

        if (content.DataContext is BaseViewModel vm)
        {
            vm.Load();
        }

        Open(content);
    }

    public static void SetIsHost(ContentDialog target, bool value)
    {
        if (value)
        {
            _host = target;
        }
    }
}
