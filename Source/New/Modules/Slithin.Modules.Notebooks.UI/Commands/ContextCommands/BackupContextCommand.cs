﻿using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Menu.Models.ItemContext;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;

namespace Slithin.Modules.Notebooks.UI.Commands.ContextCommands;

[Context(UIContext.Notebook)]
internal class BackupContextCommand : IContextCommand
{
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;

    public BackupContextCommand(ILocalisationService localisationService, IPathManager pathManager)
    {
        _localisationService = localisationService;
        _pathManager = pathManager;
    }

    public object ParentViewModel { get; set; }
    public string Titel => _localisationService.GetString("Backup");

    public bool CanExecute(object data)
    {
        return data is not TrashModel;
    }

    public void Execute(object data)
    {
        Backup((Metadata)data);
    }

    private void Backup(Metadata md)
    {
        //ToDo: make backup service and allow backup single notebook in service
        /*
            NotificationService.Show(_localisationService.GetStringFormat("Start Compressing {0}", md.VisibleName));

            var zip = new ZipFile();

            BackupNotebookOrFolder(md, zip);

            zip.Comment = "This backup was generated by Slithin";

            zip.SaveProgress += (s, e) =>
            {
                if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
                {
                    NotificationService.ShowProgress(_localisationService.GetStringFormat("Compressing '{0}'", e.CurrentEntry.FileName), e.EntriesSaved, e.EntriesTotal);
                }
            };

            zip.Save(Path.Combine(_pathManager.BackupsDir,
                $"Backup_{md.VisibleName}_{DateTime.Now:yyyy-dd-M--HH-mm-ss}_{md.ID}.notebook.zip"));

            zip.Dispose();
        */
    }

    /*
    private void BackupNotebookOrFolder(Metadata md, ZipFile zip)
    {
        if (md.Type == "CollectionType")
        {
            foreach (var sub in MetadataStorage.Local.GetByParent(md.ID))
            {
                BackupNotebookOrFolder(sub, zip);
            }
        }

        var files = Directory.GetFiles(_pathManager.NotebooksDir, $"{md.ID}*");
        foreach (var file in files)
        {
            zip.AddFile(file, "/");
        }

        var directories = Directory.GetDirectories(_pathManager.NotebooksDir, $"{md.ID}*");
        foreach (var dir in directories)
        {
            zip.AddDirectory(dir, new DirectoryInfo(dir).Name);
        }
    }*/
}
