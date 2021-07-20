using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Sync
{
    public class NotebooksFilter : INotifyPropertyChanged
    {
        private Metadata _folder;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Metadata> Documents { get; set; } = new();

        public Metadata Folder
        {
            get { return _folder; }
            set { SetValue(ref _folder, value); }
        }

        public void SortByFolder()
        {
            var ordered = Documents.OrderBy(_ => _.Type != MetadataType.CollectionType);

            Documents = new ObservableCollection<Metadata>(ordered);
        }

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string? property = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
