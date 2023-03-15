using Avalonia.Controls;
using Avalonia.Threading;
using Slithin.Core.MVVM;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.UI.Modals;
using Slithin.Modules.UI.Models;
using Slithin.Modules.UI.ViewModels;

namespace Slithin.Modules.UI.Implementations;

internal class DialogServiceImpl : IDialogService
{
    public Task<bool> Show(TranslatedString title, Control content)
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
            if (content.DataContext is BaseViewModel dvm)
            {
                dvm.Load();
            }

            Models.DialogHost.Open(new ShowDialogModal(), vm);
        });

        return tcs.Task;
    }

    public async Task<string> ShowPrompt(TranslatedString title, TranslatedString message, string defaultValue = "")
    {
        var vm = new PromptModalViewModel();
        vm.Header = title;
        vm.Input = defaultValue;
        vm.Watermark = message;

        var modal = new PromptModal();
        modal.DataContext = vm;

        if (await Show(title, modal))
        {
            return vm.Input;
        }

        return string.Empty;
    }
}
