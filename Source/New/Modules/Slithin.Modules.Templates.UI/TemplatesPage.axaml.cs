using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Slithin.Modules.Menu.Models.Menu;
using Slithin.Modules.Templates.UI.ViewModels;

namespace Slithin.Modules.Templates.UI;

[PreserveIndex(2)]
[PageIcon("Vaadin.List")]
public partial class TemplatesPage : UserControl, IPage
{
    public TemplatesPage()
    {
        InitializeComponent();

        DataContext = Container.Current.Resolve<TemplatesPageViewModel>();
    }

    public string Title => "Templates";

    public Control GetContextualMenu()
    {
        return new TemplatesContextualMenu();
    }

    bool IPage.IsEnabled() => true;

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
            /*
            var localisation = ServiceLocator.Container.Resolve<ILocalisationService>();

            var filename = e.Data.GetFileNames().First();
            var provider = ServiceLocator.Container.Resolve<IImportProviderFactory>().GetImportProvider(".png", filename);

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

                    var pathManager = ServiceLocator.Container.Resolve<IPathManager>();
                    var localRepository = ServiceLocator.Container.Resolve<LocalRepository>();
                    var validator = ServiceLocator.Container.Resolve<AddTemplateValidator>();
                    var localisationService = ServiceLocator.Container.Resolve<ILocalisationService>();

                    var vm = new AddTemplateModalViewModel(pathManager, localRepository, localisationService, validator);
                    vm.Filename = filename;
                    vm.Name = Path.GetFileNameWithoutExtension(filename);

                    DialogService.Open(new AddTemplateModal(), vm);
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

        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }
}
