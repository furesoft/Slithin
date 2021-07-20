using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Slithin.Core.Remarkable;
using Slithin.ViewModels;

namespace Slithin.UI
{
    public static class NotebooksView
    {
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
                    vm.SyncService.NotebooksFilter.Documents.Clear();

                    if (md.VisibleName.Equals("Up .."))
                    {
                        vm.SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = MetadataType.DocumentType, VisibleName = "root" });
                    }
                    else
                    {
                        vm.SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = MetadataType.DocumentType, VisibleName = "tst doc" });
                        vm.SyncService.NotebooksFilter.Documents.Add(new Metadata { Type = MetadataType.CollectionType, VisibleName = "Up .." });
                    }

                    vm.SyncService.NotebooksFilter.SortByFolder();
                }
            }
        }
    }
}
