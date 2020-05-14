using System;
using System.Windows.Input;

namespace XAMLAssetCreator.Core
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Func<T,bool> _canExecuteEvaluator;
        private readonly Action<T> _methodToExecute;

        public RelayCommand(Action<T> methodToExecute, Func<T,bool> canExecuteEvaluator)
        {
            this._methodToExecute = methodToExecute;
            this._canExecuteEvaluator = canExecuteEvaluator;
        }

        public RelayCommand(Action<T> methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (_canExecuteEvaluator == null)
            {
                return true;
            }

            var result = _canExecuteEvaluator.Invoke((T)parameter);
            return result;
        }

        public void Execute(object parameter)
        {
            if (parameter == default)
            {
                _methodToExecute.Invoke(default);
                return;
            }
            _methodToExecute.Invoke((T)parameter);
        }
    }
}