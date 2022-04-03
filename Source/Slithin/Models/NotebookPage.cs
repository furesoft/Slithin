using Slithin.Core.Remarkable;
using Slithin.Core.Remarkable.Models;

namespace Slithin.Models;

public class NotebookPage
{
    public NotebookPage(Template template, int count)
    {
        Template = template;
        Count = count;
    }

    public int Count { get; set; }
    public Template Template { get; set; }
}