using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Modules.Export.Models.Rendering;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.PdfNotebookTools.Models;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.PdfNotebookTools.ViewModels;

public class CreateNotebookModalViewModel : ModalBaseViewModel
{
    private readonly ILoadingService _loadingService;
    private readonly ITemplateStorage _templateStorage;
    private readonly NotebooksFilter _notebooksFilter;
    private readonly ILocalisationService _localisationService;
    private readonly IPathManager _pathManager;
    private readonly IDialogService _dialogService;
    private readonly INotificationService _notificationService;
    private readonly IMetadataRepository _metadataRepository;
    private IImage _cover;

    private string _customTemplateFilename;
    private ObservableCollection<string> _defaultCovers;
    private string _filename;

    private string _pageCount;
    private bool _renderName;
    private Template _selectedTemplate;
    private ObservableCollection<Template> _templates = new();
    private string _title;

    public CreateNotebookModalViewModel(IPathManager pathManager, IDialogService dialogService,
        INotificationService notificationService, IMetadataRepository metadataRepository,
        ILoadingService loadingService, ITemplateStorage templateStorage, NotebooksFilter notebooksFilter,
        ILocalisationService localisationService)
    {
        AddPagesCommand = new DelegateCommand(AddPages);
        OKCommand = new DelegateCommand(OK);

        _pathManager = pathManager;
        _dialogService = dialogService;
        _notificationService = notificationService;
        _metadataRepository = metadataRepository;
        _loadingService = loadingService;
        _templateStorage = templateStorage;
        _notebooksFilter = notebooksFilter;
        _localisationService = localisationService;
    }

    public ICommand AddPagesCommand { get; set; }

    public IImage Cover
    {
        get => _cover;
        set => SetValue(ref _cover, value);
    }

    public string CoverFilename
    {
        get => _filename;
        set => SetValue(ref _filename, value);
    }

    public string CustomTemplateFilename
    {
        get => _customTemplateFilename;
        set => SetValue(ref _customTemplateFilename, value);
    }

    public ObservableCollection<string> DefaultCovers
    {
        get => _defaultCovers;
        set => SetValue(ref _defaultCovers, value);
    }

    public ICommand OKCommand { get; set; }

    public string PageCount
    {
        get => _pageCount;
        set => SetValue(ref _pageCount, value);
    }

    public ObservableCollection<object> Pages { get; set; } = new();

    public bool RenderName
    {
        get => _renderName;
        set => SetValue(ref _renderName, value);
    }

    public Template SelectedTemplate
    {
        get => _selectedTemplate;
        set => SetValue(ref _selectedTemplate, value);
    }

    public ObservableCollection<Template> Templates
    {
        get => _templates;
        set => SetValue(ref _templates, value);
    }

    public string Title
    {
        get => _title;
        set => SetValue(ref _title, value);
    }

    public void LoadCover()
    {
        if (CoverFilename.StartsWith("custom:"))
        {
            var strm = File.OpenRead(CoverFilename.Substring("custom:".Length));
            Cover = Bitmap.DecodeToWidth(strm, 150);
        }
        else
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            var strm = assets.Open(new Uri($"avares://Slithin.Modules.PdfNotebookTools/Resources/Covers/Folder-{CoverFilename.Substring("internal:".Length)}.png"));
            Cover = Bitmap.DecodeToWidth(strm, 150);
        }
    }

    public override void OnLoad()
    {
        base.OnLoad();

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        Cover = new Bitmap(assets.Open(new Uri("avares://Slithin.Modules.PdfNotebookTools/Resources/Covers/Folder-DBlue.png")));
        CoverFilename = "internal:Folder-DBlue.png";

        var coverNames = assets.GetAssets(new Uri("avares://Slithin.Modules.PdfNotebookTools/Resources/Covers/"), null)
                                        .Select(_ => Path.GetFileNameWithoutExtension(_.ToString()).Substring(7));

        DefaultCovers = new ObservableCollection<string>(coverNames);

        Templates = new ObservableCollection<Template>(_templateStorage.Templates);
    }

    private async void AddPages(object obj)
    {
        if (!int.TryParse(PageCount, out var pcount) ||
            SelectedTemplate == null && string.IsNullOrEmpty(CustomTemplateFilename))
        {
            await _dialogService.Show(_localisationService.GetString("Page Count must be a number and a template need to be selected"));
            return;
        }

        if (!string.IsNullOrEmpty(CustomTemplateFilename))
        {
            Pages.Add(new NotebookCustomPage(CustomTemplateFilename, pcount));
        }
        else
        {
            Pages.Add(new NotebookPage(SelectedTemplate, pcount));
        }

        SelectedTemplate = null;
        PageCount = null;
        CustomTemplateFilename = null;
    }

    private string[] GeneratePageIDS(int count)
    {
        var ids = new string[count];

        for (var i = 0; i < count; i++)
        {
            ids[i] = Guid.NewGuid().ToString().ToLower();
        }

        return ids;
    }

    private async void OK(object obj)
    {
        /*
        var validationResult = _validator.Validate(this);

        if (!validationResult.IsValid)
        {
            DialogService.OpenError(validationResult.Errors.First().ToString());
            return;
        }*/

        await Task.Run(() =>
        {
            var document = new PdfDocument { PageLayout = PdfPageLayout.SinglePage, PageMode = PdfPageMode.FullScreen };
            document.Info.Author = "Slithin";
            document.Info.Title = Title;

            var size = PageSizeConverter.ToSize(PageSize.A4);

            var coverPage = document.AddPage();
            var coverGfx = XGraphics.FromPdfPage(coverPage);

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            var md = new Metadata
            {
                ID = Guid.NewGuid().ToString().ToLower(),
                VisibleName = Title,
                Type = "DocumentType",
                Version = 1,
                Parent = "",
                Content = new ContentFile { FileType = "pdf", CoverPageNumber = 0, PageCount = document.Pages.Count, Pages = GeneratePageIDS(document.Pages.Count) }
            };
            var status = _notificationService.ShowStatus(_localisationService.GetStringFormat("Generating {0}", md.VisibleName));

            Stream coverStream = null;

            if (CoverFilename.StartsWith("custom:"))
            {
                coverStream = File.OpenRead(CoverFilename.Substring("custom:".Length));
            }
            else
            {
                coverStream = assets.Open(new Uri($"avares://Slithin.Modules.PdfNotebookTools/Resources/Covers/Folder-{CoverFilename.Substring("internal:".Length)}.png"));
            }

            if (coverStream == null)
            {
                coverStream = assets.Open(new Uri("avares://Slithin.Modules.PdfNotebookTools/Resources/Cover.png"));
            }

            var coverStreamCached = delegate () { return coverStream; };

            var coverImage = XImage.FromStream(() => coverStream);

            var jpedCover = coverImage.AsJpeg();
            coverGfx.DrawImage(coverImage, 3, 3, coverPage.Width - 6, coverPage.Height - 6);

            if (RenderName)
            {
                var font = new XFont("Arial", 125);
                var fontSize = coverGfx.MeasureString(Title, font);
                coverGfx.DrawString(Title, font, XBrushes.Black, new XPoint(coverPage.Width / 2 - fontSize.Width / 2, 260));
            }

            foreach (var p in Pages)
            {
                XImage image = null;
                var count = 0;

                if (p is NotebookPage nbp)
                {
                    count = nbp.Count;
                    image = XImage.FromFile(_pathManager.TemplatesDir + "\\" + nbp.Template.Filename + ".png");
                }
                else if (p is NotebookCustomPage nbcp)
                {
                    image = XImage.FromFile(nbcp.Filename);
                    count = nbcp.Count;
                }

                for (var i = 0; i < count; i++)
                {
                    var page = document.AddPage();
                    page.Size = PageSize.A4;

                    var gfx = XGraphics.FromPdfPage(page);

                    gfx.DrawImage(image, 0, 0, page.Width, page.Height);
                }
            }

            _metadataRepository.SaveToDisk(md);

            document.Save(Path.Combine(_pathManager.NotebooksDir, $"\\{md.ID}.pdf"));

            _metadataRepository.AddMetadata(md, out var alreadyAdded);

            _notebooksFilter.Documents.Add(md);
            _notebooksFilter.SortByFolder();

            status.Step("Uploading");

            Notebook.UploadDocument(md);
        });
    }
}
