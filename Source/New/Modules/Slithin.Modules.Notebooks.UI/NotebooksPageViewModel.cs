using System.Windows.Input;
using AuroraModularis.Core;
using AuroraModularis.Logging.Models;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Notebooks.UI.Commands;

namespace Slithin.Modules.Notebooks.UI;

public class NotebooksPageViewModel : BaseViewModel
{
    private bool _isInTrash;
    private bool _isMoving;
    private Metadata _movingNotebook;
    private Metadata _selectedNotebook;

    public NotebooksPageViewModel(ILocalisationService localisationService,
                                  ILogger logger)
    {
        ExportCommand = Container.Current.Resolve<ExportCommand>();
        MakeFolderCommand = Container.Current.Resolve<MakeFolderCommand>();

        RenameCommand = Container.Current.Resolve<RenameCommand>();
        RemoveNotebookCommand = Container.Current.Resolve<RemoveNotebookCommand>();
        RestoreCommand = new DelegateCommand(obj =>
        {
            var md = (Metadata)obj;
            md.Parent = "";

            // md.Save();
            //  md.Upload(onlyMetadata: true);
        }, _ => _ is not null && ((Metadata)_).VisibleName != localisationService.GetString("Up .."));

        EmptyTrashCommand = Container.Current.Resolve<EmptyTrashCommand>();
        PinCommand = Container.Current.Resolve<PinCommand>();
        UnPinCommand = Container.Current.Resolve<UnPinCommand>();

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

        /*
        MoveHereCommand = new DelegateCommand(_ =>
        {
            MetadataStorage.Local.Move(_movingNotebook, NotebooksFilter.Folder);
            IsMoving = false;

            MetadataStorage.Local.GetMetadata(_movingNotebook.ID).Upload();

            NotebooksFilter.Documents.Clear();
            foreach (var md in MetadataStorage.Local.GetByParent(NotebooksFilter.Folder))
            {
                NotebooksFilter.Documents.Add(md);
            }

            NotebooksFilter.Documents.Add(new Metadata
            {
                Type = "CollectionType",
                VisibleName = localisationService.GetString("Up ..")
            });

            NotebooksFilter.SortByFolder();

            logger.Info($"Moved {_movingNotebook.VisibleName} to {NotebooksFilter.Folder}");
        });
            */
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

    public Metadata SelectedNotebook
    {
        get => _selectedNotebook;
        set => SetValue(ref _selectedNotebook, value);
    }

    public ICommand UnPinCommand { get; set; }
}
