namespace Slithin.Modules.Menu.Models.Menu;

public interface IPage
{
    string Title { get; }

    bool IsEnabled();
}
