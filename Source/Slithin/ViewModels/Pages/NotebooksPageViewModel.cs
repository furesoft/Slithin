using System;
using System.Windows.Input;
using Serilog;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;

namespace Slithin.ViewModels.Pages;

public class NotebooksPageViewModel : BaseViewModel
{
    private readonly ILoadingService _loadingService;
    private readonly ILogger _logger;
    private readonly IMailboxService _mailboxService;
    private readonly SynchronisationService _synchronisationService;
    private bool _isMoving;
    private Metadata _movingNotebook;
    private Metadata _selectedNotebook;

    public NotebooksPageViewModel(ILoadingService loadingService, IMailboxService mailboxService, ILogger logger)
    {
        _synchronisationService = ServiceLocator.SyncService;
        ExportCommand = ServiceLocator.Container.Resolve<ExportCommand>();

        MakeFolderCommand = new DelegateCommand(async _ =>
        {
            var name = await DialogService.ShowPrompt("Make Folder", "Foldername");
            MakeFolder(name);
        });

        RenameCommand = new DelegateCommand(async _ =>
        {
            var name = await DialogService.ShowPrompt("Rename ", "Name", ((Metadata)_).VisibleName);
            Rename((Metadata)_, name);
        }, _ => _ != null
                && _ is Metadata md
                && md.VisibleName != "Quick sheets"
                && md.VisibleName != "Up .."
                && md.VisibleName != "Trash");

        RemoveNotebookCommand = ServiceLocator.Container.Resolve<RemoveNotebookCommand>();
        MoveCommand = new DelegateCommand(_ =>
            {
                IsMoving = true;
                _movingNotebook = SelectedNotebook;
            },
            _ => _ != null
                && _ is Metadata md
                && md.VisibleName != "Quick sheets"
                && md.VisibleName != "Up .."
                && md.VisibleName != "Trash");

        MoveCancelCommand = new DelegateCommand(_ =>
        {
            IsMoving = false;
        });

        MoveHereCommand = new DelegateCommand(_ =>
        {
            MetadataStorage.Local.Move(_movingNotebook, SyncService.NotebooksFilter.Folder);
            IsMoving = false;

            MetadataStorage.Local.GetMetadata(_movingNotebook.ID).Upload();

            SyncService.NotebooksFilter.Documents.Clear();
            foreach (var md in MetadataStorage.Local.GetByParent(SyncService.NotebooksFilter.Folder))
            {
                SyncService.NotebooksFilter.Documents.Add(md);
            }

            SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = "CollectionType", VisibleName = "Up .." });

            SyncService.NotebooksFilter.SortByFolder();

            logger.Information($"Moved {_movingNotebook.VisibleName} to {SyncService.NotebooksFilter.Folder}");
        });

        _loadingService = loadingService;
        _mailboxService = mailboxService;
        _logger = logger;
    }

    public ICommand ExportCommand { get; set; }

    public bool IsMoving
    {
        get => _isMoving;
        set => SetValue(ref _isMoving, value);
    }

    public ICommand MakeFolderCommand { get; set; }
    public ICommand MoveCancelCommand { get; set; }
    public ICommand MoveCommand { get; set; }
    public ICommand MoveHereCommand { get; set; }
    public ICommand RemoveNotebookCommand { get; set; }
    public ICommand RenameCommand { get; set; }

    public Metadata SelectedNotebook
    {
        get => _selectedNotebook;
        set => SetValue(ref _selectedNotebook, value);
    }

    public override void OnLoad()
    {
        base.OnLoad();

        _mailboxService.PostAction(() =>
        {
            NotificationService.Show("Loading Notebooks");

            _loadingService.LoadNotebooks();

            NotificationService.Hide();
        });
    }

    private void MakeFolder(string name)
    {
        var id = Guid.NewGuid().ToString().ToLower();

        var md = new Metadata
        {
            ID = id,
            Parent = _synchronisationService.NotebooksFilter.Folder,
            Type = "CollectionType",
            VisibleName = name
        };

        MetadataStorage.Local.AddMetadata(md, out var alreadyAdded);

        if (alreadyAdded)
        {
            DialogService.OpenError($"'{md.VisibleName}' already exists");
            return;
        }

        md.Save();

        _synchronisationService.NotebooksFilter.Documents.Add(md);
        _synchronisationService.NotebooksFilter.SortByFolder();

        var syncItem = new SyncItem
        {
            Action = SyncAction.Add,
            Data = md,
            Direction = SyncDirection.ToDevice,
            Type = SyncType.Notebook
        };

        _synchronisationService.AddToSyncQueue(syncItem);

        _logger.Information($"Folder '{md.VisibleName}' created");

        DialogService.Close();
    }

    private void Rename(Metadata md, string newName)
    {
        _logger.Information($"Renamed '{md.VisibleName}' to '{newName}'");

        md.VisibleName = newName;

        MetadataStorage.Local.Remove(md);
        MetadataStorage.Local.AddMetadata(md, out var alreadyAdded);

        if (alreadyAdded)
        {
            return;
        }

        md.Save();

        var syncItem = new SyncItem
        {
            Action = SyncAction.Update,
            Data = md,
            Direction = SyncDirection.ToDevice,
            Type = SyncType.Notebook
        };

        _synchronisationService.AddToSyncQueue(syncItem);

        DialogService.Close();
    }
}
