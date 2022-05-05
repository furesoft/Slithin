using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Core.Services;
using Slithin.Models;
using Slithin.UI.Modals;

namespace Slithin.ViewModels.Pages;

public class MarketplacePageViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;

    public MarketplacePageViewModel(ISettingsService settingsService)
    {
        Templates.Add(new() { ID = "1", IsInstalled = false, Name = "Not Installed Template 1", Image = LoadImage("backup"), Author = "Furesoft" });
        Templates.Add(new() { ID = "2", IsInstalled = true, Name = "Installed Template 2", Image = LoadImage("epub"), Author = "Furesoft" });
        Templates.Add(new() { ID = "3", IsInstalled = false, Name = "Not Installed Template 3", Image = LoadImage("folder"), Author = "Furesoft" });
        Templates.Add(new() { ID = "4", IsInstalled = true, Name = "Installed Template 4", Image = LoadImage("pdf"), Author = "Furesoft" });
        Templates.Add(new() { ID = "5", IsInstalled = true, Name = "Installed Template 5", Image = LoadImage("folder"), Author = "Furesoft" });
        Templates.Add(new() { ID = "6", IsInstalled = false, Name = "Not Installed Template 5", Image = LoadImage("backup"), Author = "Furesoft" });

        ViewMoreCommand = new DelegateCommand(_ =>
        {
            NotificationService.Show(_.ToString());
        });
        _settingsService = settingsService;
    }

    public ObservableCollection<Sharable> Templates { get; set; } = new();

    public ICommand ViewMoreCommand { get; set; }

    public override void OnLoad()
    {
        base.OnLoad();

        var settings = _settingsService.GetSettings();

        if (settings.MarketplaceCredential == null)
        {
            DialogService.Open(new LoginModal());
        }
    }

    private IImage LoadImage(string name)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        return new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/{name}.png")));
    }
}
