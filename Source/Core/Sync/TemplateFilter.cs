using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Slithin.Core.Remarkable;

namespace Slithin.Core
{
    public class TemplateFilter : INotifyPropertyChanged
    {
        private bool _landscape;
        private string _selectedCategory;

        public TemplateFilter()
        {
            Categories = new();
            Templates = new();

            Categories.Add("All");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Categories { get; set; }

        public bool Landscape
        {
            get { return _landscape; }
            set { SetValue(ref _landscape, value); RefreshTemplates(); }
        }

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetValue(ref _selectedCategory, value); RefreshTemplates(); }
        }

        public ObservableCollection<Template> Templates { get; set; }

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string? property = null)
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void RefreshTemplates()
        {
            Templates.Clear();

            if (SelectedCategory == "All")
            {
                foreach (var item in TemplateStorage.Instance?.Templates.Where(_ => Landscape == _.Landscape))
                {
                    Templates.Add(item);
                }
            }
            else
            {
                foreach (var item in TemplateStorage.Instance?.Templates.Where(_ => _.Categories.Contains(SelectedCategory) && Landscape == _.Landscape))
                {
                    Templates.Add(item);
                }
            }
        }
    }
}
