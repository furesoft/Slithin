﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Slithin.Core;
using Slithin.Core.MVVM;
using Slithin.Entities.Remarkable;
using Slithin.Entities.Remarkable.Rendering;
using Slithin.Modules.BaseServices.Models;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.PdfNotebookTools.Models;
using Slithin.Modules.PdfNotebookTools.Validators;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;
using Slithin.Modules.UI.Models;

namespace Slithin.Modules.PdfNotebookTools.ViewModels;

public class CreateNotebookModalViewModel : ModalBaseViewModel
{
    private readonly ITemplateStorage _templateStorage;
    private readonly NotebooksFilter _notebooksFilter;
    private readonly ILocalisationService _localisationService;
    private readonly CreateNotebookValidator _validator;
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
        ILocalisationService localisationService, CreateNotebookValidator validator)
    {
        AddPagesCommand = new DelegateCommand(AddPages);
        OKCommand = new DelegateCommand(Ok);

        _pathManager = pathManager;
        _dialogService = dialogService;
        _notificationService = notificationService;
        _metadataRepository = metadataRepository;
        _templateStorage = templateStorage;
        _notebooksFilter = notebooksFilter;
        _localisationService = localisationService;
        _validator = validator;
    }

    public ICommand AddPagesCommand { get; }

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

    public ICommand OKCommand { get; }

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

            var strm = assets.Open(new($"avares://Slithin.Modules.PdfNotebookTools/Resources/Covers/Folder-{CoverFilename.Substring("internal:".Length)}.png"));
            Cover = Bitmap.DecodeToWidth(strm, 150);
        }
    }

    protected override void OnLoad()
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

        Cover = new Bitmap(assets!.Open(new("avares://Slithin.Modules.PdfNotebookTools/Resources/Covers/Folder-DBlue.png")));
        CoverFilename = "internal:Folder-DBlue.png";

        var coverNames = assets.GetAssets(new("avares://Slithin.Modules.PdfNotebookTools/Resources/Covers/"), null)
                                        .Select(_ => Path.GetFileNameWithoutExtension(_.ToString())[7..]);

        DefaultCovers = new(coverNames);

        Templates = new(_templateStorage.Templates);
    }

    private async void AddPages(object obj)
    {
        if (!int.TryParse(PageCount, out var pcount) ||
            SelectedTemplate == null && string.IsNullOrEmpty(CustomTemplateFilename))
        {
            await _dialogService.Show("Page Count must be a number and a template need to be selected");
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

    private ContentPage[] GeneratePageIDS(int count)
    {
        return Enumerable.Range(0, count)
            .Select( _ => 
                Guid.NewGuid()
                    .ToString()
                    .ToLower())
            .Select(_=> new ContentPage{ ID = _ })
            .ToArray();
    }

    private async void Ok(object obj)
    {
        var validationResult = await _validator.ValidateAsync(this);

        if (!validationResult.IsValid)
        {
            _notificationService.ShowErrorNewWindow(validationResult.Errors.AsString());
            return;
        }

        await DoWork();
    }

    private async Task DoWork()
    {
        await Task.Run(() =>
        {
            var document = InitPdfDocument();

            var coverPage = document.AddPage();
            var coverGfx = XGraphics.FromPdfPage(coverPage);

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            var md = CreateMetadata(document);
            var status =
                _notificationService.ShowStatus(_localisationService.GetStringFormat("Generating {0}", md.VisibleName));

            RenderCover(assets, coverGfx, coverPage);
            RenderPages(document);

            SaveNotebookToFile(md, document);

            _notebooksFilter.Items.Add(new Notebooks.UI.Models.FileModel(md.VisibleName, md, md.IsPinned));
            _notebooksFilter.SortByFolder();

            status.Step("Uploading");

            Notebook.UploadDocument(md);
        });
    }

    private void SaveNotebookToFile(Metadata md, PdfDocument document)
    {
        _metadataRepository.SaveToDisk(md);
        _metadataRepository.AddMetadata(md, out _);

        document.Save(Path.Combine(_pathManager.NotebooksDir, $"{md.ID}.pdf"));
    }

    private PdfDocument InitPdfDocument()
    {
        var document = new PdfDocument {PageLayout = PdfPageLayout.SinglePage, PageMode = PdfPageMode.FullScreen};
        document.Info.Author = "Slithin";
        document.Info.Title = Title;
        return document;
    }

    private void RenderCover(IAssetLoader? assets, XGraphics coverGfx, PdfPage coverPage)
    {
        var coverStream = GetCoverStream(assets);
        var coverImage = XImage.FromStream(() => coverStream);
        coverGfx.DrawImage(coverImage, 3, 3, coverPage.Width - 6, coverPage.Height - 6);

        RenderTitle(coverGfx, coverPage);
    }

    private void RenderPages(PdfDocument document)
    {
        foreach (var p in Pages)
        {
            RenderPage(p, document);
        }
    }

    private Metadata CreateMetadata(PdfDocument document)
    {
        var md = new Metadata
        {
            ID = Guid.NewGuid().ToString().ToLower(),
            VisibleName = Title,
            Type = "DocumentType",
            Version = 1,
            Parent = "",
            Content = new()
            {
                FileType = "pdf", CoverPageNumber = 0, PageCount = document.Pages.Count,
                Pages = GeneratePageIDS(document.Pages.Count)
            }
        };
        return md;
    }

    private void RenderPage(object p, PdfDocument document)
    {
        XImage image = null;
        var count = 0;

        if (p is NotebookPage nbp)
        {
            count = nbp.Count;
            image = XImage.FromFile($"{_pathManager.TemplatesDir}\\{nbp.Template.Filename}.png");
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

    private void RenderTitle(XGraphics coverGfx, PdfPage coverPage)
    {
        if (!RenderName)
        {
            return;
        }

        var font = new XFont("Arial", 125);
        var fontSize = coverGfx.MeasureString(Title, font);
        coverGfx.DrawString(Title, font, XBrushes.Black, new XPoint(coverPage.Width / 2 - fontSize.Width / 2, 260));
    }

    private Stream GetCoverStream(IAssetLoader? assets)
    {
        Stream coverStream;

        if (CoverFilename.StartsWith("custom:"))
        {
            coverStream = File.OpenRead(CoverFilename["custom:".Length..]);
        }
        else
        {
            coverStream =
                assets.Open(new(
                    $"avares://Slithin.Modules.PdfNotebookTools/Resources/Covers/{CoverFilename["internal:".Length..]}"));
        }

        if (coverStream == null)
        {
            coverStream = assets.Open(new("avares://Slithin.Modules.PdfNotebookTools/Resources/Cover.png"));
        }

        return coverStream;
    }
}
