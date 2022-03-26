using System;
using System.IO;
using Slithin.Core.ItemContext;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Services;

namespace Slithin.Core.Commands.ContextCommands.Notebooks;

[Context(UIContext.Notebook)]
public class ConvertQuicksheetToNotebookContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;

    public ConvertQuicksheetToNotebookContextCommand(
        ILocalisationService localisationService,
        IPathManager pathManager)
    {
        _localisationService = localisationService;
        _pathManager = pathManager;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Convert To Notebook");

    public bool CanHandle(object data)
    {
        if (data is not Metadata md)
        {
            return false;
        }

        if (md.VisibleName != _localisationService.GetString("Quick sheets"))
        {
            return false;
        }

        return true;
    }

    public void Invoke(object data)
    {
        ConvertToNotebook(data as Metadata);
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();

        // If the destination directory doesn't exist, create it.
        Directory.CreateDirectory(destDirName);

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string tempPath = Path.Combine(destDirName, file.Name);
            file.CopyTo(tempPath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
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
        md.Save();

        MetadataStorage.Local.AddMetadata(md, out var alreadyAdded);
        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);

        Notebook.UploadNotebook(md);
    }
}