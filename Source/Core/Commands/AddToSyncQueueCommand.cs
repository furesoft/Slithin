using System;
using System.Diagnostics;
using System.Windows.Input;
using Slithin.Core.Remarkable;

namespace Slithin.Core.Commands
{
    public class AddToSyncQueueCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is Template tmpl)
            {
            }

            Debug.WriteLine("add clicked");
        }
    }
}
