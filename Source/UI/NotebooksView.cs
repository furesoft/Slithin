using System.Linq;
using Avalonia.Controls;
using Slithin.Core.Remarkable;
using Slithin.ViewModels;

using System.Linq;

using System.Collections.Generic;

namespace Slithin.UI
{
    public static class NotebooksView
    {
        private static Stack<string> _lastFolderIDs = new();
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

        private static void _lb_DoubleTapped(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_lb.SelectedItem is Metadata md && md.Type == MetadataType.CollectionType)
            {
                if (_lb.DataContext is NotebooksPageViewModel vm)
                {
                    //ToDo: change to foldermanager
                    var item = (_lb.Items as IEnumerable<Metadata>).ToArray();
                    vm.SyncService.NotebooksFilter.Documents.Clear();

                    var id = md.ID;

                    if (md.VisibleName.Equals("Up .."))
                    {
                        id = _lastFolderIDs.Pop();
                    }
                    else
                    {
                        _lastFolderIDs.Push(md.Parent);
                    }

                    foreach (var mds in MetadataStorage.GetByParent(id))
                    {
                        vm.SyncService.NotebooksFilter.Documents.Add(mds);
                    }

                    if (_lastFolderIDs.Count > 0)
                    {
                        vm.SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = MetadataType.CollectionType, VisibleName = "Up .." });
                    }

                    vm.SyncService.NotebooksFilter.SortByFolder();
                }
            }
        }
    }
}
