using System.Collections.Generic;
using OneOf;
using PdfSharpCore.Pdf;

namespace Slithin.Core.Remarkable.Rendering
{
    public class ExportOptions
    {
        public OneOf<PdfDocument, Notebook> Document { get; set; }
        public List<int> PagesIndices { get; set; } = new();
    }
}
