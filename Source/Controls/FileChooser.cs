using System.IO;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Slithin.Core;

namespace Slithin.Controls
{
    public class FileChooser : TemplatedControl
    {
        public static StyledProperty<ICommand> BrowseCommandProperty = AvaloniaProperty.Register<FileChooser, ICommand>("BrowseCommand");
        public static StyledProperty<string> FilenameProperty = AvaloniaProperty.Register<FileChooser, string>("Filename");
        public static StyledProperty<string> FilterProperty = AvaloniaProperty.Register<FileChooser, string>("Filter");
        public static StyledProperty<string> ShortFilenameProperty = AvaloniaProperty.Register<FileChooser, string>("ShortFilename");
        public static StyledProperty<string> WatermarkProperty = AvaloniaProperty.Register<FileChooser, string>("Watermark");

        public FileChooser()
        {
            BrowseCommand = new DelegateCommand(ShowOpenFileDialog);
        }

        public ICommand BrowseCommand
        {
            get { return GetValue(BrowseCommandProperty); }
            set { SetValue(BrowseCommandProperty, value); }
        }

        public string Filename
        {
            get { return GetValue(FilenameProperty); }
            set { SetValue(FilenameProperty, value); }
        }

        public string Filter
        {
            get { return GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public string ShortFilename
        {
            get { return GetValue(ShortFilenameProperty); }
            set { SetValue(ShortFilenameProperty, value); }
        }

        public string Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        private async void ShowOpenFileDialog(object obj)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Load File"
            };

            var window = App.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
            var filenames = await ofd.ShowAsync(window.MainWindow);

            if (filenames != null)
            {
                Filename = filenames[0];
                ShortFilename = Path.GetFileName(Filename);
            }
        }
    }
}
