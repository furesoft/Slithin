using System.Windows.Input;
using AuroraModularis.Core;
using AuroraModularis.Logging.Models;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Notebooks.UI.Commands;
using Slithin.Modules.Notebooks.UI.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI.ViewModels;

internal class NotebooksPageViewModel : BaseViewModel, IFilterable<NotebooksFilter>
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
        MakeFolderCommand = ServiceContainer.Current.Resolve<MakeFolderCommand>();

        RenameCommand = ServiceContainer.Current.Resolve<RenameCommand>();
        RemoveNotebookCommand = ServiceContainer.Current.Resolve<RemoveNotebookCommand>();
        RestoreCommand = new DelegateCommand(obj =>
        {
            var md = (Metadata)((FileSystemModel)obj).Tag;
            md.Parent = "";

            metadataRepository.SaveToDisk(md);
            metadataRepository.Upload(md, true);
        }, _ => _ is not null && _ is not UpDirectoryModel);

        EmptyTrashCommand = ServiceContainer.Current.Resolve<EmptyTrashCommand>();
        PinCommand = ServiceContainer.Current.Resolve<PinCommand>();
        UnPinCommand = ServiceContainer.Current.Resolve<UnPinCommand>();

        MoveCommand = new DelegateCommand(_ =>
            {
                IsMoving = true;
                _movingNotebook = Filter.Selection;
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
        Filter = notebooksFilter;
        _loadingService = loadingService;

        MoveHereCommand = new DelegateCommand(_ =>
        {
            metadataRepository.Move((Metadata)_movingNotebook.Tag, Filter.Folder);
            IsMoving = false;

            Filter.Items.Clear();
            foreach (var mds in metadataRepository.GetByParent(Filter.Folder))
            {
                if (mds.Type == "CollectionType")
                {
                    notebooksFilter.Items.Add(
                        new DirectoryModel(mds.VisibleName, mds, mds.IsPinned) { ID = mds.ID, Parent = mds.Parent });
                }
                else
                {
                    notebooksFilter.Items.Add(
                        new FileModel(mds.VisibleName, mds, mds.IsPinned) { ID = mds.ID, Parent = mds.Parent });
                }
            }

            Filter.Items.Add(new UpDirectoryModel(Filter.ParentFolder));

            Filter.SortByFolder();

            logger.Info($"Moved {_movingNotebook.VisibleName} to {Filter.Folder}");
        });
    }

    public ICommand EmptyTrashCommand { get; set; }

    public bool IsInTrash
    {
        get => _isInTrash;
        set => SetValue(ref _isInTrash, value);
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

    public NotebooksFilter Filter { get; }

    public override async void OnLoad()
    {
        await _loadingService.LoadNotebooksAsync();
    }
}
