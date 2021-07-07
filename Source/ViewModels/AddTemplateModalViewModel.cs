using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Sync;

namespace Slithin.ViewModels
{
    public class AddTemplateModalViewModel : BaseViewModel
    {
        private string _filename;
        private IconCodeItem _iconCode;
        private bool _isLandscape;
        private string _name;
        private string _selectedCategory;

        public AddTemplateModalViewModel()
        {
            Categories = SyncService.Categories;
            Categories.RemoveAt(0);

            SelectedCategory = "Grids";

            foreach (var res in typeof(IconCodeItem).Assembly.GetManifestResourceNames())
            {
                if (res.StartsWith("Slithin.Resources.IconTiles."))
                {
                    var item = new IconCodeItem { Name = res.Split('.')[^2] };
                    item.Load();

                    IconCodes.Add(item);
                }
            }

            AddTemplateCommand = new DelegateCommand(AddTemplate);
        }

        public ICommand AddTemplateCommand { get; set; }

        public ObservableCollection<string> Categories { get; set; }

        public string Filename
        {
            get { return _filename; }
            set { SetValue(ref _filename, value); }
        }

        public IconCodeItem IconCode
        {
            get { return _iconCode; }
            set { SetValue(ref _iconCode, value); }
        }

        public ObservableCollection<IconCodeItem> IconCodes { get; set; } = new();

        public bool IsLandscape
        {
            get { return _isLandscape; }
            set { SetValue(ref _isLandscape, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set { SetValue(ref _selectedCategory, value); }
        }

        private void AddTemplate(object obj)
        {
            var template = BuildTemplate();

            File.Copy(Filename, Path.Combine(ServiceLocator.TemplatesDir, template.Filename));
            ServiceLocator.Local.Add(template);

            template.Load();

            ServiceLocator.SyncService.Templates.Add(template);

            DialogService.Close();

            var syncItem = new SyncItem() { Data = template, Direction = SyncDirection.ToDevice, Type = SyncType.Template };
            ServiceLocator.SyncService.SyncQueue.Add(syncItem);

            var configItem = new SyncItem() { Data = File.ReadAllText(Path.Combine(ServiceLocator.TemplatesDir, "templates.json")), Direction = SyncDirection.ToDevice, Type = SyncType.TemplateConfig };
            ServiceLocator.SyncService.SyncQueue.Add(configItem); //ToDo: not emmit every time, only once if the queue has any templaeconfig item
        }

        private Template BuildTemplate()
        {
            return new Template
            {
                Categories = new[] { SelectedCategory },
                Filename = Path.GetFileName(Filename),
                Name = Name,
                IconCode = IconCode.Name,
                Landscape = IsLandscape
            };
        }
    }
}
