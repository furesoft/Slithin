using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Threading;
using NuGet.Versioning;
using Slithin.Core.MVVM;
using Slithin.Modules.Updater.Models.ViewModels;

namespace Slithin.Modules.Updater.ViewModels;

internal class UpdaterViewModel : BaseViewModel
{
    private readonly Dictionary<string, NuGetVersion> _packages;

    public UpdaterViewModel(Dictionary<string, NuGetVersion> nuGetVersions)
    {
        _packages = nuGetVersions;
    }

    public ObservableCollection<ItemViewModel> Items { get; } = new();

    protected override async void OnLoad()
    {
        var workingQueue = new ObservableQueue<ItemViewModel>();
        InitCollections(workingQueue);

        await ApplyDownloadQueue(workingQueue);
        
        Process.Start(new ProcessStartInfo("dotnet", typeof(UpdateInstaller.App).Assembly.Location));
        Environment.Exit(0);
    }

    private async Task ApplyDownloadQueue(ObservableQueue<ItemViewModel> workingQueue)
    {
        await Task.Run(async () =>
        {
            while (workingQueue.Any())
            {
                var item = workingQueue.Dequeue();
                var progress = CreateProgressForSelfRemovingItem(item);

                await UpdateRepository.DownloadPackage(item.Name, item.Version, progress);

                await Task.Delay(1000);

                Items.Remove(item);
            }
        });
    }

    private Progress<bool> CreateProgressForSelfRemovingItem(ItemViewModel item)
    {
        return new(async p =>
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                item.IsDone = p;

                if (item.IsDone)
                {
                    Items.Remove(item);
                }
            });
        });
    }

    private void InitCollections(ObservableQueue<ItemViewModel> workingQueue)
    {
        foreach (var package in _packages)
        {
            Items.Add(new() {Name = package.Key, Version = package.Value});
            workingQueue.Enqueue(Items.Last());
        }
    }
}
