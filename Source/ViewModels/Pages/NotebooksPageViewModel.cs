using System;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Commands;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.UI.Modals;
using Slithin.ViewModels.Modals;
using Slithin.Core.Remarkable.Rendering;
using System.Collections.Generic;

namespace Slithin.ViewModels.Pages
{
    public class NotebooksPageViewModel : BaseViewModel
    {
        private readonly ILoadingService _loadingService;
        private readonly IMailboxService _mailboxService;
        private bool _isMoving;
        private Metadata _movingNotebook;
        private Metadata _selectedNotebook;

        public NotebooksPageViewModel(ILoadingService loadingService, IMailboxService mailboxService)
        {
            MakeFolderCommand = DialogService.CreateOpenCommand<MakeFolderModal>(
                ServiceLocator.Container.Resolve<MakeFolderModalViewModel>());

            RemoveNotebookCommand = ServiceLocator.Container.Resolve<RemoveNotebookCommand>();
            MoveCommand = new DelegateCommand(_ =>
            {
                IsMoving = true;
                _movingNotebook = SelectedNotebook;
            }, (_) => SelectedNotebook != null && !IsMoving);

            MoveCancelCommand = new DelegateCommand(_ =>
             {
                 IsMoving = false;
             });

            MoveHereCommand = new DelegateCommand(_ =>
            {
                MetadataStorage.Local.Move(_movingNotebook, SyncService.NotebooksFilter.Folder);
                IsMoving = false;

                var item = new SyncItem
                {
                    Direction = SyncDirection.ToDevice,
                    Data = MetadataStorage.Local.Get(_movingNotebook.ID),
                    Type = SyncType.Notebook,
                    Action = SyncAction.Update
                };

                SyncService.SyncQueue.Insert(item);

                SyncService.NotebooksFilter.Documents.Clear();
                foreach (var md in MetadataStorage.Local.GetByParent(SyncService.NotebooksFilter.Folder))
                {
                    SyncService.NotebooksFilter.Documents.Add(md);
                }

                SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = "CollectionType", VisibleName = "Up .." });

                SyncService.NotebooksFilter.SortByFolder();
            });

            _loadingService = loadingService;
            _mailboxService = mailboxService;
        }

        public bool IsMoving
        {
            get { return _isMoving; }
            set { SetValue(ref _isMoving, value); }
        }

        public ICommand MakeFolderCommand { get; set; }

        public ICommand MoveCancelCommand { get; set; }

        public ICommand MoveCommand { get; set; }

        public ICommand MoveHereCommand { get; set; }

        public ICommand RemoveNotebookCommand { get; set; }

        public Metadata SelectedNotebook
        {
            get { return _selectedNotebook; }
            set { SetValue(ref _selectedNotebook, value); }
        }

        public override void OnLoad()
        {
            base.OnLoad();

            _mailboxService.PostAction(() =>
            {
                _loadingService.LoadNotebooks();

                var notebook = Notebook.Load("0d1541a9-2d85-4f0f-a0b5-010013ee1eff");
                var rr = SvgRenderer.RenderPage(notebook.Pages[0], 0, notebook.Metadata);

                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test-export";

                var pag = new Page();
                var layer = new Layer();

                var pdata = "m53.6,435.4l7.4,-362.4l503,0l-362,135l462,-9c0.6,0.4 -396.4,358.4 -396.4,358.4c0,0 -214,-122 -214,-122z";
                Svg.SvgPathBuilder b = new Svg.SvgPathBuilder();
                var r = (Svg.Pathing.SvgPathSegmentList)b.ConvertFromString(pdata);
                var line = new Line();
                var points = new List<Point>();

                foreach (var p in r)
                {
                    points.Add(new Point() { X = p.Start.X, Y = p.Start.Y, Pressure = 1, Direction = 2, Width = 2 });
                    points.Add(new Point() { X = p.End.X, Y = p.End.Y, Pressure = 1, Direction = 2, Width = 2 });
                }

                line.Points = points;

                layer.Lines.Add(line);

                pag.Layers = new List<Layer>(new[] { layer });

                notebook.Pages.Add(pag);

                notebook.Save();
            });
        }
    }
}
