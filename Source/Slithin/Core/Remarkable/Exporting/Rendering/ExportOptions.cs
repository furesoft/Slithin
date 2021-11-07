using System.Collections.Generic;
using System.Linq;
using OneOf;
using PdfSharpCore.Pdf;
using Slithin.Core.Remarkable.Exporting;

namespace Slithin.Core.Remarkable.Rendering
{
    public class ExportOptions
    {
        public OneOf<PdfDocument, Notebook> Document { get; set; }
        public List<int> PagesIndices { get; set; } = new();

        public static ExportOptions Create(OneOf<PdfDocument, Notebook> document, string pageRange)
        {
            var result = new ExportOptions();

            var parsedPageRange = PageRange.Parse(pageRange);

            if (parsedPageRange.IsT0)
            {
                int max = 0;
                if (document.IsT0)
                {
                    max = document.AsT0.Pages.Count;
                }
                else if (document.IsT1)
                {
                    max = document.AsT1.Pages.Count;
                }

                result.PagesIndices = PageRange.ToIndices(parsedPageRange.AsT0, max).ToList();
            }

            result.Document = document;

            return result;
        }
    }
}
