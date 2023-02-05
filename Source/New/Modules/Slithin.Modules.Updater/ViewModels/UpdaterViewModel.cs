using System.Collections.ObjectModel;
using Slithin.Core.MVVM;

namespace Slithin.Modules.Updater.ViewModels;

internal class UpdaterViewModel : BaseViewModel
{
    public UpdaterViewModel()
    {
        foreach (var file in Directory.GetFiles(Environment.CurrentDirectory, "*.dll"))
        {
            Items.Add(new ItemViewModel() { Name = Path.GetFileNameWithoutExtension(file), });
        }
    }

    public ObservableCollection<ItemViewModel> Items { get; set; } = new();

    public override async void OnLoad()
    {
        await UpdateRepository.GetNugetPackages();
    }
}
