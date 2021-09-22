using System;
using System.Linq;
using System.Windows.Input;
using Slithin.Controls;
using Slithin.Core.Remarkable;
using Slithin.Core.Services;

namespace Slithin.Core.Commands
{
    public class ExportCommand : ICommand
    {
        private readonly IExportProviderFactory _exportProviderFactory;

        public ExportCommand(IExportProviderFactory exportProviderFactory)
        {
            _exportProviderFactory = exportProviderFactory;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return parameter is Metadata md
                && md.Type == "DocumentType"
                && _exportProviderFactory.GetAvailableProviders(md).Any();
        }

        public async void Execute(object parameter)
        {
            await DialogService.ShowDialog("Export clicked");
        }
    }
}
