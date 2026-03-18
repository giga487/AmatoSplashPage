using System;
using System.Windows.Input;

namespace Foundamentals
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            if (execute == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException(nameof(execute));
                throw argumentNullException;
            }
            this._execute = execute;
            this._canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (this._canExecute == null)
            {
                return true;
            }
            
            bool result = this._canExecute.Invoke(parameter);
            return result;
        }

        public void Execute(object? parameter)
        {
            bool canExecute = this.CanExecute(parameter);
            if (canExecute)
            {
                this._execute.Invoke(parameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            EventHandler? eventHandler = this.CanExecuteChanged;
            if (eventHandler != null)
            {
                EventArgs eventArgs = EventArgs.Empty;
                eventHandler.Invoke(this, eventArgs);
            }
        }
    }
}
