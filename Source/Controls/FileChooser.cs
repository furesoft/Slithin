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
        public static StyledProperty<string> FilenameProperty = AvaloniaProperty.Register<FileChooser, string>("Filename");
        public static StyledProperty<string> ShortFilenameProperty = AvaloniaProperty.Register<FileChooser, string>("ShortFilename");

        public static StyledProperty<string> FilterProperty = AvaloniaProperty.Register<FileChooser, string>("Filter");

        public ICommand ShowOpenFileDialogCommand;

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


        public FileChooser()
        {
            ShowOpenFileDialogCommand = new DelegateCommand(ShowOpenFileDialog);
        }

        private async void ShowOpenFileDialog(object? obj)
        {
            var ofd = new OpenFileDialog();
            ofd.Filters.Add(new FileDialogFilter() { });

            var filenames = await ofd.ShowAsync(((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow);
            Filename = filenames[0];
            ShortFilename = Path.GetFileName(Filename);
        }
    }
}