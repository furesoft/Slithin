using AuroraModularis.Core;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Notebooks.UI.Models;

public class UpDirectoryModel : DirectoryModel
{
    public UpDirectoryModel() : base(null, null, false)
    {
        var localisation = Container.Current.Resolve<ILocalisationService>();

        VisibleName = localisation.GetString("Up ..");
    }
}
