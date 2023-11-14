using System.Windows.Input;

namespace Supermarket.Wpf.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object?> execute) : this(execute, canExecute: null)
        {
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            { 
                if (_canExecute is not null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove 
            {
                if (_canExecute is not null)
                {
                    CommandManager.RequerySuggested -= value; 
                }
            }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute is null ? true : _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }

}
