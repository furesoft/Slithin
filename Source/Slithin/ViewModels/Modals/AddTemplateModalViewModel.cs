using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia.Collections;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Sync.Repositorys;
using Slithin.Models;
using Slithin.Validators;

namespace Slithin.ViewModels.Modals;

public class AddTemplateModalViewModel : ModalBaseViewModel
{
    private readonly ILocalisationService _localisationService;
    private readonly LocalRepository _localRepository;
    private readonly IPathManager _pathManager;
    private readonly SynchronisationService _synchronisationService;
    private readonly AddTemplateValidator _validator;

    private string _filename;
    private IconCodeItem _iconCode;
    private bool _isLandscape;
    private string _name;
    private object _selectedCategory;
    private int _step;
    private bool _useTemplateEditor;

    public AddTemplateModalViewModel(IPathManager pathManager,
        LocalRepository localRepository,
        ILocalisationService localisationService,
        AddTemplateValidator validator)
    {
        Categories = SyncService.TemplateFilter.Categories;

        AddTemplateCommand = new DelegateCommand(AddTemplate);
        AddCategoryCommand = new DelegateCommand(AddCategory);
        _pathManager = pathManager;
        _localRepository = localRepository;
        _localisationService = localisationService;
        _synchronisationService = ServiceLocator.SyncService;
        _validator = validator;
    }

    public ICommand AddCategoryCommand { get; set; }
    public ICommand AddTemplateCommand { get; set; }
    public ObservableCollection<string> Categories { get; set; }

    public string Filename
    {
        get => _filename;
        set => SetValue(ref _filename, value);
    }

    public IconCodeItem IconCode
    {
        get => _iconCode;
        set => SetValue(ref _iconCode, value);
    }

    public ObservableCollection<IconCodeItem> IconCodes { get; set; } = new();

    public bool IsLandscape
    {
        get => _isLandscape;
        set => SetValue(ref _isLandscape, value);
    }

    public string Name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }

    public object SelectedCategory
    {
        get => _selectedCategory;
        set => SetValue(ref _selectedCategory, value);
    }

    public int Step
    {
        get => _step;
        set => SetValue(ref _step, value);
    }

    public bool UseTemplateEditor
    {
        get => _useTemplateEditor;
        set => SetValue(ref _useTemplateEditor, value);
    }

    public override void OnLoad()
    {
        base.OnLoad();

        foreach (var res in typeof(IconCodeItem).Assembly.GetManifestResourceNames())
        {
            if (!res.StartsWith("Slithin.Resources.IconTiles."))
            {
                continue;
            }

            var item = new IconCodeItem { Name = res.Split('.')[^2] };
            item.Load();

            IconCodes.Add(item);
        }
    }

    private void AddCategory(object obj)
    {
        if (!string.IsNullOrEmpty(obj?.ToString()))
        {
            SyncService.TemplateFilter.Categories.Add(obj.ToString());
        }
        else
        {
            DialogService.OpenError(_localisationService.GetString("Category name has to be set."));
        }
    }

    private async void AddTemplate(object obj)
    {
        var validationResult = _validator.Validate(this);

        if (!validationResult.IsValid)
        {
            DialogService.OpenError(validationResult.Errors.First().ToString());
            return;
        }

        if (UseTemplateEditor)
        {
            Step++;
            return;
        }

        if (UseTemplateEditor && Step == 1 || !UseTemplateEditor)
        {
            var template = BuildTemplate();

            if (File.Exists(Path.Combine(_pathManager.TemplatesDir, template.Filename + ".png")))
            {
                if (await DialogService.ShowDialog(_localisationService.GetString("Template already exist. Would you replace it?")))
                {
                    File.Delete(Path.Combine(_pathManager.TemplatesDir, template.Filename + ".png"));
                }
                else
                {
                    return;
                }
            }

            var bitmap = System.Drawing.Image.FromFile(Filename);

            if (bitmap.Width != 1404 && bitmap.Height != 1872)
            {
                DialogService.OpenError(
                    _localisationService.GetString("The Template does not fit is not in correct dimenson. Please use a 1404x1872 dimension."));

                return;
            }

            File.Copy(Filename, Path.Combine(_pathManager.TemplatesDir, template.Filename + ".png"));

            _localRepository.AddTemplate(template);

            template.Load();

            TemplateStorage.Instance.AppendTemplate(template);
            _synchronisationService.TemplateFilter.Templates.Add(template);

            template.TransferCommand.Execute(null);

            DialogService.Close();
        }
    }

    private Template BuildTemplate()
    {
        var iconCodeValue = int.Parse(IconCode.Name, System.Globalization.NumberStyles.HexNumber);
        var iconCode = char.ConvertFromUtf32(iconCodeValue).ToString();

        if (SelectedCategory == null)
        {
            SelectedCategory = new AvaloniaList<object> { "Grids" };
        }

        return new Template
        {
            Categories = ((AvaloniaList<object>)SelectedCategory).Select(_ => _.ToString()).ToArray(),
            Filename = Path.GetFileNameWithoutExtension(Filename),
            Name = Name,
            IconCode = iconCode,
            Landscape = IsLandscape
        };
    }
}
