using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Newtonsoft.Json;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;
using Slithin.UI.ContextualMenus;
using Slithin.ViewModels;

namespace Slithin.UI.Pages
{
    public partial class NotebooksPage : UserControl, IPage
    {
        public NotebooksPage()
        {
            InitializeComponent();

            DataContext = new NotebooksPageViewModel();
        }

        public string Title => "Notebooks";

        public Control GetContextualMenu()
        {
            return new NotebooksContextualMenu();
        }

        bool IPage.IsEnabled()
        {
            return true;
        }

        public bool UseContextualMenu()
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
                e.DragEffects = DragDropEffects.None;
        }

        private async void Drop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                foreach (var filename in e.Data.GetFileNames())
                {
                    var id = Guid.NewGuid();
                    var cnt = new ContentFile();
                    cnt.FileType = Path.GetExtension(filename).Replace(".", "");

                    if (cnt.FileType == "pdf" || cnt.FileType == "epub")
                    {
                        var md = new Metadata();
                        md.ID = id.ToString();
                        md.Parent = ServiceLocator.SyncService.NotebooksFilter.Folder;
                        md.Content = cnt;
                        md.Version = 1;
                        md.Type = "DocumentType";
                        md.VisibleName = Path.GetFileNameWithoutExtension(filename);

                        MetadataStorage.Add(md, out var alreadyAdded);
                        ServiceLocator.SyncService.NotebooksFilter.Documents.Add(md);

                        File.Copy(filename, Path.Combine(ServiceLocator.NotebooksDir, md.ID + Path.GetExtension(filename)));

                        File.WriteAllText(Path.Combine(ServiceLocator.NotebooksDir, md.ID + ".metadata"), JsonConvert.SerializeObject(md));
                        File.WriteAllText(Path.Combine(ServiceLocator.NotebooksDir, md.ID + ".content"), JsonConvert.SerializeObject(cnt));

                        var syncItem = new SyncItem();
                        syncItem.Action = SyncAction.Add;
                        syncItem.Direction = SyncDirection.ToDevice;
                        syncItem.Type = SyncType.Notebook;
                        syncItem.Data = md;

                        ServiceLocator.SyncService.SyncQueue.Insert(syncItem);
                    }
                    else
                    {
                        await DialogService.ShowDialog($"The filetype '{cnt.FileType}' is not supported");
                    }
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
}
