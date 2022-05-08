using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using Slithin.API.Lib;
using Slithin.Controls.Navigation;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Core.Services;
using Slithin.Marketplace.Models;
using Slithin.Models;
using Slithin.UI.Modals;
using Slithin.UI.ResourcesPage;
using SlithinMarketplace.Models;

namespace Slithin.ViewModels.Pages;

public class ResourcesPageViewModel : BaseViewModel
{
    private readonly IMailboxService _mailboxService;
    private readonly ISettingsService _settingsService;

    public ResourcesPageViewModel(ISettingsService settingsService,
                                  IMailboxService mailboxService)
    {
        ViewMoreCommand = new DelegateCommand(asset =>
        {
            _mailboxService.PostAction(async () =>
            {
                var marketplaceAPI = ServiceLocator.Container.Resolve<MarketplaceAPI>();
                var items = marketplaceAPI.Get<AssetModel[]>(asset.ToString().ToLower())
                        .Select(_ => new Sharable() { Asset = _ });

                var vm = new ResourceListViewModel();
                vm.Items = new(items);

                ServiceLocator.Container.Register(vm);

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

        _settingsService = settingsService;
        _mailboxService = mailboxService;
    }

    public ObservableCollection<Sharable> Templates { get; set; } = new();

    public ICommand ViewMoreCommand { get; set; }

    public override void OnLoad()
    {
        base.OnLoad();

        var frame = Frame.GetFrame("resourcesFrame");

        var settings = _settingsService.GetSettings();

        if (settings.MarketplaceCredential == null)
        {
            frame.Navigate(typeof(LoginModal));
        }
        else
        {
            _mailboxService.PostAction(async () =>
            {
                var marketplaceAPI = ServiceLocator.Container.Resolve<MarketplaceAPI>();
                var templates = marketplaceAPI.Get<Template[]>("templates", 5)
                        .Select(_ => new Sharable() { Asset = _ });

                Templates = new(templates);

                ServiceLocator.Container.Register(this);

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

    private IImage LoadImage(string name)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{name}.png")));
    }

    private IImage LoadImage(byte[] bytes)
    {
        return new Bitmap(new MemoryStream(bytes));
    }
}
