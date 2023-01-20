using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Menu.Models.Menu;
using Slithin.Modules.Notebooks.UI.ViewModels;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Notebooks.UI.UI;

[PreserveIndex(1)]
[PageIcon("Codeicons.Notebook")]
[Context(UIContext.Notebook)]
public class NotebooksPage : UserControl, IPage
{
    public NotebooksPage()
    {
        InitializeComponent();

        if (!Design.IsDesignMode)
        {
            DataContext = Container.Current.Resolve<NotebooksPageViewModel>();
        }
    }

    public string Title => "Notebooks";

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
        var pathManager = Container.Current.Resolve<IPathManager>();
        var localisation = Container.Current.Resolve<ILocalisationService>();

        var notebooksDir = pathManager.NotebooksDir;

        if (e.Data.Contains(DataFormats.FileNames))
        {
            foreach (var filename in e.Data.GetFileNames())
            {
                var id = Guid.NewGuid().ToString().ToLower();
                /*
                var importProviderFactory = Container.Current.Resolve<IImportProviderFactory>();
                var provider = importProviderFactory.GetImportProvider(".pdf", filename);

                var cnt = new ContentFile { FileType = provider == null ? "epub" : "pdf" };

                if (cnt.FileType == "pdf" || cnt.FileType == "epub")
                {
                    var md = new Metadata
                    {
                        ID = id,
                        Parent = NotebooksFilter.Folder,
                        Content = cnt,
                        Version = 1,
                        Type = "DocumentType",
                        VisibleName = Path.GetFileNameWithoutExtension(filename)
                    };
                    MetadataStorage.Local.AddMetadata(md, out _);
                    NotebooksFilter.Documents.Add(md);

                    provider = importProviderFactory.GetImportProvider($".{cnt.FileType}", filename);

                    if (provider != null)
                    {
                        var inputStrm = provider.Import(File.OpenRead(filename));
                        var outputStrm =
                            File.OpenWrite(Path.Combine(notebooksDir, md.ID + "." + Path.GetExtension(filename)));
                        inputStrm.CopyTo(outputStrm);

                        outputStrm.Close();
                        inputStrm.Close();

                        md.Save();

                        md.Upload();

                        var scp = ServiceLocator.Container.Resolve<ISSHService>();

                        scp.Uploading += (s, e) =>
                        {
                            NotificationService.ShowProgress(
                                localisation.GetStringFormat(
                                    "Uploading '{0}': {1}", md.VisibleName, e.Filename)
                                , (int)e.Uploaded, (int)e.Size);
                        };

                        scp.Upload(new FileInfo(Path.Combine(notebooksDir, md.ID + "." + Path.GetExtension(filename))),
                            PathList.Documents + md.ID + "." + Path.GetExtension(filename));
                    }
                    else
                    {
                        DialogService.OpenError(localisation.GetStringFormat("The filetype '{0}' is not supported", Path.GetExtension(filename)));
                    }
                }
                else
                {
                    DialogService.OpenError(localisation.GetStringFormat("The filetype '{0}' is not supported", Path.GetExtension(filename)));
                }
                */
            }
        }
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        AddHandler(DragDrop.DropEvent, Drop);
        AddHandler(DragDrop.DragOverEvent, DragOver);
    }
}
