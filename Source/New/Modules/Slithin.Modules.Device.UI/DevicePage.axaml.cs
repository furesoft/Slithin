using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Slithin.Core.MVVM;
using Slithin.Modules.Device.UI.ViewModels;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Menu.Models.Menu;

namespace Slithin.Modules.Device.UI;

[PreserveIndex(0)]
[PageIcon("Typicons.DeviceTablet")]
[Context(UIContext.Device)]
public partial class DevicePage : UserControl, IPage
{
    public DevicePage()
    {
        InitializeComponent();
    }

    public string Title => "Device";

    bool IPage.IsEnabled()
    {
        return true;
    }

    private void DragOver(object sender, DragEventArgs e)
    {
        // Only allow Copy or Link as Drop Operations.
        e.DragEffects = e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);

        // Only allow if the dragged data contains text or filenames.
        if (!e.Data.Contains(DataFormats.Text)
            && !e.Data.Contains(DataFormats.FileNames))
        {
            e.DragEffects = DragDropEffects.None;
        }
    }

    private void Drop(object sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.FileNames))
        {
            var filename = e.Data.GetFileNames().First();

            /*
            var provider = ServiceLocator.Container.Resolve<IImportProviderFactory>().GetImportProvider(".png", filename);
            var localisation = ServiceLocator.Container.Resolve<ILocalisationService>();

            if (provider != null)
            {
                if (e.SettingsBuilderTestApp is Image img)
                {
                    var bitmap = new Bitmap(filename);

                    if (bitmap.Size.Width != 1404 && bitmap.Size.Height != 1872)
                    {
                        DialogService.OpenError(localisation.GetString("The Image does not fit is not in correct dimension. Please use a 1404x1872 dimension."));

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
            */
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        BaseViewModel.ApplyViewModel<DevicePageViewModel>(this);

        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }
}
