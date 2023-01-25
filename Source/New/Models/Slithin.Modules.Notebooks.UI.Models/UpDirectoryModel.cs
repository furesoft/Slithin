using AuroraModularis.Core;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Notebooks.UI.Models;

public class UpDirectoryModel : DirectoryModel
{
    public FileSystemModel ParentFolder { get; }

    public UpDirectoryModel(FileSystemModel parentFolder) : base(null, null, false)
    {
        ParentFolder = parentFolder;
        var localisation = Container.Current.Resolve<ILocalisationService>();

        VisibleName = localisation.GetString("Up ..");
    }
}
