using AuroraModularis.Core;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Notebooks.UI.Models;

public class UpDirectoryModel : DirectoryModel
{
    public UpDirectoryModel(FileSystemModel parentFolder) : base(null, null, false)
    {
        ParentFolder = parentFolder;
        var localisation = ServiceContainer.Current.Resolve<ILocalisationService>();

        VisibleName = localisation.GetString("Up ..");
    }

    public FileSystemModel ParentFolder { get; }
}
