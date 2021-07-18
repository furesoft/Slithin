using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using Slithin.Core;
using Slithin.ViewModels;
using Slithin.UI.Modals;

namespace Slithin.Controls
{
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
            return new DelegateCommand((o) =>
            {
                Open(new T(), viewModel);
            });
        }

        public static bool GetIsHost(ContentDialog target)
        {
            return object.ReferenceEquals(_host, target);
        }

        public static void Open(object content)
        {
            if (_host != null)
            {
                _host.DialogContent = content;
                _host.IsOpened = true;
            }
        }

        public static void Open(Control content, BaseViewModel viewModel)
        {
            content.DataContext = viewModel;

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
            var dc = new
            {
                CancelCommand = new DelegateCommand((_) => Close()),
                Message = msg
            };

            Open(new ErrorModal { DataContext = dc });
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

            var vm = new MessageBoxModalViewModel();

            vm.Message = message;
            vm.AcceptCommand = new DelegateCommand(_ =>
            {
                Close();
                tcs.TrySetResult(true);
            });

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Open(new MessageBoxModal(), vm);
            });

            return tcs.Task;
        }
    }
}
