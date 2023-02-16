using EpubSharp;
using OneOf;
using PdfSharpCore.Pdf;
using Slithin.Entities.Remarkable.Rendering;

namespace Slithin.Modules.Export.Models;

[GenerateOneOf]
public partial class ExportDocument : OneOfBase<PdfDocument, Notebook, EpubBook>
{
}
