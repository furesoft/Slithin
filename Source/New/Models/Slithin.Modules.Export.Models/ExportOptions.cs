namespace Slithin.Modules.Export.Models;

public class ExportOptions
{
    public ExportDocument Document { get; set; }
    public List<int> PagesIndices { get; set; } = new();

    public bool ShouldHideTemplates { get; set; }

    public static ExportOptions Create(ExportDocument document, string pageRange)
    {
        var result = new ExportOptions();

        var parsedPageRange = PageRange.Parse(pageRange);

        if (parsedPageRange.IsT0)
        {
            var max = 0;
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
