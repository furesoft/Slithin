using AuroraModularis.Core;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Slithin.Entities.Remarkable;
using Slithin.Modules.I18N.Models;

namespace Slithin.Modules.Notebooks.UI;

public static class NotebooksView
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
        /*
        NotebooksFilter.Documents.Clear();

        var id = md.ID;

        if (!string.IsNullOrEmpty(md.VisibleName) && md.VisibleName.Equals(localisation.GetString("Up ..")))
        {
            id = _lastFolderIDs.Pop();
            NotebooksFilter.Folder = id;

            if (id == "")
            {
                NotebooksFilter.Documents.Add(new Metadata
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

        foreach (var mds in MetadataStorage.Local.GetByParent(id))
        {
            NotebooksFilter.Documents.Add(mds);
        }

        if (_lastFolderIDs.Count > 0)
        {
            NotebooksFilter.Documents.Add(new Metadata { Type = "CollectionType", VisibleName = localisation.GetString("Up ..") });
            NotebooksFilter.Folder = id;
        }
        else
        {
            NotebooksFilter.Folder = "";
        }

        NotebooksFilter.SortByFolder();
        */
    }
}
