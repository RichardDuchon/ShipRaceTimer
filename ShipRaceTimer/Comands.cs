using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShipRaceTimer
{
    class Comands : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _action;

        public Comands(Action action)
        {
            _action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
            //return _canExecute.Invoke();            
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }
    }
}
