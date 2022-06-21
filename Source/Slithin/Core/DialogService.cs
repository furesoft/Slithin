using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Slithin.Controls;
using Slithin.Core.MVVM;
using Slithin.UI.Modals;
using Slithin.UI.Views;
using Slithin.ViewModels.Modals;

namespace Slithin.Core;

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

    public static void OpenError(string msg)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            var dc = new MessageBoxModalViewModel { Message = msg };

            var errorWindow = new ErrorWindow();
            errorWindow.Content = new ErrorModal();

            dc.AcceptCommand = new DelegateCommand(_ => errorWindow.Close());
            dc.CancelCommand = dc.AcceptCommand;
            errorWindow.DataContext = dc;

            errorWindow.Show();
        });
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
            }),
            CancelCommand = new DelegateCommand(_ =>
            {
                Close();
                tcs.TrySetResult(default);
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
            }),
            CancelCommand = new DelegateCommand(_ =>
            {
                Close();
                tcs.TrySetResult(default);
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

        var vm = new PromptModalViewModel { Header = header, Watermark = watermark, Input = defaultValue };

        vm.AcceptCommand = new DelegateCommand(_ =>
        {
            if (!string.IsNullOrEmpty(vm.Input))
            {
                Close();
                tcs.TrySetResult(vm.Input);
            }
            else
            {
                OpenError($"{vm.Watermark} cannot be empty");
            }
        });
        vm.CancelCommand = new DelegateCommand(_ =>
        {
            Close();
            tcs.TrySetResult(default);
        });

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Open(new PromptModal(), vm);
        });

        return tcs.Task;
    }
}
