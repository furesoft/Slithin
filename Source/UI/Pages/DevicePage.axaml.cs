using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Models;
using Slithin.UI.ContextualMenus;
using Slithin.ViewModels.Pages;

namespace Slithin.UI.Pages
{
    public partial class DevicePage : UserControl, IPage
    {
        public DevicePage()
        {
            InitializeComponent();
        }

        public string Title => "Device";

        public Control GetContextualMenu()
        {
            return new DevicePageContextualMenu();
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

        private void Drop(object sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.FileNames))
            {
                var filename = e.Data.GetFileNames().First();
                var provider = ServiceLocator.Container.Resolve<IImportProviderFactory>().GetImportProvider(".png", filename);

                if (provider != null)
                {
                    if (e.Source is Image img)
                    {
                        var bitmap = System.Drawing.Image.FromFile(filename);

                        if (bitmap.Width != 1404 && bitmap.Height != 1872)
                        {
                            DialogService.OpenError("The Screen Image does not fit is not in correct dimenson. Please use a 1404x1872 dimension.");

                            return;
                        }

                        var dc = img.Parent.DataContext;

                        if (dc is CustomScreen cs)
                        {
                            var localRepository = ServiceLocator.Container.Resolve<LocalRepository>();

                            var screenStrm = provider.Import(File.OpenRead(filename));

                            localRepository.AddScreen(screenStrm, cs.Filename);

                            var item = new SyncItem
                            {
                                Action = SyncAction.Add,
                                Data = cs,
                                Direction = SyncDirection.ToDevice,
                                Type = SyncType.Screen
                            };

                            ServiceLocator.SyncService.AddToSyncQueue(item);

                            cs.Load();
                        }
                    }
                }
                else
                {
                    DialogService.OpenError($"The file '{filename}' has the wrong Filetype");
                }
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = ServiceLocator.Container.Resolve<DevicePageViewModel>();

            AddHandler(DragDrop.DropEvent, Drop);
            AddHandler(DragDrop.DragOverEvent, DragOver);
        }
    }
}
