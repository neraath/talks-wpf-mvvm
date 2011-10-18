namespace AggieTweets.ViewModels
{
    using System;
    using System.Windows.Input;

    public class DelegateCommand : ICommand
    {
        private Action<object> commandToExecute;
        private Func<object, bool> checkCanExecute;

        public DelegateCommand(Action<object> command)
            : this(command, null)
        {
        }

        public DelegateCommand(Action<object> command, Func<object, bool> canExecute)
        {
            commandToExecute = command;
            checkCanExecute = canExecute;
        }

        #region Implementation of ICommand

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter)
        {
            commandToExecute(parameter);
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public bool CanExecute(object parameter)
        {
            if (checkCanExecute == null) return true;

            return checkCanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        #endregion
    }
}
