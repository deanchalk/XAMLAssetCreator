using System;
using System.Windows.Input;

namespace XAMLAssetCreator.Core
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T, bool> _canExecFunc;
        private readonly Action<T> _executeAction;

        public RelayCommand(Action<T> executeAction, Func<T, bool> canExecFunc)
        {
            _executeAction = executeAction;
            _canExecFunc = canExecFunc;
        }

        public RelayCommand(Action<T> executeAction)
            : this(executeAction, null)
        {
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_canExecFunc == null) return true;

            var result = _canExecFunc.Invoke((T) parameter);
            return result;
        }

        public void Execute(object parameter)
        {
            if (parameter == default)
            {
                _executeAction.Invoke(default);
                return;
            }

            _executeAction.Invoke((T) parameter);
        }
    }
}