using System.Collections.ObjectModel;

namespace Slithin.Core.Sync
{
    public class ToolsFilter : NotifyObject
    {
        public ObservableCollection<string> Categories { get; set; } = new();
    }
}
