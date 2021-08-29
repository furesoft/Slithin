using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;
using Slithin.Core.Sync;
using Slithin.Models;

namespace Slithin.ViewModels.Modals
{
    public class AppendNotebookModalViewModel : BaseViewModel
    {
        private readonly ILoadingService _loadingService;
        private readonly IPathManager _pathManager;
        private readonly AppendNotebookValidator _validator;
        private string _customTemplateFilename;
        private string _pageCount;
        private Template _selectedTemplate;
        private ObservableCollection<Template> _templates = new();

        public AppendNotebookModalViewModel(IPathManager pathManager,
                                            AppendNotebookValidator validator,
                                            ILoadingService loadingService)
        {
            _pathManager = pathManager;
            _validator = validator;
            _loadingService = loadingService;

            AddPagesCommand = new DelegateCommand(AddPages);
            OKCommand = new DelegateCommand(OK);
        }

        public ICommand AddPagesCommand { get; set; }

        public string CustomTemplateFilename
        {
            get { return _customTemplateFilename; }
            set { SetValue(ref _customTemplateFilename, value); }
        }

        public ICommand OKCommand { get; set; }

        public string PageCount
        {
            get { return _pageCount; }
            set { SetValue(ref _pageCount, value); }
        }

        public ObservableCollection<object> Pages { get; set; } = new();

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

        public override void OnLoad()
        {
            base.OnLoad();

            if (TemplateStorage.Instance.Templates == null)
            {
                _loadingService.LoadTemplates();
            }

            Templates = new ObservableCollection<Template>(TemplateStorage.Instance.Templates);
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
                var document = PdfReader.Open();

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

                var syncItem = new SyncItem
                {
                    Action = SyncAction.Update,
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
