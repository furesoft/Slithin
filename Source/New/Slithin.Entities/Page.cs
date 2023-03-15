using Slithin.Modules.I18N.Models;

namespace Slithin.Entities;

public class Page
{
    public object DataContext { get; set; }
    public TranslatedString Header { get; set; }
    public object Icon { get; set; }
    public object Tag { get; set; }
}
