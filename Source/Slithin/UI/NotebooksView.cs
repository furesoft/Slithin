using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Slithin.Core.Remarkable;
using Slithin.ViewModels.Pages;

namespace Slithin.UI;

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

        vm.SyncService.NotebooksFilter.Documents.Clear();

        var id = md.ID;

        if (md.VisibleName.Equals("Up .."))
        {
            id = _lastFolderIDs.Pop();
            vm.SyncService.NotebooksFilter.Folder = id;

            if (id == "")
            {
                vm.SyncService.NotebooksFilter.Documents.Add(new Metadata
                {
                    Type = "CollectionType", VisibleName = "Trash", ID = "trash"
                });
            }
        }
        else if (md.VisibleName == "Trash")
        {
            id = "trash";
            _lastFolderIDs.Push("");
        }
        else
        {
            _lastFolderIDs.Push(md.Parent);
        }

        foreach (var mds in MetadataStorage.Local.GetByParent(id))
        {
            vm.SyncService.NotebooksFilter.Documents.Add(mds);
        }

        if (_lastFolderIDs.Count > 0)
        {
            vm.SyncService.NotebooksFilter.Documents.Add(new Metadata {Type = "CollectionType", VisibleName = "Up .."});
            vm.SyncService.NotebooksFilter.Folder = id;
        }
        else
        {
            vm.SyncService.NotebooksFilter.Folder = "";
        }

        vm.SyncService.NotebooksFilter.SortByFolder();
    }
}
