using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.Core.Menu;
using Slithin.Core.Services;
using Slithin.Core.Sync.Repositorys;
using Slithin.Models;
using Slithin.UI.ContextualMenus;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.Pages;

[PreserveIndex(0)]
[PageIcon("Typicons.DeviceTablet")]
public partial class DevicePage : UserControl, IPage
{
    public DevicePage()
    {
        InitializeComponent();
    }

    public string Title => "Device";

    public Control GetContextualMenu() => new DeviceContextualMenu();

    bool IPage.IsEnabled() => true;

    public bool UseContextualMenu() => true;

    private void DragOver(object sender, DragEventArgs e)
    {
        // Only allow Copy or Link as Drop Operations.
        e.DragEffects = e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);

        // Only allow if the dragged data contains text or filenames.
        if (!e.Data.Contains(DataFormats.Text)
            && !e.Data.Contains(DataFormats.FileNames))
            e.DragEffects = DragDropEffects.None;
    }

    private void Drop(object sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.FileNames))
        {
            var filename = e.Data.GetFileNames().First();
            var provider = ServiceLocator.Container.Resolve<IImportProviderFactory>().GetImportProvider(".png", filename);
            var localisation = ServiceLocator.Container.Resolve<ILocalisationService>();

            if (provider != null)
            {
                if (e.Source is Image img)
                {
                    var bitmap = System.Drawing.Image.FromFile(filename);

                    if (bitmap.Width != 1404 && bitmap.Height != 1872)
                    {
                        DialogService.OpenError(localisation.GetString("The Image does not fit is not in correct dimenson. Please use a 1404x1872 dimension."));

                        return;
                    }

                    bitmap.Dispose();

                    var dc = img.Parent.DataContext;

                    if (dc is CustomScreen cs)
                    {
                        var localRepository = ServiceLocator.Container.Resolve<LocalRepository>();

                        using var source = File.OpenRead(filename);
                        using var screenStrm = provider.Import(source);

                        localRepository.AddScreen(screenStrm, cs.Filename);

                        cs.TransferCommand.Execute(null);

                        cs.Load();
                    }
                }
            }
            else
            {
                DialogService.OpenError(localisation.GetStringFormat("The file '{0}' has the wrong Filetype", filename));
            }
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        if (!Design.IsDesignMode)
        {
            DataContext = ServiceLocator.Container.Resolve<DevicePageViewModel>();
        }

        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }
}
