using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using Material.Styles;
using Slithin.Core;
using Slithin.UI.Modals;
using Slithin.ViewModels.Modals;

namespace Slithin.Controls;

public static class DialogService
{
    private static ContentDialog _host;

    public static void Close()
    {
        if (_host != null)
        {
            _host.IsOpened = false;
        }
    }

    public static ICommand CreateOpenCommand<T>(BaseViewModel viewModel)
        where T : Control, new()
    {
        return new DelegateCommand(_ =>
        {
            Open(new T(), viewModel);
        });
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

    public static void Open()
    {
        if (_host != null)
        {
            _host.IsOpened = true;
        }
    }

    public static void OpenDialogError(string message)
    {
        SnackbarHost.Post(message, "dialogError");
    }

    public static void OpenError(string msg)
    {
        var dc = new {CancelCommand = new DelegateCommand(_ => Close()), Message = msg};

        Open(new ErrorModal {DataContext = dc});
    }

    public static void SetIsHost(ContentDialog target, bool value)
    {
        if (value)
        {
            _host = target;
        }
    }

    public static Task<bool> ShowDialog(string message)
    {
        TaskCompletionSource<bool> tcs = new();

        var vm = new MessageBoxModalViewModel
        {
            Message = message,
            AcceptCommand = new DelegateCommand(_ =>
            {
                Close();
                tcs.TrySetResult(true);
            })
        };

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Open(new MessageBoxModal(), vm);
        });

        return tcs.Task;
    }

    public static Task<bool> ShowDialog(string title, object content)
    {
        TaskCompletionSource<bool> tcs = new();

        var vm = new ShowDialogModalViewModel
        {
            Title = title,
            Content = content,
            AcceptCommand = new DelegateCommand(_ =>
            {
                Close();
                tcs.TrySetResult(true);
            })
        };

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Open(new ShowDialogModal(), vm);
        });

        return tcs.Task;
    }

    public static Task<string> ShowPrompt(string header, string watermark, string defaultValue = null)
    {
        TaskCompletionSource<string> tcs = new();

        var vm = new PromptModalViewModel {Header = header, Watermark = watermark, Input = defaultValue};

        vm.AcceptCommand = new DelegateCommand(_ =>
        {
            if (!string.IsNullOrEmpty(vm.Input))
            {
                Close();
                tcs.TrySetResult(vm.Input);
            }
            else
            {
                OpenDialogError($"{vm.Watermark} cannot be empty");
            }
        });

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Open(new PromptModal(), vm);
        });

        return tcs.Task;
    }
}
