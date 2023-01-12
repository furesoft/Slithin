using System.Collections.ObjectModel;
using System.Windows.Input;
using AuroraModularis.Core;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Slithin.Controls.Navigation;
using Slithin.Core.MVVM;
using Slithin.Modules.Resources.Models;
using Slithin.Modules.Resources.UI.Pages;
using Slithin.Modules.Settings.Models;

namespace Slithin.Modules.Resources.UI.ViewModels;

public class ResourcesPageViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;

    public ResourcesPageViewModel(ISettingsService settingsService)
    {
        ViewMoreTemplatesCommand = CreateLoadCommand<Template>();

        _settingsService = settingsService;
    }

    public ObservableCollection<Sharable> Templates { get; set; } = new();

    public ICommand ViewMoreTemplatesCommand { get; set; }

    public override void OnLoad()
    {
        base.OnLoad();

        var frame = Frame.GetFrame("resourcesFrame");

        var settings = _settingsService.GetSettings();

        if (settings.MarketplaceToken == null)
        {
            frame.Navigate(typeof(LoginModal));
        }
        else
        {
            Task.Run(async () =>
            {
                var marketplaceAPI = Container.Current.Resolve<MarketplaceAPI>();
                var templates = marketplaceAPI.Get<Template[]>("templates", 5)
                        .Select(_ => new Sharable() { Asset = _ });

                Templates = new(templates);

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    frame.Navigate(typeof(ResourcesMainPage));
                });

                Parallel.For(0, Templates.Count, async (index) =>
                {
                    var template = Templates[index];
                    var bytes = marketplaceAPI.GetBytes(template.Asset.FileID);

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Templates[index].Image = LoadImage(bytes);
                    });
                });
            });
        }
    }

    private DelegateCommand CreateLoadCommand<T>()
        where T : AssetModel
    {
        return new DelegateCommand(asset =>
        {
            Task.Run(async () =>
            {
                var marketplaceAPI = Container.Current.Resolve<MarketplaceAPI>();
                var items = marketplaceAPI.Get<T[]>(asset.ToString().ToLower())
                        .Select(_ => new Sharable() { Asset = _ });

                var vm = new ResourceListViewModel();
                vm.Items = new(items);

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var frame = Frame.GetFrame("resourcesFrame");
                    frame.Navigate(typeof(ListPage));
                });

                Parallel.For(0, vm.Items.Count, async (index) =>
                {
                    var template = vm.Items[index];
                    var bytes = marketplaceAPI.GetBytes(template.Asset.FileID);

                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        vm.Items[index].Image = LoadImage(bytes);
                    });
                });
            });
        });
    }

    private IImage LoadImage(byte[] bytes)
    {
        return new Bitmap(new MemoryStream(bytes));
    }
}
