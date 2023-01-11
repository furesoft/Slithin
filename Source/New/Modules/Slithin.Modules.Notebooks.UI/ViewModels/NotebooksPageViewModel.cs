using System.Windows.Input;
using AuroraModularis.Core;
using AuroraModularis.Logging.Models;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Notebooks.UI.Commands;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI.ViewModels;

internal class NotebooksPageViewModel : BaseViewModel
{
    private readonly ILoadingService _loadingService;
    private bool _isInTrash;
    private bool _isMoving;
    private FileSystemModel _movingNotebook;

    public NotebooksPageViewModel(ILocalisationService localisationService,
                                  ILogger logger,
                                  IMetadataRepository metadataRepository,
                                  NotebooksFilter notebooksFilter,
                                  ILoadingService loadingService)
    {
        ExportCommand = Container.Current.Resolve<ExportCommand>();
        MakeFolderCommand = Container.Current.Resolve<MakeFolderCommand>();

        RenameCommand = Container.Current.Resolve<RenameCommand>();
        RemoveNotebookCommand = Container.Current.Resolve<RemoveNotebookCommand>();
        RestoreCommand = new DelegateCommand(obj =>
        {
            var md = (Metadata)((FileSystemModel)obj).Tag;
            md.Parent = "";

            metadataRepository.SaveToDisk(md);
            metadataRepository.Upload(md, true);
        }, _ => _ is not null && _ is not UpDirectoryModel);

        EmptyTrashCommand = Container.Current.Resolve<EmptyTrashCommand>();
        PinCommand = Container.Current.Resolve<PinCommand>();
        UnPinCommand = Container.Current.Resolve<UnPinCommand>();

        MoveCommand = new DelegateCommand(_ =>
            {
                IsMoving = true;
                _movingNotebook = NotebooksFilter.SelectedNotebook;
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
        NotebooksFilter = notebooksFilter;
        _loadingService = loadingService;

        MoveHereCommand = new DelegateCommand(_ =>
        {
            metadataRepository.Move((Metadata)_movingNotebook.Tag, NotebooksFilter.Folder);
            IsMoving = false;

            NotebooksFilter.Documents.Clear();
            foreach (var mds in metadataRepository.GetByParent(NotebooksFilter.Folder))
            {
                if (mds.Type == "CollectionType")
                {
                    notebooksFilter.Documents.Add(new DirectoryModel(mds.VisibleName, mds, mds.IsPinned) { ID = mds.ID, Parent = mds.Parent });
                }
                else
                {
                    notebooksFilter.Documents.Add(new FileModel(mds.VisibleName, mds, mds.IsPinned) { ID = mds.ID, Parent = mds.Parent });
                }
            }

            NotebooksFilter.Documents.Add(new UpDirectoryModel());

            NotebooksFilter.SortByFolder();

            logger.Info($"Moved {_movingNotebook.VisibleName} to {NotebooksFilter.Folder}");
        });
    }

    public ICommand EmptyTrashCommand { get; set; }
    public ICommand ExportCommand { get; set; }

    public bool IsInTrash
    {
        get { return _isInTrash; }
        set { SetValue(ref _isInTrash, value); }
    }

    public bool IsMoving
    {
        get => _isMoving;
        set => SetValue(ref _isMoving, value);
    }

    public ICommand MakeFolderCommand { get; set; }
    public ICommand MoveCancelCommand { get; set; }
    public ICommand MoveCommand { get; set; }
    public ICommand MoveHereCommand { get; set; }
    public ICommand PinCommand { get; set; }
    public ICommand RemoveNotebookCommand { get; set; }
    public ICommand RenameCommand { get; set; }
    public ICommand RestoreCommand { get; set; }

    public ICommand UnPinCommand { get; set; }
    public NotebooksFilter NotebooksFilter { get; }

    public override void OnLoad()
    {
        base.OnLoad();

        _loadingService.LoadNotebooks();
    }
}
