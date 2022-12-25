using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;
using Slithin.Modules.Notebooks.UI.ViewModels;
using Slithin.Modules.Repository.Models;
using Slithin.Modules.Sync.Models;

namespace Slithin.Modules.Notebooks.UI;

internal static class NotebooksView
{
    private static readonly Stack<string> _lastFolderIDs = new();
    private static ListBox _lb;

    public static bool GetIsView(Control control)
    {
        return control == _lb;
    }

    public static void SetIsView(ListBox lb, bool value)
    {
        _lb = lb;

        _lb.DoubleTapped += _lb_DoubleTapped;
    }

    private static void _lb_DoubleTapped(object sender, RoutedEventArgs e)
    {
        if (_lb.SelectedItem is not Metadata md || md.Type != "CollectionType" ||
            _lb.DataContext is not NotebooksPageViewModel vm)
        {
            return;
        }

        var localisation = Container.Current.Resolve<ILocalisationService>();
        var notebooksFilter = Container.Current.Resolve<NotebooksFilter>();
        var mdStorage = Container.Current.Resolve<IMetadataRepository>();

        notebooksFilter.Documents.Clear();

        var id = md.ID;

        if (!string.IsNullOrEmpty(md.VisibleName) && md.VisibleName.Equals(localisation.GetString("Up ..")))
        {
            id = _lastFolderIDs.Pop();
            notebooksFilter.Folder = id;

            if (id == "")
            {
                notebooksFilter.Documents.Add(new Metadata
                {
                    Type = "CollectionType",
                    VisibleName = localisation.GetString("Trash"),
                    ID = "trash"
                });
            }
        }
        else if (md.VisibleName == localisation.GetString("Trash"))
        {
            id = "trash";
            _lastFolderIDs.Push("");
        }
        else
        {
            _lastFolderIDs.Push(md.Parent);
        }

        vm.IsInTrash = id == "trash";

        foreach (var mds in mdStorage.GetByParent(id))
        {
            notebooksFilter.Documents.Add(mds);
        }

        if (_lastFolderIDs.Count > 0)
        {
            notebooksFilter.Documents.Add(new Metadata { Type = "CollectionType", VisibleName = localisation.GetString("Up ..") });
            notebooksFilter.Folder = id;
        }
        else
        {
            notebooksFilter.Folder = "";
        }

        notebooksFilter.SortByFolder();
    }
}
