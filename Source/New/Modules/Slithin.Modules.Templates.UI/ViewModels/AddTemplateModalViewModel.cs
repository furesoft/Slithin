using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.Templates.UI.Models;
using Slithin.Modules.Templates.UI.Validators;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.Templates.UI.ViewModels;

public class AddTemplateModalViewModel : ModalBaseViewModel
{
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;
    private readonly INotificationService _notificationService;
    private readonly ITemplateStorage _templateStorage;
    private readonly AddTemplateValidator _validator;
    private readonly TemplatesFilter _templatesFilter;
    private readonly IDialogService _dialogService;
    private readonly ICacheService _cacheService;
    private string _filename;
    private IconCodeItem _iconCode;
    private bool _isLandscape;
    private string _name;
    private object _selectedCategory;

    public AddTemplateModalViewModel(IPathManager pathManager, INotificationService notificationService,
        ITemplateStorage templateStorage, AddTemplateValidator validator,
        TemplatesFilter templatesFilter, IDialogService dialogService, ICacheService cacheService,
        ILocalisationService localisationService)
    {
        Categories = templatesFilter.Categories;

        AddTemplateCommand = new DelegateCommand(AddTemplate);
        AddCategoryCommand = new DelegateCommand(AddCategory);
        _pathManager = pathManager;
        _notificationService = notificationService;
        _templateStorage = templateStorage;
        _validator = validator;
        _templatesFilter = templatesFilter;
        _dialogService = dialogService;
        _cacheService = cacheService;
        _localisationService = localisationService;
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

    public override void OnLoad()
    {
        base.OnLoad();

        if (_cacheService.Contains("IconTiles"))
        {
            IconCodes = _cacheService.GetObject<ObservableCollection<IconCodeItem>>("IconTiles");
            return;
        }

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        var iconTiles = assets.GetAssets(new("avares://Slithin.Modules.Templates.UI/Resources/IconTiles"), null);

        foreach (var res in iconTiles)
        {
            var item = new IconCodeItem { Name = Path.GetFileNameWithoutExtension(res.ToString()) };

            var imageStrm = assets.Open(res);
            item.Image = Bitmap.DecodeToWidth(imageStrm, 32, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.Default);

            IconCodes.Add(item);
        }

        _cacheService.AddObject("IconTiles", IconCodes);
    }

    private async void AddCategory(object obj)
    {
        if (!string.IsNullOrEmpty(obj?.ToString()))
        {
            _templatesFilter.Categories.Add(obj.ToString());
        }
        else
        {
            await _dialogService.Show(_localisationService.GetString("Category name has to be set."));
        }
    }

    private async void AddTemplate(object obj)
    {
        var validationResult = _validator.Validate(this);

        if (!validationResult.IsValid)
        {
            _notificationService.ShowErrorNewWindow(validationResult.Errors.AsString());
            return;
        }

        var template = BuildTemplate();

        if (File.Exists(Path.Combine(_pathManager.TemplatesDir, template.Filename + ".png")))
        {
            if (await _dialogService.Show(_localisationService.GetString("Template already exist. Would you replace it?")))
            {
                File.Delete(Path.Combine(_pathManager.TemplatesDir, template.Filename + ".png"));
            }
            else
            {
                return;
            }
        }

        var bitmap = new Bitmap(Filename);

        if (bitmap.Size.Width != 1404 && bitmap.Size.Height != 1872)
        {
            await _dialogService.Show(
                _localisationService.GetString("The Template does not fit is not in correct dimenson. Please use a 1404x1872 dimension."));

            return;
        }

        File.Copy(Filename, Path.Combine(_pathManager.TemplatesDir, template.Filename + ".png"));

        //_localRepository.AddTemplate(template);

        await _templateStorage.LoadTemplateAsync(template);

        _templateStorage.AppendTemplate(template);
        _templatesFilter.Items.Add(template);

        template.TransferCommand.Execute(null);
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
