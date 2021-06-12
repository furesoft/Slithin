using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Layout;
using Slithin.Controls;
using Slithin.Core;
using Slithin.Core.Remarkable;
using Slithin.Modals;

namespace Slithin.ViewModels
{
    public class TemplatesPageViewModel : BaseViewModel
    {
        public ICommand OpenAddModalCommand { get; set; }

        public TemplatesPageViewModel()
        {
            OpenAddModalCommand = DialogService.CreateOpenCommand(new AddTemplateModal(), new AddTemplateModalViewModel());
        }
    }
}