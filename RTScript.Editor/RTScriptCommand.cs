using System;
using System.Windows.Input;

namespace RTScript.Editor
{
    public class RTScriptCommand : ICommand
    {
        private readonly Action _action;
        private readonly Func<bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public RTScriptCommand(Action action, Func<bool> canExecute = default)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
            => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter)
            => _action.Invoke();
    }
}
