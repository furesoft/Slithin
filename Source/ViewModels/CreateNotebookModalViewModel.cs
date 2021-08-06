using System;
using System.Collections.ObjectModel;
using System.IO;
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
using Slithin.Core.Scripting;

namespace Slithin.ViewModels
{
    public class CreateNotebookModalViewModel : BaseViewModel
    {
        private IImage _cover;
        private string _filename;
        private string _name;

        private string _pageCount;

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

            document.Save(ServiceLocator.ConfigBaseDir + "\\test.pdf");

            DialogService.Close();
        }
    }
}
