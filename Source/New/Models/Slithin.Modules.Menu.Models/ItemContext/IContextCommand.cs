using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Menu.Models.ItemContext;

public interface IContextCommand
{
    public object ParentViewModel { get; set; }
    public TranslatedString Title { get; }

    bool CanExecute(object data);

    void Execute(object data);
}
