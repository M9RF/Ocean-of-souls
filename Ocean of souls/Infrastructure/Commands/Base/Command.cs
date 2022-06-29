using System;
using System.Windows.Input;

namespace Ocean_of_souls.Infrastructure.Commands.Base
{
    abstract class Command : ICommand
    {
        /// <summary>
        /// Emitted when the CanExute function returns a new type.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// A function that returns true, false. If true, the command is marked. False - the team didn't go up.
        /// </summary>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// Something that must be done by the team itself.
        /// </summary>
        public abstract void Execute(object parameter);
    }
}
