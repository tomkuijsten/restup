using System;
using System.Windows.Input;

namespace Restup.HeadedDemo
{
    public class DelegateCommand<T> : ICommand where T : class
    {
        private readonly Action<T> command;
        private readonly Func<T, bool> canExecuteFunc;

        public DelegateCommand(Action<T> command, Func<T, bool> canExecuteFunc = null)
        {
            this.command = command;
            this.canExecuteFunc = canExecuteFunc;
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteFunc == null || canExecuteFunc(parameter as T);
        }

        public void Execute(object parameter)
        {
            command(parameter as T);
        }

#pragma warning disable 67
        public event EventHandler CanExecuteChanged;
#pragma warning restore 67
    }
}