using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Sync
{
    public class NotebooksFilter : INotifyPropertyChanged
    {
        private ObservableCollection<Metadata> _documents = new();
        private Metadata _folder;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Metadata> Documents
        {
            get { return _documents; }
            set { SetValue(ref _documents, value); }
        }

        public Metadata Folder
        {
            get { return _folder; }
            set { SetValue(ref _folder, value); }
        }

        public void SortByFolder()
        {
            var ordered = Documents.OrderByDescending(_ => _.Type == MetadataType.CollectionType);
            ordered = ordered.OrderByDescending(_ => _.VisibleName.Equals("Up .."));

            Documents = new ObservableCollection<Metadata>(ordered);
        }

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string? property = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
