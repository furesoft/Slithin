using System.Collections.ObjectModel;
using NuGet.Versioning;
using Slithin.Core.MVVM;

namespace Slithin.Modules.Updater.ViewModels;

internal class UpdaterViewModel : BaseViewModel
{
    private readonly Dictionary<string, NuGetVersion> _packages;

    public UpdaterViewModel(Dictionary<string, NuGetVersion> nuGetVersions)
    {
        _packages = nuGetVersions;
    }

    public ObservableCollection<ItemViewModel> Items { get; set; } = new();

    public override async void OnLoad()
    {
        foreach (var package in _packages)
        {
            Items.Add(new() {Name = package.Key });
        }

        await Task.Run(() =>
        {
            while(Items.Count > 0)
            {
                for (var index = 0; index < Items.Count; index++)
                {
                    var item = Items[index];
                    
                    item.Progress++;

                    if (item.Progress >= 100)
                    {
                        Items.Remove(item);
                    }
                }
            }
            RequestClose();
        });
    }
}
