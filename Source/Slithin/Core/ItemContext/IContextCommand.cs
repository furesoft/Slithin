namespace Slithin.Core.ItemContext;

public interface IContextCommand
{
    public object ParentViewModel { get; set; }
    public string Titel { get; }

    bool CanHandle(object data);

    void Invoke(object data);
}
