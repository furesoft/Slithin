using System.Windows.Input;
using Serilog;
using Slithin.Commands;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Core.Sync;

namespace Slithin.ViewModels.Pages;

public class NotebooksPageViewModel : BaseViewModel
{
    private readonly ILoadingService _loadingService;
    private readonly ILocalisationService _localisationService;
    private readonly ILogger _logger;
    private readonly IMailboxService _mailboxService;
    private readonly SynchronisationService _synchronisationService;
    private bool _isMoving;
    private Metadata _movingNotebook;
    private Metadata _selectedNotebook;

    public NotebooksPageViewModel(
        ILoadingService loadingService,
        IMailboxService mailboxService,
        ILocalisationService localisationService,
        ILogger logger)
    {
        _synchronisationService = ServiceLocator.SyncService;

        ExportCommand = ServiceLocator.Container.Resolve<ExportCommand>();
        MakeFolderCommand = ServiceLocator.Container.Resolve<MakeFolderCommand>();

        RenameCommand = ServiceLocator.Container.Resolve<RenameCommand>();
        RemoveNotebookCommand = ServiceLocator.Container.Resolve<RemoveNotebookCommand>();

        //May replace moving with drag and drop
        MoveCommand = new DelegateCommand(_ =>
            {
                IsMoving = true;
                _movingNotebook = SelectedNotebook;
            },
            _ => _ != null
                && _ is Metadata md
                && md.VisibleName != localisationService.GetString("Quick sheets")
                && md.VisibleName != localisationService.GetString("Up ..")
                && md.VisibleName != localisationService.GetString("Trash"));

        MoveCancelCommand = new DelegateCommand(_ =>
        {
            IsMoving = false;
        });

        MoveHereCommand = new DelegateCommand(_ =>
        {
            MetadataStorage.Local.Move(_movingNotebook, _synchronisationService.NotebooksFilter.Folder);
            IsMoving = false;

            MetadataStorage.Local.GetMetadata(_movingNotebook.ID).Upload();

            _synchronisationService.NotebooksFilter.Documents.Clear();
            foreach (var md in MetadataStorage.Local.GetByParent(_synchronisationService.NotebooksFilter.Folder))
            {
                _synchronisationService.NotebooksFilter.Documents.Add(md);
            }

            _synchronisationService.NotebooksFilter.Documents.Add(new Metadata
            {
                Type = "CollectionType",
                VisibleName = localisationService.GetString("Up ..")
            });

            _synchronisationService.NotebooksFilter.SortByFolder();

            logger.Information($"Moved {_movingNotebook.VisibleName} to {_synchronisationService.NotebooksFilter.Folder}");
        });

        _loadingService = loadingService;
        _mailboxService = mailboxService;
        _localisationService = localisationService;
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
    }
}
