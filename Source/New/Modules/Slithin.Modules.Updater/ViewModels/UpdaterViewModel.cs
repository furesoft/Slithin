using System.Collections.ObjectModel;
using Avalonia.Threading;
using Slithin.Core.MVVM;

namespace Slithin.Modules.Updater.ViewModels;

internal class UpdaterViewModel : BaseViewModel
{
    public UpdaterViewModel()
    {
        
    }

    public ObservableCollection<ItemViewModel> Items { get; set; } = new();

    public override async void OnLoad()
    {
        var packages = await UpdateRepository.GetUpdatablePackages();

        foreach (var package in packages)
        {
            Items.Add(new() {Name = package.Key });
        }

        await Task.Run(async () =>
        {
            for (int i =  0; i < 100; i++)
            {
                for (var index = 0; index < Items.Count; index++)
                {
                    var item = Items[index];
                    
                    await Dispatcher.UIThread.InvokeAsync(async () =>
                    {
                        item.Progress++;

                        if (item.Progress >= 100)
                        {
                            Items.Remove(item);
                        }
                    });
                }
            }
        });
    }
}
