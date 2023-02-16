namespace Slithin.Modules.Menu.Models.ContextualMenu;

public interface IContextualMenuProvider
{
    void RegisterContextualMenuElements(ContextualRegistrar registrar);
}
