using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Primitives;
using Avalonia.Dialogs;
using Slithin.Core;

namespace Slithin.Controls
{
    public class FileChooser : TemplatedControl
    {
        public static StyledProperty<string> FilenameProperty = AvaloniaProperty.Register<FileChooser, string>("Filename");
        public static StyledProperty<string> ShortFilenameProperty = AvaloniaProperty.Register<FileChooser, string>("ShortFilename");

        public static StyledProperty<string> FilterProperty = AvaloniaProperty.Register<FileChooser, string>("Filter");

        public static StyledProperty<ICommand> BrowseCommandProperty = AvaloniaProperty.Register<FileChooser, ICommand>("BrowseCommand");

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

        public ICommand BrowseCommand
        {
            get { return GetValue(BrowseCommandProperty); }
            set { SetValue(BrowseCommandProperty, value); }
        }


        public FileChooser()
        {
            BrowseCommand = new DelegateCommand(ShowOpenFileDialog);
        }

        private void ShowOpenFileDialog(object? obj)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Programm laden";

            var window = App.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime;
            var filenames = ofd.ShowAsync(window.MainWindow).Result;
        }
    }
}