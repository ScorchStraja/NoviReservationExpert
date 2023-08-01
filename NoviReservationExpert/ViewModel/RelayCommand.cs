using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NoviReservationExpert.ViewModel
{
    public class RelayCommand : ICommand
    {
        Action<object> _TargetExecuteMethod;
        Func<bool> _TargetCanExecuteMethod;
        //public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object> executeMethod)
        {
            _TargetExecuteMethod = executeMethod;
        }
        public RelayCommand(Action<object> executeMethod, Func<bool> canExecuteMethod)
        {
            _TargetExecuteMethod = executeMethod;
            _TargetCanExecuteMethod = canExecuteMethod;
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #region ICommand komponente

        bool ICommand.CanExecute(object parameter)
        {
            if (_TargetCanExecuteMethod != null)
            {
                return _TargetCanExecuteMethod();
            }
            if (_TargetExecuteMethod != null)
            {
                return true;
            }

            return false;
        }

        void ICommand.Execute(object? parameter)
        {
            if (_TargetExecuteMethod != null)
            {
                _TargetExecuteMethod(parameter);
            }
        }

        public event EventHandler CanExecuteChanged = delegate { };

        #endregion
    }
}
