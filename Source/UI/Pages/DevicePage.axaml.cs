using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Slithin.Core;
using Slithin.UI.ContextualMenus;
using Slithin.ViewModels;

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
            return false;
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
                var filename = e.Data.GetFileNames().First();

                //copy filename to CustomScreens Dir
                //Reload image

                ((CustomScreen)sender).Load();
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            DataContext = new DevicePageViewModel();

            AddHandler(DragDrop.DropEvent, Drop);
            AddHandler(DragDrop.DragOverEvent, DragOver);
        }
    }
}
