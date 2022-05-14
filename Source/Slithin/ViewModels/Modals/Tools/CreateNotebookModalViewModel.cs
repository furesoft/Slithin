using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Renci.SshNet;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Exporting.Rendering;
using Slithin.Core.Remarkable.Models;
using Slithin.Core.Services;
using Slithin.Models;
using Slithin.Validators;

namespace Slithin.ViewModels.Modals.Tools;

public class CreateNotebookModalViewModel : ModalBaseViewModel
{
    private readonly ILoadingService _loadingService;
    private readonly ILocalisationService _localisationService;
    private readonly IMailboxService _mailboxService;
    private readonly IPathManager _pathManager;
    private readonly ScpClient _scp;
    private readonly CreateNotebookValidator _validator;
    private IImage _cover;

    private string _customTemplateFilename;
    private ObservableCollection<string> _defaultCovers;
    private string _filename;

    private string _pageCount;
    private bool _renderName;
    private Template _selectedTemplate;
    private ObservableCollection<Template> _templates = new();
    private string _title;

    public CreateNotebookModalViewModel(IPathManager pathManager,
        CreateNotebookValidator validator,
        ILoadingService loadingService,
        IMailboxService mailboxService,
        ILocalisationService localisationService,
        ScpClient scpClient)
    {
        AddPagesCommand = new DelegateCommand(AddPages);
        OKCommand = new DelegateCommand(OK);

        _pathManager = pathManager;
        _validator = validator;
        _loadingService = loadingService;
        _mailboxService = mailboxService;
        _localisationService = localisationService;
        _scp = scpClient;
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
            var strm = GetType().Assembly
                .GetManifestResourceStream("Slithin.Resources.Covers." + CoverFilename.Substring("internal:".Length) +
                                           ".png");
            Cover = Bitmap.DecodeToWidth(strm, 150);
        }
    }

    public override void OnLoad()
    {
        base.OnLoad();

        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        Cover = new Bitmap(assets.Open(new Uri("avares://Slithin/Resources/Covers/Folder-DBlue.png")));
        CoverFilename = "internal:Folder-DBlue.png";

        var coverNames = GetType().Assembly.GetManifestResourceNames()
            .Where(_ => _.StartsWith("Slithin.Resources.Covers."))
            .Select(_ => _.Replace(".png", "")["Slithin.Resources.Covers.".Length..]);

        DefaultCovers = new ObservableCollection<string>(coverNames);

        if (TemplateStorage.Instance.Templates != null)
        {
            Templates = new ObservableCollection<Template>(TemplateStorage.Instance.Templates);
            return;
        }

        _mailboxService.PostAction(() =>
        {
            NotificationService.Show(_localisationService.GetString("Loading Templates"));

            _loadingService.LoadTemplates();

            Templates = new ObservableCollection<Template>(TemplateStorage.Instance.Templates);
        });
    }

    private void AddPages(object obj)
    {
        if (!int.TryParse(PageCount, out var pcount) ||
            SelectedTemplate == null && string.IsNullOrEmpty(CustomTemplateFilename))
        {
            DialogService.OpenDialogError(_localisationService.GetString("Page Count must be a number and a template need to be selected"));
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
        string[] ids = new string[count];

        for (int i = 0; i < count; i++)
        {
            ids[i] = Guid.NewGuid().ToString().ToLower();
        }

        return ids;
    }

    private void OK(object obj)
    {
        var validationResult = _validator.Validate(this);

        if (!validationResult.IsValid)
        {
            DialogService.OpenDialogError(validationResult.Errors.First().ToString());
            return;
        }

        _mailboxService.PostAction(() =>
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

            NotificationService.Show(_localisationService.GetStringFormat("Generating {0}", md.VisibleName));

            Stream coverStream = null;

            if (CoverFilename.StartsWith("custom:"))
            {
                coverStream = File.OpenRead(CoverFilename.Substring("custom:".Length));
            }
            else
            {
                coverStream = GetType().Assembly
                    .GetManifestResourceStream("Slithin.Resources.Covers." + CoverFilename.Substring("internal:".Length) +
                                               ".png");
            }

            if (coverStream == null)
            {
                coverStream = GetType().Assembly.GetManifestResourceStream("Slithin.Resources.Cover.png");
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

            md.Save();

            document.Save(_pathManager.NotebooksDir + $"\\{md.ID}.pdf");

            MetadataStorage.Local.AddMetadata(md, out var alreadyAdded);

            SyncService.NotebooksFilter.Documents.Add(md);
            SyncService.NotebooksFilter.SortByFolder();

            Notebook.UploadDocument(md);
        });

        DialogService.Close();
    }
}
