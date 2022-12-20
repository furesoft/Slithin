using Avalonia.Controls;
using Avalonia.Threading;
using Slithin.Core.MVVM;
using Slithin.Modules.UI.Models;
using Slithin.ViewModels.Modals;

namespace Slithin.Modules.UI;

internal class DialogServiceImpl : IDialogService
{
    public Task<bool> Show(string title, Control content)
    {
        TaskCompletionSource<bool> tcs = new();

        var vm = new ShowDialogModalViewModel
        {
            Title = title,
            Content = content,
            AcceptCommand = new DelegateCommand(_ =>
            {
                Models.DialogHost.Close();
                tcs.TrySetResult(true);
            }),
            CancelCommand = new DelegateCommand(_ =>
            {
                Models.DialogHost.Close();
                tcs.TrySetResult(false);
            })
        };

        Dispatcher.UIThread.InvokeAsync(() =>
        {
            Models.DialogHost.Open(new ShowDialogModal(), vm);
        });

        return tcs.Task;
    }
}
