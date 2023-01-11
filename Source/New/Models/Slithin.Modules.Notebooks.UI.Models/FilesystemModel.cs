using Slithin.Core.MVVM;

namespace Slithin.Modules.Notebooks.UI.Models;

public class FilesystemModel : NotifyObject
{
    private string _visibleName;

    private object _tag;

    private bool _isPinned;

    public FilesystemModel(string visibleName, object tag, bool isPinned)
    {
        VisibleName = visibleName;
        Tag = tag;
        IsPinned = isPinned;
    }

    public string ID { get; set; }
    public string Parent { get; set; }

    public virtual string VisibleName
    {
        get => _visibleName;
        set => SetValue(ref _visibleName, value);
    }

    public virtual object Tag
    {
        get { return _tag; }
        set { SetValue(ref _tag, value); }
    }

    public virtual bool IsPinned
    {
        get { return _isPinned; }
        set { SetValue(ref _isPinned, value); }
    }

    public override string ToString()
    {
        return VisibleName;
    }
}
