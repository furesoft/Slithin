using Slithin.Entities.Remarkable;
using Slithin.Entities.Remarkable.Rendering;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI.Commands.ContextCommands;

[Context(UIContext.Notebook)]
internal class ConvertQuicksheetToNotebookContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly NotebooksFilter _notebooksFilter;
    private readonly IMetadataRepository _metadataRepository;
    private readonly IPathManager _pathManager;

    public ConvertQuicksheetToNotebookContextCommand(ILocalisationService localisationService,
                                                     NotebooksFilter notebooksFilter,
                                                     IMetadataRepository metadataRepository,
                                                     IPathManager pathManager)
    {
        _localisationService = localisationService;
        _notebooksFilter = notebooksFilter;
        _metadataRepository = metadataRepository;
        _pathManager = pathManager;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Convert To Notebook");

    public bool CanExecute(object data)
    {
        if (data is not FileSystemModel fsm)
        {
            return false;
        }

        if (fsm.Tag is Metadata md)
        {
            if (md.VisibleName != _localisationService.GetString("Quick sheets"))
            {
                return false;
            }
        }

        return true;
    }

    public void Execute(object data)
    {
        var fsm = (FileSystemModel)data;
        
        ConvertToNotebook(fsm.Tag as Metadata);
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        var dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        var dirs = dir.GetDirectories();

        // If the destination directory doesn't exist, create it.
        Directory.CreateDirectory(destDirName);

        // Get the files in the directory and copy them to the new location.
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            var tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (var subdir in dirs)
            {
                var tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
            }
        }
    }

    private void ConvertToNotebook(Metadata md)
    {
        var newID = Guid.NewGuid().ToString().ToLower();

        DirectoryCopy(Path.Combine(_pathManager.NotebooksDir, md.ID), Path.Combine(_pathManager.NotebooksDir, newID), false);
        DirectoryCopy(Path.Combine(_pathManager.NotebooksDir, md.ID + ".thumbnails"), Path.Combine(_pathManager.NotebooksDir, newID + ".thumbnails"), false);

        md.ID = newID;
        md.VisibleName += " " + _localisationService.GetString("Notebook");
        _metadataRepository.SaveToDisk(md);

        _metadataRepository.AddMetadata(md, out var alreadyAdded);
        _notebooksFilter.Documents.Add(new FileModel(md.VisibleName, md, md.IsPinned));

        Notebook.UploadNotebook(md);
    }
}
