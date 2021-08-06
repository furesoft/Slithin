using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Newtonsoft.Json;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Scripting;
using Slithin.Core.Sync;

namespace Slithin.ViewModels
{
    public class CreateNotebookModalViewModel : BaseViewModel
    {
        private IImage _cover;
        private string _filename;
        private string _name;

        private string _pageCount;

        private bool _renderName;
        private Template _selectedTemplate;

        public CreateNotebookModalViewModel()
        {
            Templates = new ObservableCollection<Template>(TemplateStorage.Instance.Templates);

            Categories = SyncService.TemplateFilter.Categories;

            AddPagesCommand = new DelegateCommand(AddPages);
            OKCommand = new DelegateCommand(OK);

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            Cover = new Bitmap(assets.Open(new Uri($"avares://Slithin/Resources/Cover.png")));
        }

        public ICommand AddPagesCommand { get; set; }

        public ObservableCollection<string> Categories { get; set; } = new();

        public IImage Cover
        {
            get { return _cover; }
            set { SetValue(ref _cover, value); }
        }

        public string Filename
        {
            get { return _filename; }
            set { SetValue(ref _filename, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public ICommand OKCommand { get; set; }

        public string PageCount
        {
            get { return _pageCount; }
            set { SetValue(ref _pageCount, value); }
        }

        public ObservableCollection<(Template, int)> Pages { get; set; } = new();

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

        public ObservableCollection<Template> Templates { get; set; } = new();

        public void LoadCover()
        {
            using var strm = File.OpenRead(Filename);
            Cover = Bitmap.DecodeToWidth(strm, 150, Avalonia.Visuals.Media.Imaging.BitmapInterpolationMode.HighQuality);
        }

        private void AddPages(object obj)
        {
            Pages.Add((SelectedTemplate, int.Parse(PageCount)));

            SelectedTemplate = null;
            PageCount = null;
        }

        private void OK(object obj)
        {
            var document = new PdfDocument();
            document.PageLayout = PdfPageLayout.SinglePage;
            document.PageMode = PdfPageMode.FullScreen;
            document.Info.Author = "Slithin";
            document.Info.Title = Name;

            XSize size = PageSizeConverter.ToSize(PageSize.A4);

            var coverPage = document.AddPage();
            var coverGfx = XGraphics.FromPdfPage(coverPage);

            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();

            var coverImage = XImage.FromStream(() => assets.Open(new Uri($"avares://Slithin/Resources/Cover.png")));
            coverGfx.DrawImage(coverImage, 0, 0, coverPage.Width, coverPage.Height);

            if (RenderName)
            {
                var font = new XFont("Arial", 125);
                XSize fontSize = coverGfx.MeasureString(Name, font);
                coverGfx.DrawString(Name, font, XBrushes.Black, new XPoint(coverPage.Width / 2 - fontSize.Width / 2, 260));
            }

            foreach (var p in Pages)
            {
                var t = XImage.FromFile(ServiceLocator.TemplatesDir + "\\" + p.Item1.Filename + ".png");

                for (int i = 0; i < p.Item2; i++)
                {
                    var page = document.AddPage();
                    page.Size = PageSize.A4;

                    var gfx = XGraphics.FromPdfPage(page);

                    gfx.DrawImage(t, 0, 0, page.Width, page.Height);
                }
            }

            var md = new Metadata();
            md.ID = Guid.NewGuid().ToString();
            md.VisibleName = Name;
            md.Type = "DocumentType";
            md.Version = 1;
            md.Parent = "";

            md.Content = new() { FileType = "pdf", CoverPageNumber = 0, PageCount = document.Pages.Count };

            File.WriteAllText(Path.Combine(ServiceLocator.NotebooksDir, md.ID + ".metadata"), JsonConvert.SerializeObject(md, Formatting.Indented));
            File.WriteAllText(Path.Combine(ServiceLocator.NotebooksDir, md.ID + ".content"), JsonConvert.SerializeObject(md.Content, Formatting.Indented));

            document.Save(ServiceLocator.NotebooksDir + $"\\{md.ID}.pdf");

            MetadataStorage.Local.Add(md, out var alreadyAdded);

            SyncService.NotebooksFilter.Documents.Add(md);

            var syncItem = new SyncItem();
            syncItem.Action = SyncAction.Add;
            syncItem.Data = md;
            syncItem.Direction = SyncDirection.ToDevice;
            syncItem.Type = SyncType.Notebook;

            SyncService.SyncQueue.Insert(syncItem);

            DialogService.Close();
        }
    }
}
