using System.Windows.Input;
using System;

namespace ITU_projekt.Models
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        // Constructor that takes Action<object> for methods that require parameters
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        // Constructor that takes both Action<object> and Predicate<object>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Event to notify when the CanExecute state changes
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // CanExecute logic
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        // Execute logic that takes the parameter
        public void Execute(object parameter) => _execute(parameter);
    }
}
