using System;
using System.Windows.Input;

namespace Slithin.Core
{
    public class DelegateCommand : ICommand
    {
        private readonly Func<object?, bool> _canExecuteHandler;
        private readonly Action<object?> _handler;

        public DelegateCommand(Action<object?> handler, Func<object?, bool> canExecuteHandler = null)
        {
            _handler = handler;
            _canExecuteHandler = canExecuteHandler;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (_canExecuteHandler != null)
            {
                return _canExecuteHandler(parameter);
            }

            return true;
        }

        public void Execute(object? parameter)
        {
            _handler(parameter);
        }
    }
}
