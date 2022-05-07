using System;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace Slithin.ViewModels.Pages;

public class ResourcesPageViewModel : BaseViewModel
{
    private readonly IMailboxService _mailboxService;
    private readonly ISettingsService _settingsService;

    public ResourcesPageViewModel(ISettingsService settingsService,
                                  IMailboxService mailboxService)
    {
        ViewMoreCommand = new DelegateCommand(_ =>
        {
            NotificationService.Show(_.ToString());
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
                var _marketplaceAPI = ServiceLocator.Container.Resolve<MarketplaceAPI>();
                var templates = _marketplaceAPI.Get<Template[]>("templates", 5).Select(_ => new Sharable() { Name = _.Filename });

                Templates = new(templates);

                ServiceLocator.Container.Register(this);

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    frame.Navigate(typeof(ResourcesMainPage));
                });
            });
        }
    }

    private IImage LoadImage(string name)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{name}.png")));
    }
}
