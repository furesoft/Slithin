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
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Core.Validators;
using Slithin.Models;

namespace Slithin.ViewModels.Modals
{
    public class CreateNotebookModalViewModel : BaseViewModel
    {
        private readonly ILoadingService _loadingService;
        private readonly IMailboxService _mailboxService;
        private readonly IPathManager _pathManager;
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
                                            IMailboxService mailboxService)
        {
            AddPagesCommand = new DelegateCommand(AddPages);
            OKCommand = new DelegateCommand(OK);

            _pathManager = pathManager;
            _validator = validator;
            _loadingService = loadingService;
            _mailboxService = mailboxService;
        }

        public ICommand AddPagesCommand { get; set; }

        public IImage Cover
        {
            get { return _cover; }
            set { SetValue(ref _cover, value); }
        }

        public string CoverFilename
        {
            get { return _filename; }
            set { SetValue(ref _filename, value); }
        }

        public string CustomTemplateFilename
        {
            get { return _customTemplateFilename; }
            set { SetValue(ref _customTemplateFilename, value); }
        }

        public ObservableCollection<string> DefaultCovers
        {
            get { return _defaultCovers; }
            set { SetValue(ref _defaultCovers, value); }
        }

        public ICommand OKCommand { get; set; }

        public string PageCount
        {
            get { return _pageCount; }
            set { SetValue(ref _pageCount, value); }
        }

        public ObservableCollection<object> Pages { get; set; } = new();

        public bool RenderName
        {
            get { return _renderName; }
            set { SetValue(ref _renderName, value); }
        }

        public Template SelectedTemplate
        {
            get { return _selectedTemplate; }
            set { SetValue(ref _selectedTemplate, value); }
        }

        public ObservableCollection<Template> Templates
        {
            get { return _templates; }
            set { SetValue(ref _templates, value); }
        }

        public string Title
        {
            get { return _title; }
            set { SetValue(ref _title, value); }
        }

        public void LoadCover()
        {
            if (CoverFilename.StartsWith("custom:"))
            {
                using var strm = File.OpenRead(CoverFilename.Substring("custom:".Length));
                Cover = Bitmap.DecodeToWidth(strm, 150, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality);
            }
            else
            {
                using var strm = GetType().Assembly.GetManifestResourceStream("Slithin.Resources.Covers." + CoverFilename.Substring("internal:".Length) + ".png");
                Cover = Bitmap.DecodeToWidth(strm, 150, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality);
            }
        }

        public override void OnLoad()
        {
            base.OnLoad();

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            Cover = new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/Covers/Folder-DBlue.png")));
            CoverFilename = "internal:Folder-DBlue.png";

            var coverNames = GetType().Assembly.GetManifestResourceNames().
                Where(_ => _.StartsWith("Slithin.Resources.Covers.")).
                Select(_ => _.Replace(".png", "")["Slithin.Resources.Covers.".Length..]);

            DefaultCovers = new ObservableCollection<string>(coverNames);

            if (TemplateStorage.Instance.Templates == null)
            {
                _mailboxService.PostAction(() =>
                {
                    _loadingService.LoadTemplates();

                    Templates = new ObservableCollection<Template>(TemplateStorage.Instance.Templates);
                });
            }
        }

        private void AddPages(object obj)
        {
            if (int.TryParse(PageCount, out var pcount) && (SelectedTemplate != null || !string.IsNullOrEmpty(CustomTemplateFilename)))
            {
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
            else
            {
                DialogService.OpenDialogError("Page Count must be a number and a template need to be selected");
            }
        }

        private void OK(object obj)
        {
            var validationResult = _validator.Validate(this);

            if (validationResult.IsValid)
            {
                var document = new PdfDocument
                {
                    PageLayout = PdfPageLayout.SinglePage,
                    PageMode = PdfPageMode.FullScreen
                };
                document.Info.Author = "Slithin";
                document.Info.Title = Title;

                var size = PageSizeConverter.ToSize(PageSize.A4);

                var coverPage = document.AddPage();
                var coverGfx = XGraphics.FromPdfPage(coverPage);

                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

                Stream coverStream = null;
                if (CoverFilename.StartsWith("custom:"))
                {
                    coverStream = File.OpenRead(CoverFilename.Substring("custom:".Length));
                }
                else
                {
                    coverStream = GetType().Assembly.GetManifestResourceStream("Slithin.Resources.Covers." + CoverFilename.Substring("internal:".Length) + ".png");
                }

                if (coverStream == null)
                {
                    coverStream = GetType().Assembly.GetManifestResourceStream("Slithin.Resources.Cover.png");
                }

                var coverImage = XImage.FromStream(() => coverStream);
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
                    int count = 0;

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

                var md = new Metadata
                {
                    ID = Guid.NewGuid().ToString().ToLower(),
                    VisibleName = Title,
                    Type = "DocumentType",
                    Version = 1,
                    Parent = "",

                    Content = new() { FileType = "pdf", CoverPageNumber = 0, PageCount = document.Pages.Count }
                };

                md.Save();

                document.Save(_pathManager.NotebooksDir + $"\\{md.ID}.pdf");

                MetadataStorage.Local.Add(md, out var alreadyAdded);

                SyncService.NotebooksFilter.Documents.Add(md);
                SyncService.NotebooksFilter.SortByFolder();

                var syncItem = new SyncItem
                {
                    Action = SyncAction.Add,
                    Data = md,
                    Direction = SyncDirection.ToDevice,
                    Type = SyncType.Notebook
                };

                SyncService.SyncQueue.Insert(syncItem);

                DialogService.Close();
            }
            else
            {
                DialogService.OpenDialogError(validationResult.Errors.First().ToString());
            }
        }
    }
}
