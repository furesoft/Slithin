using System.Collections.ObjectModel;
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

        await Task.Run(() =>
        {
            foreach (var item in Items)
            {
                item.Progress++;
            }
        });
    }
}
