using System;
using System.Windows.Input;

namespace Slithin
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _handler;

        public DelegateCommand(Action<object?> handler)
        {
            _handler = handler;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _handler(parameter);
        }
    }
}