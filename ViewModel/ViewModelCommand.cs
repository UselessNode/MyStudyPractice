using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppDB.ViewModel
{
    internal class ViewModelCommand : ICommand
    {
        private Action<object> executeDarkModeCommand;

        public ViewModelCommand(Action<object> executeDarkModeCommand)
        {
            this.executeDarkModeCommand = executeDarkModeCommand;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void Execute(object parameter)
        {
            //throw new NotImplementedException();
        }
    }
}
